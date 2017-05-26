using UnityEngine;

namespace Networking
{
    public class AuthController
    {
        private static readonly AuthController instance = new AuthController();

        private AuthController()
        {
        }

        public static AuthController Instance
        {
            get { return instance; }
        }

        public void Register()
        {
            new GameSparks.Api.Requests.RegistrationRequest()
                .SetDisplayName("Randy")
                .SetPassword("test_password_123456")
                .SetUserName("Test User 1")
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