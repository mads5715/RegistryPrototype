using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RegistryPrototype.DAL
{
    public sealed class TokenCache
    {
        private static TokenCache instance = null;
        private static readonly object padlock = new object();
        private static readonly Dictionary<string, string> userTokens = new Dictionary<string, string>();

        private TokenCache()
        {
            
        }
        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static TokenCache Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {

                        instance = new TokenCache();
                    }
                    return instance;
                }
            }
        }
        public string GetTokenForUser(string userName) {
            if (userTokens.ContainsKey(userName))
            {
                var outString = "";
                userTokens.TryGetValue(userName,out outString);
                return outString;
            }
            else
            {
                //Generate token
                var preHashToken = Guid.NewGuid();
                var hashedToken = ComputeSha256Hash(preHashToken.ToString()+"SecretStupidSalt...");
                var token = Convert.ToString(hashedToken);
                userTokens.Add(userName,token);
                return token;
            }
        }

        public bool ValidateToken(string token) {
            return userTokens.ContainsValue(token);
        }

        private string GetKeyFromValue(string input) {
            foreach (var item in userTokens)
            {
                if (item.Value == input)
                {
                    return item.Key;
                }
            }
            return null;
        }

        public void RemoveToken(string token)
        {
            if (userTokens.ContainsValue(token))
            {
                userTokens.Remove(GetKeyFromValue(token));
            }
            else
            {
                return;
            }
        }
    }
}
