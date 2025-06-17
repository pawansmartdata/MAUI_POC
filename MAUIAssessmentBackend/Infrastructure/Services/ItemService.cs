using App.Core.Dtos;
using App.Core.Interfaces.IRepository;
using App.Core.Interfaces.IServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _repository;

        public ItemService(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ItemDto>> GetAllItemsAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(item => ToDto(item));
        }

        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            return item is null ? null : ToDto(item);
        }

        public async Task<ItemDto> CreateItemAsync(ItemDto dto)
        {
            var item = FromDto(dto);
            var created = await _repository.AddAsync(item);
            return ToDto(created);
        }

        public async Task<bool> UpdateItemAsync(int id, ItemDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Latitude = dto.Latitude;
            existing.Longitude = dto.Longitude;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return false;

            await _repository.DeleteAsync(item);
            return true;
        }

        // --- Mapping helpers ---
        private static ItemDto ToDto(Item item) => new ItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Latitude = item.Latitude,
            Longitude = item.Longitude
        };

        private static Item FromDto(ItemDto dto) => new Item
        {
            Name = dto.Name,
            Description = dto.Description,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude
        };
    }

}
