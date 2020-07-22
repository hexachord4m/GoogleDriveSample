using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ServiceAccountWebSample.Models;
using System.Web.Hosting;

namespace ServiceAccountWebSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileWrapper dataFile)
        {
            string credentialFilePath = HostingEnvironment.MapPath("~/Client/credentials.json");

            // 認証ファイル保存
            dataFile.SaveAs(credentialFilePath);

            ViewBag.Message = "登録しました";

            return View("Index");
        }

        public ActionResult GooleDrive(GoogleDriveModel model)
        {
            ICredential credential;
            string credentialFilePath = HostingEnvironment.MapPath("~/Client/credentials.json");

            using (var stream = new FileStream(credentialFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                     .CreateScoped(new[] { DriveService.Scope.DriveReadonly }).UnderlyingCredential;
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Service Account Sample.",
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

            return View(model);
        }

        public ActionResult Gmail()
        {
            ViewBag.Message = "認証してください";

            return View();
        }
    }
}