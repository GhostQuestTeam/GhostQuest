using System.Collections.Generic;

namespace HauntedCity.GameMechanics.Main
{
    public static class GameConfiguration
    {
        public static HashSet<string> AllowableEnemies = new HashSet<string>()
        {
            "skull_ghost",
            "shadow_skull",
            "white_skull",
            "devil_mask",
            "skeleton",
            "headless"
        };
    }
}