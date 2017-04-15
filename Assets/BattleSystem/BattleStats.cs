namespace BattleSystem
{
    public class BattleStats
    {
        public Solidity Solidity { get; set; }

        public uint MaxHealth
        {
            get { return Solidity.MaxHealth; }
            set { Solidity.MaxHealth = value; }
        }

        public int CurrentHealth => Solidity.CurrentHealth;

        public BattleStats(Solidity solidity)
        {
            Solidity = solidity;
        }

    }
}