using HauntedCity.Networking.Interfaces;
using UnityEngine;
using Zenject;

namespace HauntedCity.UI
{
    public class DiedPanel:Panel
    {
        [Inject] private IPlayerStatsManager _playerStatsManager;

        public void Resurrect()
        {
            _playerStatsManager.Resurrect();
            Hide();
        }
    }
}