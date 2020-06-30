using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceAccountWebSample.Models
{
    public class GoogleDriveModel
    {
        public string FolderName { get; set; }
        public List<FileData> FileDataList { get; set; }
    }

    public class FileData
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}