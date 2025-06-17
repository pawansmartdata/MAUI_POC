using MAUIAssessnentFrontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessnentFrontend.Services
{
    public interface IApiService
    {
        Task<List<ItemDto>> GetItemsAsync();
        Task<ItemDto?> GetItemAsync(int id);
        Task<ItemDto?> CreateItemAsync(ItemDto item);
        Task<bool> UpdateItemAsync(int id, ItemDto item);
        Task<bool> DeleteItemAsync(int id);
    }
}
