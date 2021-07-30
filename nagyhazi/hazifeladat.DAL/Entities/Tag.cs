using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.DAL.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Article> Articles { get; } = new List<Article>();
        public ICollection<ArticleTag> TaggedArticles { get; } = new List<ArticleTag>();
    }
}
