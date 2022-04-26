using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarStore.Authentication.Commons
{
    public static class CarStoreConstants
    {
        public const string UserRole = "User";
        public const string SuperUserRole = "SuperUser";

        //Authentication config
        public const string AuthenSecretKey = "CarStore@123";
        //public const string AdminClientId = "AdminClientId";
        //public const string AdminClientName = "AdminClientName";
        //public const string AdminResponseType = "code id_token";
        //public const string AdminAudience = "CarStore.APIGateway";
    }
}
