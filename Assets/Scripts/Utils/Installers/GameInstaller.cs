using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking;
using HauntedCity.UI;
using Zenject;

namespace HauntedCity.Utils.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            #region UnityComponents
            Container.Bind<GameController>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<BattleStateController>().FromComponentInHierarchy().AsSingle().NonLazy(); 
            Container.Bind<SceneAgregator>().FromComponentInHierarchy().AsSingle().NonLazy();
            Container.Bind<ScreenManager>().FromComponentInHierarchy().AsSingle().NonLazy();
            #endregion
            
            #region NormalClasses
            Container.Bind<WeaponLoader>().AsSingle();
            Container.Bind<BattleStatsCalculator>().AsSingle();
            Container.Bind<StorageService>().AsSingle();
            #endregion

        }
    }
 

}