using MAUIAssessnentFrontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAssessnentFrontend.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://your-api-url/api/item"; // Replace with your actual API

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ItemDto>> GetItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ItemDto>>(BaseUrl) ?? new();
        }

        public async Task<ItemDto?> GetItemAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ItemDto>($"{BaseUrl}/{id}");
        }

        public async Task<ItemDto?> CreateItemAsync(ItemDto item)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, item);
            return await response.Content.ReadFromJsonAsync<ItemDto>();
        }

        public async Task<bool> UpdateItemAsync(int id, ItemDto item)
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", item);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
