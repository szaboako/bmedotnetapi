using hazifeladat.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.BLL.Interfaces
{
    public interface IArticleService
    {
        Task<Article> GetArticleAsync(int articleId);
        Task<IEnumerable<Article>> GetArticlesAsync();
        Task<IEnumerable<Article>> GetArticlesByTagAsync(string tag);
        Task<Article> InsertArticleAsync(Article newArticle);
        Task UpdateArticleAsync(int articleId, Article updatedArticle);
        Task DeleteArticleAsync(int articleId);
    }
}
