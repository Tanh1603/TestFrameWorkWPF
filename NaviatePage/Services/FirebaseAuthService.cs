using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviatePage.Services
{
    public class FirebaseAuthService
    {
        private readonly FirebaseAuthClient _client;

        public FirebaseAuthService(string apiKey)
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = apiKey,
                AuthDomain = "tanh-first-project.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                },
                UserRepository = new FileUserRepository("FirebaseSample")
            };

            _client = new FirebaseAuthClient(config);
        }

        public async Task<string> RegisterUser(string email, string password)
        {
            var auth = await _client.CreateUserWithEmailAndPasswordAsync(email, password);
            return auth.User.Uid;
        }

        public async Task<string> LoginUser(string email, string password)
        {
            var auth = await _client.SignInWithEmailAndPasswordAsync(email, password);
            return auth.User.Uid;
        }
    }
}