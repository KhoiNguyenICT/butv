
using BUTV.Core.Domain.Media;
using BUTV.Services;
using BUTV.Services.Seo;
using BUTV.Data;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

using System.IO;
using System.Threading;

namespace BUTV.Services.Media
{
    public class PictureService : IPictureService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IHostingEnvironment _env;
        private readonly IRepository<Picture> _pictureRepository;
        public PictureService(IRepository<Picture> pictureRepository,
             IHostingEnvironment env,
             IHttpContextAccessor httpContext)
        {
            _env = env;
            _httpContext = httpContext;
            this._pictureRepository = pictureRepository;
        }
        public Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _pictureRepository.Get(pictureId);
        }
        public virtual string GetPictureUrl(Picture picture,
          int targetSize = 0,
          bool showDefaultPicture = true,
          PictureType defaultPictureType = PictureType.Entity)
        {
            string url = string.Empty;
            byte[] pictureBinary = null;
            if (picture != null)
                pictureBinary = LoadPictureBinary(picture);
            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                {
                    //url = GetDefaultPictureUrl(targetSize, defaultPictureType, storeLocation);
                }
                return url;
            }

            if (picture.IsNew)
            {
                DeletePictureThumbs(picture);

                //we do not validate picture binary here to ensure that no exception ("Parameter is not valid") will be thrown
                picture = UpdatePicture(picture.Id,
                    pictureBinary,
                    picture.MimeType,
                    picture.SeoFilename,
                    picture.AltAttribute,
                    picture.TitleAttribute,
                    false,
                    false);
            }

            var seoFileName = picture.SeoFilename; // = GetPictureSeName(picture.SeoFilename); //just for sure

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;
            if (targetSize == 0)
            {
                thumbFileName = !String.IsNullOrEmpty(seoFileName)
                    ? string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), seoFileName, lastPart)
                    : string.Format("{0}.{1}", picture.Id.ToString("0000000"), lastPart);
            }
            else
            {
                thumbFileName = !String.IsNullOrEmpty(seoFileName)
                    ? string.Format("{0}_{1}_{2}.{3}", picture.Id.ToString("0000000"), seoFileName, targetSize, lastPart)
                    : string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), targetSize, lastPart);
            }
            string thumbFilePath = GetThumbLocalPath(thumbFileName);

            //the named mutex helps to avoid creating the same files in different threads,
            //and does not decrease performance significantly, because the code is blocked only for the specific file.
            using (var mutex = new Mutex(false, thumbFileName))
            {
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    mutex.WaitOne();

                    //check, if the file was created, while we were waiting for the release of the mutex.
                    if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                    {
                        byte[] pictureBinaryResized;

                        //resizing required
                        if (targetSize != 0)
                        {
                            using (var stream = new MemoryStream(pictureBinary))
                            {
                                using (MagickImage image = new MagickImage(stream))
                                {
                                    var size = new Size(image.Width, image.Height);

                                    var newSize = CalculateDimensions(size, targetSize);
                                    image.Resize(newSize.Width, newSize.Height);
                                    image.Strip();
                                    image.Quality = 100;
                                    pictureBinaryResized = image.ToByteArray();
                                }

                            }
                        }
                        else
                        {
                            //create a copy of pictureBinary
                            pictureBinaryResized = pictureBinary;
                        }

                        SaveThumb(thumbFilePath, thumbFileName, picture.MimeType, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }

            }
            url = GetThumbUrl(thumbFileName);
            return url;
        }
        public virtual string GetPictureSeName(string name)
        {
            return SeoExtensions.GetSeName(name, true, false);
        }
        protected virtual Size CalculateDimensions(Size originalSize, int targetSize,
       ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true)
        {
            float width, height;

            switch (resizeType)
            {
                case ResizeType.LongestSide:
                    if (originalSize.Height > originalSize.Width)
                    {
                        // portrait
                        width = originalSize.Width * (targetSize / (float)originalSize.Height);
                        height = targetSize;
                    }
                    else
                    {
                        // landscape or square
                        width = targetSize;
                        height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    }
                    break;
                case ResizeType.Width:
                    width = targetSize;
                    height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    break;
                case ResizeType.Height:
                    width = originalSize.Width * (targetSize / (float)originalSize.Height);
                    height = targetSize;
                    break;
                default:
                    throw new Exception("Not supported ResizeType");
            }

            if (ensureSizePositive)
            {
                if (width < 1)
                    width = 1;
                if (height < 1)
                    height = 1;
            }

            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        public virtual Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType,
        string seoFilename, string altAttribute = null, string titleAttribute = null,
        bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = GetPictureById(pictureId);
            if (picture == null)
                return null;

            //delete old thumbs if a picture has been changed
            if (seoFilename != picture.SeoFilename)
                DeletePictureThumbs(picture);

            picture.PictureBinary = pictureBinary;
            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;
            picture.IsNew = isNew;

            _pictureRepository.Update(picture);

            //if (!this.StoreInDb)
            //    SavePictureInFile(picture.Id, pictureBinary, mimeType);

            //event notification
            //_eventPublisher.EntityUpdated(picture);

            return picture;
        }

        public virtual byte[] ValidatePicture(byte[] pictureBinary, string mimeType)
        {
            //using (var destStream = new MemoryStream())
            //{
            //    ImageBuilder.Current.Build(pictureBinary, destStream, new ResizeSettings
            //    {
            //        MaxWidth = _mediaSettings.MaximumImageSize,
            //        MaxHeight = _mediaSettings.MaximumImageSize,
            //        Quality = _mediaSettings.DefaultImageQuality
            //    });
            //    return destStream.ToArray();
            //}
            return pictureBinary;
        }
        protected virtual bool GeneratedThumbExists(string thumbFilePath, string thumbFileName)
        {

            return File.Exists(thumbFilePath);
        }
        protected virtual string GetThumbUrl(string thumbFileName)
        {
            var url = "http://" + _httpContext.HttpContext.Request.Host.Value + "/images/thumbs/";
            url = url + thumbFileName;
            return url;
        }
        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, string mimeType, byte[] binary)
        {
            File.WriteAllBytes(thumbFilePath, binary);
        }
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            string filter = string.Format("{0}*.*", picture.Id.ToString("0000000"));
            var thumbDirectoryPath = CommonHelper.MapPath("/images/thumbs", _env);
            string[] currentFiles = System.IO.Directory.GetFiles(thumbDirectoryPath, filter, SearchOption.AllDirectories);
            foreach (string currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                File.Delete(thumbFilePath);
            }
        }
        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = CommonHelper.MapPath("~/images/thumbs", _env);
            //if (_mediaSettings.MultipleThumbDirectories)
            //{
            //    //get the first two letters of the file name
            //    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(thumbFileName);
            //    if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > MULTIPLE_THUMB_DIRECTORIES_LENGTH)
            //    {
            //        var subDirectoryName = fileNameWithoutExtension.Substring(0, MULTIPLE_THUMB_DIRECTORIES_LENGTH);
            //        thumbsDirectoryPath = Path.Combine(thumbsDirectoryPath, subDirectoryName);
            //        if (!System.IO.Directory.Exists(thumbsDirectoryPath))
            //        {
            //            System.IO.Directory.CreateDirectory(thumbsDirectoryPath);
            //        }
            //    }
            //}
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }
        protected virtual byte[] LoadPictureBinary(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            var result = picture.PictureBinary;
            return result;
        }
        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            var filePath = GetPictureLocalPath(fileName);
            if (!File.Exists(filePath))
                return new byte[0];
            return File.ReadAllBytes(filePath);
        }
        //public virtual string GetDefaultPictureUrl(int targetSize = 0,
        //   PictureType defaultPictureType = PictureType.Entity,
        //   string storeLocation = null)
        //{
        //    string defaultImageFileName;
        //    switch (defaultPictureType)
        //    {
        //        //case PictureType.Avatar:
        //        //    defaultImageFileName = _settingService.GetSettingByKey("Media.Customer.DefaultAvatarImageName", "default-avatar.jpg");
        //        //    break;
        //        case PictureType.Entity:
        //        default:
        //            defaultImageFileName = "";// _settingService.GetSettingByKey("Media.DefaultImageName", "default-image.png");
        //            break;
        //    }
        //    string filePath = GetPictureLocalPath(defaultImageFileName);
        //    if (!File.Exists(filePath))
        //    {
        //        return "";
        //    }


        //    if (targetSize == 0)
        //    {
        //        string url = (!String.IsNullOrEmpty(storeLocation)
        //                         ? storeLocation
        //                         : _webHelper.GetStoreLocation())
        //                         + "content/images/" + defaultImageFileName;
        //        return url;
        //    }
        //    else
        //    {
        //        string fileExtension = Path.GetExtension(filePath);
        //        string thumbFileName = string.Format("{0}_{1}{2}",
        //            Path.GetFileNameWithoutExtension(filePath),
        //            targetSize,
        //            fileExtension);
        //        var thumbFilePath = GetThumbLocalPath(thumbFileName);
        //        if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
        //        {
        //            using (var b = new Bitmap(filePath))
        //            {
        //                using (var destStream = new MemoryStream())
        //                {
        //                    var newSize = CalculateDimensions(b.Size, targetSize);
        //                    ImageBuilder.Current.Build(b, destStream, new ResizeSettings
        //                    {
        //                        Width = newSize.Width,
        //                        Height = newSize.Height,
        //                        Scale = ScaleMode.Both,
        //                        Quality = _mediaSettings.DefaultImageQuality
        //                    });
        //                    var destBinary = destStream.ToArray();
        //                    SaveThumb(thumbFilePath, thumbFileName, "", destBinary);
        //                }
        //            }
        //        }
        //        var url = GetThumbUrl(thumbFileName, storeLocation);
        //        return url;
        //    }
        //}
        protected virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            //also see System.Web.MimeMapping for more mime types

            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return Path.Combine(CommonHelper.MapPath("/images/", _env), fileName);
        }
    }
    public enum ResizeType
    {
        LongestSide,
        Width,
        Height
    }
    public class Size
    {
        public Size(int w, int h)
        {
            Width = w;
            Height = h;
        }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
