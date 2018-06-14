using MicBeach.Util.Net;
using MicBeach.Util.Serialize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWeb.Controllers.Base;
using MicBeach.Util.Extension;
using System.Configuration;

namespace TestWeb.Controllers
{
    /// <summary>
    /// 上传
    /// </summary>
    public class UploadController : WebBaseController
    {
        #region 客户端

        [HttpPost]
        public ActionResult Upload(string fileKey, string newFileName = "", string folder = "")
        {
            var file = Request.Files[fileKey];
            string fileName = newFileName.IsNullOrEmpty() ? file.FileName : newFileName;
            var result = HttpUtil.Upload("http://sitesingle.8t6x.com/Upload/UploadFile", new Dictionary<string, byte[]>()
            {
                { fileName,file.InputStream.ToBytes()}
            }, new Dictionary<string, string>()
            {
                { "Folder",folder}
            });
            return Json(result);
        }

        public ActionResult UploadJobFile()
        {
            return Upload("job_file");
        }

        #endregion

        #region 服务端

        [HttpPost]
        public ActionResult UploadFile(UploadOption option)
        {
            UploadResult result = null;
            try
            {
                var files = Request.Files;
                if (files == null || files.Count <= 0)
                {
                    result = new UploadResult()
                    {
                        ErrorMsg = "没有上传任何文件",
                        Success = false
                    };
                }
                else
                {
                    List<UploadFileInfo> fileInfoList = new List<UploadFileInfo>();
                    foreach (string fileKey in files)
                    {
                        var file = files[fileKey];
                        var fileSaveInfo = Save(file, option);
                        fileInfoList.Add(fileSaveInfo);
                    }
                    result = new UploadResult()
                    {
                        ErrorMsg = "上传成功",
                        Success = true,
                        FileInfoList = fileInfoList
                    };
                }
            }
            catch (Exception ex)
            {
                result = new UploadResult()
                {
                    ErrorMsg = ex.Message,
                    Success = false
                };
            }
            return Content(BinarySerialize.SerializeToString<UploadResult>(result));
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="file">文件对象</param>
        /// <param name="option">上传选项</param>
        /// <returns></returns>
        UploadFileInfo Save(HttpPostedFileBase file, UploadOption option)
        {
            #region 参数判断

            if (file == null || file.ContentLength <= 0)
            {
                return null;
            }

            #endregion

            #region 文件名

            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            if (option?.RenameFile ?? false)
            {
                fileName = Guid.NewGuid().ToString().Replace("-", "");
            }

            #endregion

            #region 文件后缀

            string suffix = Path.GetExtension(file.FileName);
            if (option != null && !option.Suffix.IsNullOrEmpty())
            {
                suffix = option.Suffix;
            }

            #endregion

            fileName = string.Format("{0}.{1}", fileName, suffix);

            #region 保存路径

            string uploadRootDirectory = ConfigurationManager.AppSettings["UploadRoot"];//上传根目录
            string fileDirectory = option?.Folder;
            string fullDirectory = Server.MapPath(Path.Combine(uploadRootDirectory, fileDirectory));
            if (!Directory.Exists(fullDirectory))
            {
                Directory.CreateDirectory(fullDirectory);
            }
            string relativePath = Path.Combine(uploadRootDirectory, fileDirectory, fileName);
            string fullSavePath = Server.MapPath(relativePath);

            #endregion

            System.IO.File.WriteAllBytes(fullSavePath, file.InputStream.ToBytes());
            return new UploadFileInfo()
            {
                FileName = fileName,
                Suffix = suffix,
                FullPath = fullSavePath,
                RelativePath = relativePath,
                OriginalFileName = file.FileName
            };
        }

        public ActionResult SearchFiles(FileFilter filter)
        {
            string uploadRoot = ConfigurationManager.AppSettings["UploadRoot"];
            List<string> searchFolders = new List<string>();
            if (!filter.Paths.IsNullOrEmpty())
            {
                searchFolders.AddRange(filter.Paths.Select(c => Path.Combine(uploadRoot, c)));
            }
            else
            {
                searchFolders.Add(uploadRoot);
            }
            List<string> fileNames = new List<string>();
            foreach (string path in searchFolders)
            {
                string serverPath = Server.MapPath(path);

            }
            //string localPath = Server.MapPath(path);
            //string[] suffixArray = suffixFilter.LSplit(",");
            //string[] files = Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
            //    .Where(x => suffixArray.Contains(Path.GetExtension(x).ToLower()))
            //    .Select(x => path + x.Substring(localPath.Length).Replace("\\", "/")).ToArray();
            return Content("");
        }

        /// <summary>
        /// 搜索目录下的文件
        /// </summary>
        /// <param name="fileFilter">搜索条件</param>
        /// <param name="folder">搜索文件夹</param>
        /// <returns></returns>
        List<string> SearchFolderFiles(FileFilter fileFilter, string folder)
        {
            return null;
        }

        #endregion
    }
}