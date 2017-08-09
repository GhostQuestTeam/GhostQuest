using HauntedCity.Geo;
using UnityEngine.UI;
using UnityEngine;

namespace HauntedCity.UI.PointInfo
{
    public class TheirPointInfoPanel:PointInfoPanel
    {
        public Text PointOwner;

        public override void UpdateView(PointOfInterestData point)
        {
            base.UpdateView(point);
            PointOwner.text = _point.DisplayName;
        }

        private GameSparksBattle _gsb;

        public void OnStartCapture(object sender, GameSparksBattle.POI_START_CAP_ev_arg arg)
        {
            if (arg.poid == _point.Poid)
            {
                //_gsb.OnPOIStartCap -= OnStartCapture;
                if (arg.isError || !arg.isStarted)
                {
                    //WE DID NOT START - TODO SHOW ERROR
                }
                else
                {
                    Hide();
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
            _gsb.sendStartCapture(_point.Poid);
        }

        public void OnDestroy()
        {
            _gsb.OnPOIStartCap -= OnStartCapture;
        }
    }
}