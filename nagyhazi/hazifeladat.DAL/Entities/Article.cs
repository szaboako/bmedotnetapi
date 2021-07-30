using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.DAL.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int PictureId { get; set; }
        public Picture Picture { get; set; }

        public ICollection<Tag> Tags { get; } = new List<Tag>();
        public ICollection<ArticleTag> ArticleTags { get; } = new List<ArticleTag>();
    }
}
