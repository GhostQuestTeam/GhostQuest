namespace BattleSystem
{
    public delegate void ShellEffect(BattleStats stats);

    public class ShellInfo
    {
        public ShellEffect Effect;
        public int Damage { get; set; }

        public ShellInfo(int damage, ShellEffect effect = null)
        {
            Damage = damage;
        }
    }

    public class WeaponInfo
    {
        public string Id { get; }
        public int ShootCost { get; set; }
        public double Cooldown { get; set; }
        public ShellInfo Shell { get; }

        private WeaponInfo(string id, int shootCost, double cooldown)
        {
            Id = id;
            ShootCost = shootCost;
            Cooldown = cooldown;
        }

        public WeaponInfo(string id, int shootCost, double cooldown, ShellInfo shell) : this(id, shootCost, cooldown)
        {
            Shell = shell;
        }

        public WeaponInfo(string id, int shootCost, double cooldown, int damage, ShellEffect effect = null) :
            this(id, shootCost, cooldown)
        {
            Shell = new ShellInfo(damage, effect);
        }
    }
}