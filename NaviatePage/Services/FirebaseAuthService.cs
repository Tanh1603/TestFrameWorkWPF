using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using System.Net.Http;
using Newtonsoft.Json;
using Azure.Core;
using System.IO.Pipes;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Net.Mail;
using System.Net;

namespace NaviatePage.Services
{
    public class FirebaseAuthService
    {
        private readonly FirebaseAuthClient _client;
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        public string CODE;

        public FirebaseAuthService(string apiKey)
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = apiKey,
                AuthDomain = "tanh-first-project.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider(),
                },
                UserRepository = new FileUserRepository("FirebaseSample")
            };
            _accessToken = apiKey;
            _httpClient = new HttpClient();
            _client = new FirebaseAuthClient(config);
        }

        public async Task<string> RegisterUser(string email, string password, string displayName = null)
        {
            try
            {
                var auth = await _client.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
                return auth.User.Uid; // Trả về UID khi đăng ký thành công
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                return string.Empty;
            }
        }

        public async Task<string> LoginUser(string email, string password)
        {
            try
            {
                var auth = await _client.SignInWithEmailAndPasswordAsync(email, password);
                return auth.User.Uid;
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                return string.Empty;
            }
        }

        public async Task<bool> ResetPassword(string email)
        {
            try
            {
                await _client.ResetEmailPasswordAsync(email);
                return true;
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                return false;
            }
        }

        public void SignOut()
        {
            try
            {
                _client.SignOut();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetDisplayName()
        {
            try
            {
                return _client.User.Info.DisplayName;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}