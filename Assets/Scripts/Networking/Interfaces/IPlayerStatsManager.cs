using System.Collections.Generic;

namespace HauntedCity.Networking.Interfaces
{
    public interface IPlayerStatsManager
    {
        void UpgradeAttributes(int survivability, int endurance, int power);
        void BuyWeapon(string weaponID);
        void ChooseWeapons(List<string> weaponsIDs);
        void Resurrect();
    }
}