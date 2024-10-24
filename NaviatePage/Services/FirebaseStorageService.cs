using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;

namespace NaviatePage.Services
{
    public class FirebaseStorageService
    {
        private readonly string _firebaseBucket;

        public FirebaseStorageService(string firebaseBucket)
        {
            _firebaseBucket = firebaseBucket;
        }

        public async Task<string> UploadFileAsync(string localFilePath)
        {
            // Đường dẫn tới file bạn muốn upload
            var url = new Uri(localFilePath).LocalPath;
            using (var stream = File.Open(url, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // FirebaseStorage upload logic
                var task = new FirebaseStorage(
                    _firebaseBucket)
                    .Child("Test_Image")
                    .Child(Path.GetFileName(url))
                    .PutAsync(stream);

                var downloadUrl = await task;

                return downloadUrl;
            }
        }
    }
}