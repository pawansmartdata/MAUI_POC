using App.Core.Dtos;
using App.Core.Interfaces.IRepository;
using App.Core.Interfaces.IServices;
using Domain.Entities;
using Domain.Utility;
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
        private readonly IImageService _imageService;
    
        public ItemService(IItemRepository repository, IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
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

        public async Task<ItemDto> CreateItemAsync(ItemDto dto, string webRootPath)
        {
            string? imageUrl = null;

            // Handle image upload if provided
            if (dto.ItemImage != null)
            {
                var result = await _imageService.Upload(dto.ItemImage, webRootPath);
                if (result is ResponseDto { Status: 200, Data: not null } uploadSuccess)
                {
                    imageUrl = uploadSuccess.Data.ToString();
                }
                // Else: silently skip setting imageUrl if upload failed
            }

            var item = FromDto(dto);
            item.ItemImageUrl = imageUrl ?? string.Empty;

            var created = await _repository.AddAsync(item);
            return ToDto(created);
        }

        public async Task<bool> UpdateItemAsync(int id, ItemDto dto, string webRootPath)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            // Handle image upload if a new file is provided
            if (dto.ItemImage != null)
            {
                var result = await _imageService.Upload(dto.ItemImage, webRootPath);
                if (result is ResponseDto uploadResponse && uploadResponse.Status == 200)
                {
                    existing.ItemImageUrl = uploadResponse.Data?.ToString() ?? existing.ItemImageUrl;
                }
                //else
                //{
                //    throw new Exception(uploadResponse?.Message ?? "Image upload failed.");
                //}
            }

            // Conditionally update fields if provided
            
            
               existing.Name = dto.Name??existing.Name;        
               existing.Description = dto.Description?? existing.Description;
               existing.Latitude = dto.Latitude?? existing.Latitude;
               existing.Longitude = dto.Longitude?? existing.Longitude; 

            

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
            
            Name = item.Name,
            Description = item.Description,
            ItemImageUrl = item.ItemImageUrl,
            Latitude = item.Latitude,
            Longitude = item.Longitude,
        };

        private static Item FromDto(ItemDto dto) => new Item
        {
            Name = dto.Name,
            Description = dto.Description,
            ItemImageUrl=dto.ItemImageUrl,
            Latitude = dto.Latitude ??00.00,
            Longitude = dto.Longitude ??00.00,
        };
    }

}
