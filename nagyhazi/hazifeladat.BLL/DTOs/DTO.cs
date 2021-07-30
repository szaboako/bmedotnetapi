using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.BLL.DTOs
{
    public record Tag
    {
        public int Id { get; init; }
        public string Name { get; set; }
        //public List<Article> Articles { get; set; }
    }

    /*public record ArticleTag
    {
        public int Id { get; init; }
        public int TagId { get; init; }
        public Tag Tag { get; init; }
        public int ArticleId { get; init; }
        public Article Article { get; init; }
    }*/

    public record Picture
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Path { get; init; }
    }

    public record Article
    {
        public int Id { get; init; }
        [Required(ErrorMessage = "Article label is required.", AllowEmptyStrings = false)]
        public string Label { get; init; }
        public string Description { get; init; }
        public string Content { get; init; }
        public int PictureId { get; init; }
        public Picture Picture { get; init; }
        public List<Tag> Tags { get; init; }
    }
}
