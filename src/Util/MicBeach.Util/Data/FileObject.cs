using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Data
{
    /// <summary>
    /// File Object
    /// </summary>
    public struct FileObject
    {
        #region fields

        private string _filePath;//file path
        private string _fileName;//file name
        private string _fileNameWithOutExtension;//file name without extension
        private string _extension;//file extension
        private bool _isImage;//is image
        private static string[] _imageFileExtensions = new string[] { "jpg", "jpeg", "png", "gif", "bmp", "tif", "tiff", "ico" };

        #endregion

        #region constructor

        /// <summary>
        /// instance a FileObject object
        /// </summary>
        /// <param name="filePath">filePath</param>
        public FileObject(string filePath)
        {
            _filePath = filePath;
            _fileName = Path.GetFileName(filePath);
            _fileNameWithOutExtension = Path.GetFileNameWithoutExtension(filePath);
            _extension = Path.GetExtension(_filePath);
            _isImage = IsImageFile(_extension);
        }

        #endregion

        #region Propertys

        /// <summary>
        /// FilePath
        /// </summary>
        public string FilePath
        {
            get
            {
                return _filePath;
            }
        }

        /// <summary>
        /// FileName
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        /// <summary>
        /// FileNameWithOutExtension
        /// </summary>
        public string FileNameWithOutExtension
        {
            get
            {
                return _fileNameWithOutExtension;
            }
        }

        /// <summary>
        /// Extension
        /// </summary>
        public string Extension
        {
            get
            {
                return _extension;
            }
        }

        /// <summary>
        /// IsImage
        /// </summary>
        public bool IsImage
        {
            get
            {
                return _isImage;
            }
        }

        #endregion

        #region static methods

        /// <summary>
        /// verify whether is a image type file
        /// </summary>
        /// <param name="extension">file extension</param>
        /// <returns>is a image type file</returns>
        private static bool IsImageFile(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }
            return _imageFileExtensions.Contains(extension);
        }

        #endregion
    }
}
