using System;
using System.Collections.Generic;
using GameSparks.Core;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.SkillSystem;
using HauntedCity.Geo;
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
        public BattleStateController.BattleResult LastBattleResult { get; private set; }

        private PointOfInterestData _currentPOImeta;

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
            
            _sceneAgregator.OnSceneChange += OnSceneChange;
            _sceneAgregator.OnAllScenesLoad += OnAllScenesLoad;
            _battleStateController.OnBattleEnd += BattleEndHandle;

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
            _battleStateController.OnBattleEnd -= BattleEndHandle;
            
             
            _sceneAgregator.OnSceneChange -= OnSceneChange;
            _sceneAgregator.OnAllScenesLoad -= OnAllScenesLoad;

        }

        public void OnSuccessCapture(object sender, GameSparksBattle.POI_SUCESS_CAP_ev_arg arg)
        {
            if(arg.poid == _currentPOImeta.Poid)
            {
                //GameStats.AddExp(LastBattleResult.EarnedExp);
                GameObject.Find("BattleRoot").SetActive(false);
                _sceneAgregator.switchToScene("map");
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
                _battleStateController.StartBattle(new Dictionary<string, int>( _currentPOImeta.Enemies) );
            }
            
        }

        public void StartBattle(PointOfInterestData meta)
        {
            _currentPOImeta = meta;
            
            _sceneAgregator.switchToScene("battle");
        }

        public void StartBattle()
        {
            GameObject.Find("LocationProviderRoot").SetActive(false);

            _sceneAgregator.switchToScene("battle");
        }

        
        public void BattleEndHandle(BattleStateController.BattleResult battleResult)
        {
            LastBattleResult = battleResult;
            switch (battleResult.Type)
            {
                case BattleStateController.BattleResultType.WON:
                    _gsb.sendSuccessCapture(_currentPOImeta.Poid);
                    _currentPOImeta.DisplayName = _authService.Nickname;
                    break;
                case BattleStateController.BattleResultType.LOSE:
                    _gsb.sendFailCaptureConfirm(_currentPOImeta.Poid);
                    GameObject.Find("BattleRoot").SetActive(false);
                    _sceneAgregator.switchToScene("map");
                    break;
                case BattleStateController.BattleResultType.STOPED:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        
        
    }
}