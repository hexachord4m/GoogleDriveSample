using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using OAuthWebSample.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace OAuthWebSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata() { ApplicationPath = Request.ApplicationPath }).AuthorizeAsync(cancellationToken);
            if (result.Credential != null)
            {
                ViewBag.Message = "認証済み クライアントID:" + ((AuthorizationCodeFlow)result.Credential.Flow).ClientSecrets.ClientId;
            }
            else
            {
                ViewBag.Message = "未認証";
            }

            return View();
        }

        public async Task<ActionResult> AuthAsync(CancellationToken cancellationToken)
        {
            // 認証ファイル保存
            if (Request.Files != null && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                string credentialFilePath = HostingEnvironment.MapPath("~/Client/credentials.json");
                Request.Files[0].SaveAs(credentialFilePath);
            }
            else
            {
                ViewBag.Message = "認証ファイルをアップロードしてください";
                return View("Index");
            }

            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata() { ApplicationPath = Request.ApplicationPath }).AuthorizeAsync(cancellationToken);
            if (result.Credential != null)
            {
                ViewBag.Message = "認証済み";
                return View("Index");
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }

            return View("Index");
        }

        public async Task<ActionResult> GooleDrive(GoogleDriveModel model, CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata() { ApplicationPath = Request.ApplicationPath }).AuthorizeAsync(cancellationToken);
            if (result.Credential != null)
            {
                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "Drive API OAuth Sample.",
                });

                // Define parameters of request.
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 10;
                listRequest.Fields = "nextPageToken, files(id, name)";

                // List files.
                var files = listRequest.Execute().Files;
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        var fileData = new FileData() { Name = file.Name, Id = file.Id };
                        model.FileDataList.Add(fileData);
                    }
                }
            }
            else
            {
                ViewBag.Message = "認証してください";
            }

            return View(model);
        }

        public ActionResult Gmail()
        {
            ViewBag.Message = "認証してください";

            return View();
        }
    }
}