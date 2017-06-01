using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.UI;
using Zenject;

namespace HauntedCity.Utils.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<BattleStateController>().FromNewComponentOnNewGameObject().AsSingle().NonLazy(); 
            Container.Bind<SceneAgregator>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<ScreenManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            
            Container.Bind<WeaponLoader>().AsSingle();
            Container.Bind<BattleStatsCalculator>().AsSingle();
        }
    }
 

}