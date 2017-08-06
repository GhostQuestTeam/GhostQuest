using System;

namespace HauntedCity.GameMechanics.SkillSystem
{
    public class Experience
    {        
        public int Level { get; private set; }
        public int CurrentExp { get; private set; }
        public int ExpToLevel { get; private set; }

        private void UpdateExpToNextLevel()
        {
            ExpToLevel = 80 * Level * Level * Level + 50 * Level + 1000;
        }

        private void NextLevel()
        {
            Level++;
            CurrentExp = 0;
            UpdateExpToNextLevel();
        }

        public int AddExp(int exp)
        {
            int earnedLevels = 0;
            do
            {
                var tmp = (ExpToLevel - CurrentExp);
                if (exp >= tmp)
                {
                    exp -= tmp;
                    NextLevel();
                    earnedLevels++;
                    if (exp < 0)
                    {
                        break;
                    }
                }
            } while (CurrentExp >= ExpToLevel);
            CurrentExp += exp;
            return earnedLevels;
        }

        public Experience(int level=1, int exp=0)
        {
            Level = level;
            ExpToLevel = 0;
            UpdateExpToNextLevel();
            AddExp(exp);
        }
    }
}