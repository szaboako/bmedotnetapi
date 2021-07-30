using AutoMapper;
using hazifeladat.BLL.DTOs;
using hazifeladat.BLL.Exceptions;
using hazifeladat.BLL.Interfaces;
using hazifeladat.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.BLL.Services
{
    public class PictureService : IPictureService
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;
        public PictureService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task DeletePictureAsync(int pictureId)
        {
            File.Delete(_context.Pictures.AsNoTracking().SingleOrDefault(p => p.Id == pictureId).Path);
            _context.Pictures.Remove(new DAL.Entities.Picture { Id = pictureId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Pictures.SingleOrDefault(p => p.Id == pictureId) == null)
                    throw new EntityNotFoundException("Nem található a kép");
                else throw;
            }
        }

        public async Task<Picture> GetPictureAsync(int pictureId)
        {
            return await _mapper.ProjectTo<Picture>(
                        _context.Pictures.Where(p => p.Id == pictureId)
            ).SingleOrDefaultAsync()
            ?? throw new EntityNotFoundException("Nem található a kép");
        }

        public async Task<IEnumerable<Picture>> GetPicturesAsync()
        {
            return await
               _mapper.ProjectTo<Picture>(_context.Pictures)
               .ToListAsync();
        }

        public async Task<Picture> InsertPictureAsync(string description, IFormFile pic)
        {

            var filepath = Path.Combine(Environment.CurrentDirectory, @"Pictures\", pic.FileName);
            using (Stream filestream = new FileStream(filepath, FileMode.Create))
            {
                await pic.CopyToAsync(filestream);
            }
            var newPicture = new Picture() { Name = pic.FileName, Description = description, Path = filepath };
            var efPicture = _mapper.Map<DAL.Entities.Picture>(newPicture);
            _context.Pictures.Add(efPicture);
            await _context.SaveChangesAsync();
            return await GetPictureAsync(efPicture.Id);
        }

        public async Task UpdatePictureAsync(int pictureId, string description, IFormFile pic)
        {
            try
            {
                var oldPicture = await _context.Pictures.AsNoTracking().SingleOrDefaultAsync(p => p.Id == pictureId);

                if (pic != null)
                {
                    var filepath = Path.Combine(Environment.CurrentDirectory, @"Pictures\", pic.FileName);
                    using (Stream filestream = new FileStream(filepath, FileMode.Create))
                    {
                        await pic.CopyToAsync(filestream);
                    }
                    var pictureDescription = "";
                    if (description == null)
                        pictureDescription = oldPicture.Description;
                    else
                        pictureDescription = description;

                    var updatedPicture = new Picture() { Name = pic.FileName, Description = pictureDescription, Path = filepath };
                    var efPicture = _mapper.Map<DAL.Entities.Picture>(updatedPicture);
                    File.Delete(GetPictureAsync(pictureId).Result.Path);
                    efPicture.Id = pictureId;
                    var entry = _context.Attach(efPicture);
                    entry.State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    var updatedPicture = new Picture() { Name = oldPicture.Name, Description = description, Path = oldPicture.Path };
                    var efPicture = _mapper.Map<DAL.Entities.Picture>(updatedPicture);
                    efPicture.Id = pictureId;
                    var entry = _context.Attach(efPicture);
                    entry.State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Pictures
                .SingleOrDefaultAsync(p => p.Id == pictureId) == null)
                    throw new EntityNotFoundException("Nem található a kép");
                else throw;
            }
        }
    }
}
