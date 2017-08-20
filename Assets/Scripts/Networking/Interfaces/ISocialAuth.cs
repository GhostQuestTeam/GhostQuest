using System;
using GameSparks.Api.Responses;

namespace HauntedCity.Networking.Interfaces
{
    public interface ISocialAuth
    {
        event Action<AuthenticationResponse> OnAuthDone;
        void DoAuth();
        void Logout();
    }
}