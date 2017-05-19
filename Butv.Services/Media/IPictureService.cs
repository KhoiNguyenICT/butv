using BUTV.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BUTV.Services.Media
{
    public interface IPictureService
    {
        Picture GetPictureById(int pictureId);
        string GetPictureUrl(Picture picture,
          int targetSize = 0,
          bool showDefaultPicture = true,
          PictureType defaultPictureType = PictureType.Entity);
        Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType,
         string seoFilename, string altAttribute = null, string titleAttribute = null,
         bool isNew = true, bool validateBinary = true);
       
    }
    public enum PictureType
    {
        /// <summary>
        /// Entities (products, categories, manufacturers)
        /// </summary>
        Entity = 1,
        /// <summary>
        /// Avatar
        /// </summary>
        Avatar = 10,
    }
}
