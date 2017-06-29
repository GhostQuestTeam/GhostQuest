using System.Collections.Generic;
using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.SkillSystem;
using HauntedCity.Networking;
using HauntedCity.Utils;
using UnityEngine;
using Zenject;

namespace HauntedCity.GameMechanics.Main
{
    public class GameController : MonoBehaviour
    {
        public static PlayerGameStats GameStats = new PlayerGameStats();

        private BattleStateController _battleStateController;
        private SceneAgregator _sceneAgregator;
        private StorageService _storageService;

        public string[] AllowableGhosts = { "shadow_skull", "devil_mask","skull_ghost" };

        private GameSparksPOIsExtraction.ExtractedPointMetadata _currentPOImeta;

        private GameSparksBattle _gsb;

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
            _gsb = GameObject.Find("GameSparks").GetComponent<GameSparksBattle>();
            _gsb.OnPOISuccessCap += OnSuccessCapture;
            _gsb.OnPOIFailCapConfirm += OnFailCaptureConfirm;
            GameStats.OnAttributesUpgrade += () => _storageService.SavePlayer(GameStats);
            
            _sceneAgregator.OnSceneChange += OnSceneChange;
            _sceneAgregator.OnAllScenesLoad += OnAllScenesLoad;
            _battleStateController.OnWon += BattleWonHandle;
            _battleStateController.OnLose += BattleLoseHandle;

            _storageService.OnLoad += OnPlayerLoad;
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
                if(!arg.isError && arg.isSuccess)
                {
                    //WE REALLY SUCCEDED CAPTURE
                }
                else
                {
                    //WE FAILED CAPTURE
                }
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
            if (sceneName == "battle")
            {
                _battleStateController.StartBattle(_currentPOImeta.enemies);
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

        public Dictionary<string, int> RandomGhosts()
        {
            var result = new Dictionary<string, int>();
            foreach (var ghost in AllowableGhosts)
            {
                result.Add(ghost, Random.Range(3, 5));
            }
            return result;
        }


        public void BattleWonHandle(int score)
        {
            GameStats.AddExp(score);
            _gsb.sendSuccessCapture(_currentPOImeta.poid);
            //DONT WE DISABLE OURSELVES AND SCRIPT DOES NOT FINISH?
            GameObject.Find("BattleRoot").SetActive(false);


            _sceneAgregator.switchToScene("map");
            Debug.Log("Level: " + GameStats.Level + "  " + GameStats.CurrentExp + "/" + GameStats.ExpToLevel);
            _storageService.SavePlayer(GameStats);
        }

        public void BattleLoseHandle()
        {
            Debug.Log("Lose in battle");
            GameObject.Find("BattleRoot").SetActive(false);
            _gsb.sendFailCaptureConfirm(_currentPOImeta.poid);
            _sceneAgregator.switchToScene("map");
            _storageService.SavePlayer(GameStats);
        }
        
    }
}