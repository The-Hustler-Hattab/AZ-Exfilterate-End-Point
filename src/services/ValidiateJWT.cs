using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Exfilterate.utilites;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace First_Demo_finc.services
{
    internal static class ValidiateJWT
    {

        private static readonly HttpClient _httpClient = new HttpClient();
        private static JwkSet _jwkSet;

        readonly static string _jwkSetUrl = Constants.JWK_ENDPOINT;

        private async static Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                if (_jwkSet==null)
                {
                    // Fetch the public keys from Okta
                    HttpResponseMessage response = await _httpClient.GetAsync(_jwkSetUrl);
                    response.EnsureSuccessStatusCode();
                    string jwkSetJson = await response.Content.ReadAsStringAsync();

                    // Parse the JSON representation of public keys
                    _jwkSet = JwkSet.FromJson(jwkSetJson, mapper: new JsonMapper());
                }



                // Obtain the public keys from the set
                List<Jwk> keys = _jwkSet.Keys;

                // Iterate through each key and try to validate the token
                foreach (Jwk chosenKey in keys)
                {
                    try
                    {
                        // Perform JWT verification using the chosen public key

                        if(VerifyJwt(token, chosenKey))
                        {
                        return true;

                        }
                    }
                    catch (Exception e)
                    {
                        // Log and continue to the next key in case of any exception
                        Console.WriteLine($"Error validating token with key: {chosenKey.KeyId}. {e.Message}");
                    }
                }

                // Token validation failed with all keys
                return false;
            }
            catch (Exception e)
            {
                
                // Token parsing or verification failed
                Console.WriteLine($"Error parsing or verifying token. {e.Message}");
                return false;
            }
        }

        public static bool VerifyJwt(string jwtToken, Jwk chosenKey)
        {
            RSA rsa = chosenKey.RsaKey(); // Convert JWK to RSA

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // You may want to set these to true based on your requirements
                ValidateAudience = false,
                IssuerSigningKey = new RsaSecurityKey(rsa)
            };

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(jwtToken, validationParameters, out var securityToken);
                return true; // Token is valid
            }
            catch (SecurityTokenValidationException)
            {
                return false; // Token validation failed
            }
        }



        private static string ExtractBearerToken(HttpRequestData req)
        {
            // Check if the Authorization header is present
            if (req.Headers.TryGetValues("Authorization", out var headerValues))
            {
                // Get the first value of the Authorization header
                var header = headerValues.FirstOrDefault();

                // Check if the header starts with "Bearer "
                if (header != null && header.StartsWith("Bearer "))
                {
                    // Extract the token excluding "Bearer " prefix
                    return header.Substring("Bearer ".Length);
                }
            }

            // Authorization header is not present or does not contain a bearer token
            return null;
        }

        public static async Task<bool> ValidateToken(HttpRequestData req)
        {
            // Extract the token from the Authorization header
            string token = ExtractBearerToken(req);

            // Check if the token is present and valid
            if (token != null && await ValidateTokenAsync(token))
            {
                // Validate the token
                return true;
            }
            else
            {
                return false;
            }


        }   


    }
}
