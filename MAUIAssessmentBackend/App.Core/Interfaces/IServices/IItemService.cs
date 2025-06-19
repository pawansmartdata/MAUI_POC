using App.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces.IServices
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllItemsAsync();
        Task<ItemDto?> GetItemByIdAsync(int id);
        Task<ItemDto> CreateItemAsync(ItemDto dto, string webRootPath);
        Task<bool> UpdateItemAsync(int id, ItemDto dto, string webRootPath);

        Task<bool> DeleteItemAsync(int id);
    }
}
