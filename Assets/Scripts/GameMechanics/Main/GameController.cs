using System;
using System.Collections.Generic;
using GameSparks.Core;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.SkillSystem;
using HauntedCity.Networking;
using HauntedCity.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace HauntedCity.GameMechanics.Main
{
    public class GameController : MonoBehaviour
    {
        public static PlayerGameStats GameStats = new PlayerGameStats();

        public event Action OnPlayerStatsUpdate;

        private BattleStateController _battleStateController;
        private SceneAgregator _sceneAgregator;
        private StorageService _storageService;

        [Inject] private MessageRetranslator _messageRetranslator;
        [Inject] private AuthService _authService;

        
        public string[] AllowableGhosts = { "shadow_skull", "devil_mask","skull_ghost" };

        private GameSparksPOIsExtraction.ExtractedPointMetadata _currentPOImeta;

        private GameSparksBattle _gsb;
        private int _lastScore;

        [Inject]
        public void InitializeDependencies(BattleStateController battleStateController, 
            SceneAgregator sceneAgregator,
            StorageService storageService)
        {
            _battleStateController = battleStateController;
            _sceneAgregator = sceneAgregator;
            _storageService = storageService;
        }
        
        void Awake()
        {
        }

        void Start()
        {
            _gsb = GameObject.FindObjectOfType<GameSparksBattle>();
            _gsb.OnPOISuccessCap += OnSuccessCapture;
            _gsb.OnPOIFailCapConfirm += OnFailCaptureConfirm;
            GameStats.OnAttributesUpgrade += () => _storageService.SavePlayer(GameStats);
            
            _sceneAgregator.OnSceneChange += OnSceneChange;
            _sceneAgregator.OnAllScenesLoad += OnAllScenesLoad;
            _battleStateController.OnWon += BattleWonHandle;
            _battleStateController.OnLose += BattleLoseHandle;

            _messageRetranslator.Subscribe(MessageType.PLAYER_STATS_UPDATE, UpdateStats);
            
            _storageService.OnLoad += OnPlayerLoad;
        }

        void UpdateStats(GSData data)
        {
            GSRequestData requestData = new GSRequestData(data); 
            GameStats.GSData = requestData;
            if (OnPlayerStatsUpdate != null)
            {
                OnPlayerStatsUpdate();
            }
        }
        
        public void OnDestroy()
        {
            _gsb.OnPOISuccessCap -= OnSuccessCapture;
            _gsb.OnPOIFailCapConfirm -= OnFailCaptureConfirm;
        }

        public void OnSuccessCapture(object sender, GameSparksBattle.POI_SUCESS_CAP_ev_arg arg)
        {
            if(arg.poid == _currentPOImeta.poid)
            {
                //_gsb.OnPOISuccessCap -= OnSuccessCapture;
//                if(!arg.isError && arg.isSuccess)
//                {
                    GameStats.AddExp(_lastScore);
                    _storageService.SavePlayer(GameStats);

                    //WE REALLY SUCCEDED CAPTURE
//                }
//                else
//                {
//                    WE FAILED CAPTURE
//                }
                GameObject.Find("BattleRoot").SetActive(false);
                _sceneAgregator.switchToScene("map");
//                Debug.Log("Level: " + GameStats.PlayerExperience.Level + "  " + GameStats.CurrentExp + "/" + GameStats.ExpToLevel);
            }
        }

        public void OnFailCaptureConfirm(object sender, GameSparksBattle.POI_FAIL_CAP_CONFIRM_ev_arg arg)
        {
            //WE CONFIRMED CAPTURE AND GOT RESULT - SHALL WE DO SMTH?
        }

        private void OnAllScenesLoad()
        {
            //_sceneAgregator.switchToScene("battle");
        }

        //TODO
        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "start_scene")
            {
                var mapRoot =GameObject.Find("LocationProviderRoot");
                if (mapRoot != null)
                {
                    mapRoot.SetActive(false);
                }
            }
        }

        public void StartGame()
        {
            _sceneAgregator.switchToScene("map");
        }

        private void OnPlayerLoad()
        {
            if (_storageService.PlayerStats != null)
            {
                GameStats = _storageService.PlayerStats;
            }
        }
        
        private void OnSceneChange(string sceneName)
        {
            GameStats.StorageService = _storageService;
            if (sceneName == "battle")
            {
                _battleStateController.StartBattle(new Dictionary<string, int>( _currentPOImeta.enemies) );
            }
            
        }

        public void StartBattle(GameSparksPOIsExtraction.ExtractedPointMetadata meta)
        {
            _currentPOImeta = meta;
            
            _sceneAgregator.switchToScene("battle");
        }

        public void StartBattle()
        {
            GameObject.Find("LocationProviderRoot").SetActive(false);

            _sceneAgregator.switchToScene("battle");
        }

        public void BattleWonHandle(int score)
        {
            _gsb.sendSuccessCapture(_currentPOImeta.poid);
            _currentPOImeta.displayName = _authService.Nickname;
            _lastScore = score;
            //DONT WE DISABLE OURSELVES AND SCRIPT DOES NOT FINISH?
          
        }

        public void BattleLoseHandle()
        {
            _lastScore = 0;
            _gsb.sendFailCaptureConfirm(_currentPOImeta.poid);
            Debug.Log("Lose in battle");
            GameObject.Find("BattleRoot").SetActive(false);
            _sceneAgregator.switchToScene("map");
            _storageService.SavePlayer(GameStats);
        }
        
    }
}