using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking;
using HauntedCity.Networking.GameSparksImpl;
using HauntedCity.Networking.Interfaces;
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
            #endregion
            
            #region NormalClasses
            Container.Bind<MessageRetranslator>().AsSingle();
            Container.Bind<WeaponLoader>().AsSingle();
            Container.Bind<BattleStatsCalculator>().AsSingle();
            Container.Bind<StorageService>().AsSingle();
            Container.Bind<LeaderboardService>().AsSingle();
            Container.Bind<AuthService>().AsSingle();
            Container.Bind<IPlayerStatsManager>().To<GameSparksPlayerStatsManager>().AsSingle();
            Container.Bind<IPOIStatsManager>().To<GameSparksPOIStatsManager>().AsSingle();
            #endregion

        }
    }
 

}