using MAUIAssessmentFrontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemDto>> GetAllItemsAsync();
    }
}
