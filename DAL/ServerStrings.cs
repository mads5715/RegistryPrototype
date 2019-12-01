using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public static class ServerStrings
    {
        /// <summary>
        /// Returns the connection string a connection string for the Docker-compose MySQL, 
        /// or if one is specified in the environment with ConnectionString: server=...;userid+... etc. this will take priority.
        /// </summary>
        /// <returns>string: connectionstring</returns>
        public static string GetMySQLConnectionString() {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ConnectionString"))) {
                return "server = mysqlServer; user id = RegistryClone; password = RegistryClone2019; port = 3306; database = NPMRegistryClone;";
            }
            else {
                return Environment.GetEnvironmentVariable("ConnectionString");
            }
        }

        public static string GetString(string stringName) {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(stringName))){
                return ""; //User Cryptographic Random for the salt
            } else {
                return Environment.GetEnvironmentVariable(stringName);
            }
        }
    }
}
