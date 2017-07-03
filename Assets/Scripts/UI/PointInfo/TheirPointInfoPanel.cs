using UnityEngine.UI;
using UnityEngine;

namespace HauntedCity.UI.PointInfo
{
    public class TheirPointInfoPanel:PointInfoPanel
    {
        public Text PointOwner;

        public override void UpdateView(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            base.UpdateView(point);
            PointOwner.text = _point.displayName;
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
            Hide();
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