using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUIAssessmentFrontend.Services
{
    public class ItemService : IItemService
    {
        private readonly HttpClient _httpClient;

        public ItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ItemDto>> GetAllItemsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Item");
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response JSON: {json}");

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<ItemDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<ItemDto>();
        }
        public async Task<bool> AddItemAsync(ItemDto item)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Item", item);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddItem error: {ex.Message}");
                return false;
            }
        }
    }
}
