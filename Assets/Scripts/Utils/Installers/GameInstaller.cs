using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking;
using HauntedCity.Networking.GameSparksImpl;
using HauntedCity.Networking.Interfaces;
using HauntedCity.Networking.Social;
using HauntedCity.UI;
using UnityEngine;
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
            Container.Bind<IPlayerStatsManager>().To<GameSparksPlayerStatsManager>().AsSingle();
            Container.Bind<IPOIStatsManager>().To<GameSparksPOIStatsManager>().AsSingle();
            Container.Bind<ISocialAuth>().WithId("FB").To<FacebookAuth>().AsTransient();
            Container.Bind<ISocialAuth>().WithId("G+").To<GooglePlusAuth>().AsTransient();
                        
            AuthService authService = new AuthService(
                Container.ResolveId<ISocialAuth>("FB"),
                Container.ResolveId<ISocialAuth>("G+")
            );
            
            Container.Bind<AuthService>().FromInstance(authService).AsSingle();

            #endregion

        }
    }
 

}