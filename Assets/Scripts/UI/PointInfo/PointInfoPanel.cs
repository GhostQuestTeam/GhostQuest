using HauntedCity.GameMechanics.Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;

namespace HauntedCity.UI.PointInfo
{
    public class PointInfoPanel:Panel
    {
        private GameSparksPOIsExtraction.ExtractedPointMetadata _point;

        [Inject] private GameController _gameController;
        
        public Text PointOwner;
        public Text PointInfo;
        public GhostsPanel GhostPanel;

        public void UpdateView(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            _point = point;
            PointOwner.text = _point.uoid;
            PointInfo.text = _point.LatLon.ToString();
            GhostPanel.UpdateView(_point);
        }

        public void Show(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            UpdateView(point);
            Show();
        }


        private GameSparksBattle _gsb;

        public void OnStartCapture(object sender, GameSparksBattle.POI_START_CAP_ev_arg arg)
        {
            if (arg.poid == _point.poid)
            {
                //_gsb.OnPOIStartCap -= OnStartCapture;
                if (arg.isError || !arg.isStarted)
                {
                    //WE DID NOT START - TODO SHOW ERROR
                }
                else
                {
                    _gameController.StartBattle(_point);
                }
                
            }
        }

        public void ToFight()
        {
            if (_gsb == null)
            {
                _gsb = GameObject.Find("GameSparks").GetComponent<GameSparksBattle>();
                _gsb.OnPOIStartCap += OnStartCapture;
            }
            _gsb.sendStartCapture(_point.poid);
        }

        public void OnDestroy()
        {
            _gsb.OnPOIStartCap -= OnStartCapture;
        }

    }
}