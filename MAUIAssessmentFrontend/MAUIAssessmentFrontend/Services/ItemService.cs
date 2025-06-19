using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
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
            var response = await _httpClient.GetAsync("api/Item"); // Adjust endpoint if needed
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ItemDto>>();
            }

            return new List<ItemDto>();
        }
    }
}
