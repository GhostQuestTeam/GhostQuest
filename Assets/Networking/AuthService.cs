using UnityEngine;

namespace Networking
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
    }
}