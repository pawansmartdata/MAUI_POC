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
        Task<List<ItemResponseDto>> GetAllItemsAsync();
        Task<ItemResponseDto> GetItemByIdAsync(int id);
        Task<bool> AddItemAsync(ItemDto item);
        Task<bool> DeleteItemAsync(int id);
        Task<bool> UpdateItemAsync(int id, ItemDto item);
    }
}
