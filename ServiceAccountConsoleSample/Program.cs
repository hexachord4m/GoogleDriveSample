using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace ServiceAccountConsoleSample
{
    class Program
    {
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Drive API By Service Account";
        
        public static void Main(string[] args)
        {
            ICredential credential;
            string credentialFileName = "credentials.json";

            using (var stream = new FileStream(credentialFileName, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                     .CreateScoped(Scopes).UnderlyingCredential;
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
