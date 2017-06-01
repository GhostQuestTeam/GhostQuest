using GameSparks.Core;
using UnityEngine;

namespace HauntedCity.Networking
{
    public class AuthService
    {
        private static readonly AuthService instance = new AuthService();

        private AuthService()
        {
        }

        public static AuthService Instance
        {
            get { return instance; }
        }


        public bool IsAuthenticated
        {
            get { return GS.GSPlatform.AuthToken != null; }
        }
        
        public void Register(string login, string nickname, string password)
        {
            new GameSparks.Api.Requests.RegistrationRequest()
                .SetDisplayName(nickname)
                .SetPassword(password)
                .SetUserName(login)
                .Send((response) =>
                    {
                        if (!response.HasErrors)
                        {
                            Debug.Log("Player Registered");
                        }
                        else
                        {
                            Debug.Log("Error Registering Player");
                        }
                    }
                );
        }

        public void Login(string login, string password)
        {
            new GameSparks.Api.Requests.AuthenticationRequest()
                .SetUserName(login)
                .SetPassword(password)
                .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        Debug.Log("Player Authenticated...");
                    }
                    else
                    {
                        Debug.Log("Error Authenticating Player...");
                    }
                });
        }

        public void Logout()
        {
            GS.GSPlatform.AuthToken = null;
//            new GameSparks.Api.Requests.LogEventRequest().SetEventKey("LOGOUT").Send((response) =>
//            {
//                if (!response.HasErrors)
//                {
//                    Debug.Log("Success logout is:" + !GS.Authenticated );
//                }
//                else
//                {
//                    Debug.Log("Error logout...");
//                }
//            });
        }
    }
}