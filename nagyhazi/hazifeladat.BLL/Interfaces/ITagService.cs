using hazifeladat.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hazifeladat.BLL.Interfaces
{
    public interface ITagService
    {
        Task<Tag> GetTagAsync(int tagId);
        Task<IEnumerable<Tag>> GetTagsAsync();
        Task<Tag> InsertTagAsync(Tag newTag);
        Task UpdateTagAsync(int tagId, Tag updatedTag);
        Task DeleteTagAsync(int tagId);
    }
}
