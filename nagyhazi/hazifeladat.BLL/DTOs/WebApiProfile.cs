using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.BLL.DTOs
{
    public class WebApiProfile : Profile
    {
        public WebApiProfile()
        {
            CreateMap<DAL.Entities.Article, Article>()
                .ForMember(a => a.Tags,
                opt => opt.MapFrom(x => x.ArticleTags.Select(at => at.Tag)))
                .ReverseMap();
            CreateMap<DAL.Entities.Picture, Picture>().ReverseMap();
            CreateMap<DAL.Entities.Tag, Tag>().ReverseMap();
        }
    }
}
