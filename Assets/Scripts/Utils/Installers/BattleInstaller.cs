using HauntedCity.GameMechanics.BattleSystem;
using Zenject;
using UnityEngine;
namespace HauntedCity.Utils.Installers
{
    public class BattleInstaller:MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerBattleBehavior>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<PlayerBattleController>()
                .FromResolveGetter<PlayerBattleBehavior>((p) => p.BattleController)
                .AsSingle();

        }
    }
}