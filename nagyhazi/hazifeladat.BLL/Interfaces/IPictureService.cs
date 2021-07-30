using hazifeladat.BLL.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.BLL.Interfaces
{
    public interface IPictureService
    {
        Task<Picture> GetPictureAsync(int pictureId);
        Task<IEnumerable<Picture>> GetPicturesAsync();
        Task<Picture> InsertPictureAsync(string description, IFormFile pic);
        Task UpdatePictureAsync(int pictureId, string description, IFormFile pic);
        Task DeletePictureAsync(int pictureId);
    }
}
