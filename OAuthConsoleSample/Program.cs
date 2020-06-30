using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OAuthConsoleSample
{
    class Program
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Drive API By OAuth";

        static void Main(string[] args)
        {
            UserCredential credential;
            string credentialFileName = "credentials.json";

            using (var stream = new FileStream(credentialFileName, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Request service.
            FilesResource.ListRequest listRequest = service.Files.List();
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            Console.WriteLine("FILE COUNT IS: {0}", files.Count);

            Console.Read();
        }
    }
}
