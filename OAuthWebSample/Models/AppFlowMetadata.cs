using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;

namespace OAuthWebSample.Models
{
    public class AppFlowMetadata : FlowMetadata
    {
        private static readonly string credentialFilePath = HostingEnvironment.MapPath("~/Client/credentials.json");
        private static readonly string tokenFilePath = HostingEnvironment.MapPath("~/Client/token.json");

        private static IAuthorizationCodeFlow flow;

        public AppFlowMetadata()
        {
            using (var stream = new FileStream(credentialFilePath, FileMode.Open, FileAccess.Read))
            {
                flow = new OfflineAccessGoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = GoogleClientSecrets.Load(stream).Secrets,
                    Scopes = new[] { DriveService.Scope.Drive },
                    DataStore = new FileDataStore(tokenFilePath),
                });

            }
        }

        public override string GetUserId(Controller controller)
        {
            return "user";
        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }

        public override string AuthCallback
        {
            get
            {
                if (string.IsNullOrEmpty(ApplicationPath))
                {
                    return base.AuthCallback;
                }
                else
                {
                    return ApplicationPath + "/AuthCallback/IndexAsync";
                }
            }
        }
        public string ApplicationPath { get; set; }
    }
}