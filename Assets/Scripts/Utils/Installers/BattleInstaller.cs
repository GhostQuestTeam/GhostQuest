using HauntedCity.GameMechanics.BattleSystem;
using Zenject;

namespace HauntedCity.Utils.Installers
{
    public class BattleInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerBattleBehavior>()
                .FromComponentInNewPrefabResource("Player")
                .WithGameObjectName("Player")
                .AsSingle()
                .NonLazy();
        }
    }
}