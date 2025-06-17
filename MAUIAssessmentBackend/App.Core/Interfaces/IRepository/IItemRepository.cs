using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces.IRepository
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task<Item> AddAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(Item item);
    }
}
