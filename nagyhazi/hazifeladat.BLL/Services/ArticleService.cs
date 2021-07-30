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
    public class ArticleService : IArticleService
    {
        private readonly NorthwindContext _context;
        private readonly IMapper _mapper;
        public ArticleService(NorthwindContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteArticleAsync(int articleId)
        {
            _context.Articles.Remove(new DAL.Entities.Article { Id = articleId });
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Articles.SingleOrDefault(a => a.Id == articleId) == null)
                    throw new EntityNotFoundException("Nem található a cikk");
                else throw;
            }

        }

        public async Task<Article> GetArticleAsync(int articleId)
        {
            return await _mapper.ProjectTo<Article>(
                        _context.Articles.Where(a => a.Id == articleId)
            ).SingleOrDefaultAsync()
            ?? throw new EntityNotFoundException("Nem található a cikk");
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            return await
                _mapper.ProjectTo<Article>(_context.Articles)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesByTagAsync(string tag)
        {
            return await _mapper.ProjectTo<Article>(
                _context.Tags
                .Where(t => t.Name.Contains(tag)).SelectMany(t => t.Articles))
                .ToListAsync();
        }

        public async Task<Article> InsertArticleAsync(Article newArticle)
        {
            var efArticle = _mapper.Map<DAL.Entities.Article>(newArticle);
            _context.Articles.Attach(efArticle);
            await _context.SaveChangesAsync();
            return await GetArticleAsync(efArticle.Id);
        }

        public async Task UpdateArticleAsync(int articleId, Article updatedArticle)
        {
            //removing articletags
            var tagIds = new List<int>();
            foreach(var tag in updatedArticle.Tags)
            {
                tagIds.Add(tag.Id);
            }
            var articleTagsToRemove = _context.ArticleTags.Where(at => !tagIds.Contains(at.TagId) && at.ArticleId == articleId).ToList();
            _context.ArticleTags.RemoveRange(articleTagsToRemove);

            //adding not already existing articletags
            var existingIds = _context.ArticleTags.Where(at => tagIds.Contains(at.TagId) && at.ArticleId == articleId).Select(at => at.TagId).ToList();
            
            var articleTagsToAdd = new List<DAL.Entities.ArticleTag>();
            foreach (var tag in updatedArticle.Tags)
            {
                if(!existingIds.Contains(tag.Id))
                {
                    var tempAT = new DAL.Entities.ArticleTag() { ArticleId = articleId, TagId = tag.Id };
                    articleTagsToAdd.Add(tempAT);
                }  
            }
            _context.ArticleTags.AddRange(articleTagsToAdd);

            var efArticle = _mapper.Map<DAL.Entities.Article>(updatedArticle);
            efArticle.Id = articleId;
            var entry = _context.Attach(efArticle);
            entry.State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Articles
                .SingleOrDefaultAsync(p => p.Id == articleId) == null)
                    throw new EntityNotFoundException("Nem található a cikk");
                else throw;
            }
        }
    }
}
