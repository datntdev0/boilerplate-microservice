using System.Net;

namespace datntdev.Microservice.Shared.Common;

public class Constants
{
    public class Application 
    {
        public const string AuthenticationScheme = "Cookies";
    }

    public class Endpoints
    {
        public const string Health = "/health";
        public const string Aliveness = "/alive";
        public const string AuthSignIn = "/auth/signin";
        public const string AuthSignUp = "/auth/signup";
        public const string AuthSignOut = "/auth/signout";
    }
}
