using AutoMapper;
using hazifeladat.BLL.DTOs;
using hazifeladat.BLL.Exceptions;
using hazifeladat.BLL.Interfaces;
using hazifeladat.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.BLL.Services
{
    public class TagService : ITagService
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;
        public TagService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task DeleteTagAsync(int tagId)
        {
            var articleTags =_context.ArticleTags.Where(at => at.TagId == tagId).ToList();
            _context.ArticleTags.RemoveRange(articleTags);
            _context.Tags.Remove(new DAL.Entities.Tag { Id = tagId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Tags.SingleOrDefault(p => p.Id == tagId) == null)
                    throw new EntityNotFoundException("Nem található a tag");
                else throw;
            }
        }

        public async Task<Tag> GetTagAsync(int tagId)
        {
            return await _mapper.ProjectTo<Tag>(
                        _context.Tags.Where(p => p.Id == tagId)
                        ).SingleOrDefaultAsync()
                        ?? throw new EntityNotFoundException("Nem található a tag");
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync()
        {
            return await
                _mapper.ProjectTo<Tag>(_context.Tags)
                .ToListAsync();
        }

        public async Task<Tag> InsertTagAsync(Tag newTag)
        {
            var efTag = _mapper.Map<DAL.Entities.Tag>(newTag);
            _context.Tags.Add(efTag);
            await _context.SaveChangesAsync();
            return await GetTagAsync(efTag.Id);
        }

        public async Task UpdateTagAsync(int tagId, Tag updatedTag)
        {
            var efTag = _mapper.Map<DAL.Entities.Tag>(updatedTag);
            efTag.Id = tagId;
            var entry = _context.Attach(efTag);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Tags
                .SingleOrDefaultAsync(p => p.Id == tagId) == null)
                    throw new EntityNotFoundException("Nem található a tag");
                else throw;
            }
        }
    }
}
