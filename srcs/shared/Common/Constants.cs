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
        public const string OAuth2Token = "/connect/token";
        public const string OAuth2Authorize = "/connect/authorize";
    }

    public enum Permissions
    {
        None = 0,

        Tenancy = 1000,
        Tenancy_Read = 1001,
        Tenancy_Write = 1002,

        Users = 2000,
        Users_Read = 2001,
        Users_Write = 2002,

        Roles = 3000,
        Roles_Read = 3001,
        Roles_Write = 3002,
    }

    public enum TenancySides
    {
        Host = 0x01,
        Tenant = 0x10,
    }
}
