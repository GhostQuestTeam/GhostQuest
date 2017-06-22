using NUnit.Framework;
using HauntedCity.GameMechanics.BattleSystem;

namespace HauntedCityTests.BattleSystemTests
{
    [TestFixture]
    public class SolidityTest
    {
        private Solidity _testSolidity;
        private const uint _MAX_HEALTH = 100;
        private const int _DEFENCE = 5;

        [SetUp]
        public void Setup()
        {
            _testSolidity = new Solidity(_MAX_HEALTH, _DEFENCE);
        }

        [Test]
        public void DefenceReducesDamage()
        {
            var damage = 10;
            var expectedHealth = _testSolidity.CurrentHealth - (damage - _DEFENCE);

            _testSolidity.Attack(damage);

            Assert.AreEqual(expectedHealth, _testSolidity.CurrentHealth);
        }

        [Test]
        public void DamageIsZeroIfDefenceGreaterDamage()
        {
            var damage = 2;
            var expectedHealth = _testSolidity.CurrentHealth;

            _testSolidity.Attack(damage);

            Assert.AreEqual(expectedHealth, _testSolidity.CurrentHealth);
        }

        [Test]
        public void BigDamageNullifiesHealth()
        {
            var damage = _DEFENCE + (int)_MAX_HEALTH + 100;
            var expectedHealth = 0;


            _testSolidity.Attack(damage);

            Assert.AreEqual(expectedHealth, _testSolidity.CurrentHealth);
        }

        [Test]
        public void ObjectWithNotZeroHealthIsAlive()
        {
            var aliveSolidity = new Solidity(_MAX_HEALTH, _DEFENCE, 0, 1);

            Assert.True(aliveSolidity.IsAlive());
        }

        [Test]
        public void ObjectWithZeroHealthIsNotAlive()
        {
            var deadSolidity = new Solidity(_MAX_HEALTH, _DEFENCE, 0, 0);

            Assert.False(deadSolidity.IsAlive());
        }

        [Test]
        public void HealWorksOnAliveObject()
        {
            var health = 10;
            var aliveSolidity = new Solidity(_MAX_HEALTH, _DEFENCE, 0, health);
            var healPoints = 30;
            var expectedHealth = healPoints + health;

            aliveSolidity.Heal(healPoints);

            Assert.AreEqual(expectedHealth, aliveSolidity.CurrentHealth);
        }

        [Test]
        public void HealNotWorksOnDeathObject()
        {
            var deathSolidity = new Solidity(_MAX_HEALTH, _DEFENCE, 0, 0);
            var expectedHealth = 0;

            deathSolidity.Heal(10);

            Assert.AreEqual(expectedHealth, deathSolidity.CurrentHealth);
        }

        [Test]
        public void CurrentHealthNotGreaterMaxHealth()
        {
            var expectedHealth = _MAX_HEALTH;

            _testSolidity.Heal(10);

            Assert.AreEqual(expectedHealth, _testSolidity.CurrentHealth);
        }

    }
}