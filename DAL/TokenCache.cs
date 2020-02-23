/*
    Copyright (C) 2019  Mads Dürr-Wium

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
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
