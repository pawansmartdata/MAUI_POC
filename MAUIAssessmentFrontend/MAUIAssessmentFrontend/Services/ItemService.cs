using MAUIAssessmentFrontend.Models;
using MAUIAssessmentFrontend.Services.Interfaces;
using MAUIAssessmentFrontend.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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


        public async Task<List<ItemResponseDto>> GetAllItemsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Item");
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response JSON: {json}");

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<ItemResponseDto>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine($"Request failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return new List<ItemResponseDto>();
        }
        public async Task<bool> AddItemAsync(ItemDto item)

        {
            try
            {
                using var formData = new MultipartFormDataContent();

                //formData.Add(new StringContent(item.Id.ToString()), "Id");
                formData.Add(new StringContent(item.Name ?? ""), "Name");
                //formData.Add(new StringContent(item.ItemImage ?? ""), "ItemImage");
                formData.Add(new StringContent(item.Description ?? ""), "Description");
                formData.Add(new StringContent(item.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Latitude");
                formData.Add(new StringContent(item.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Longitude");

                if (!string.IsNullOrWhiteSpace(item.ItemImage) && File.Exists(item.ItemImage))
                {
                    var stream = File.OpenRead(item.ItemImage);
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // Or detect MIME type

                    formData.Add(fileContent, "ItemImage", Path.GetFileName(item.ItemImage));
                }

                var response = await _httpClient.PostAsync("api/Item", formData);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddItem error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Item/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteItem error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateItemAsync(int id, ItemDto item)
        {
            try
            {
                using var formData = new MultipartFormDataContent();

                formData.Add(new StringContent(item.Name ?? ""), "Name");
                formData.Add(new StringContent(item.Description ?? ""), "Description");
                formData.Add(new StringContent(item.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Latitude");
                formData.Add(new StringContent(item.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)), "Longitude");

                if (!string.IsNullOrWhiteSpace(item.ItemImage) && File.Exists(item.ItemImage))
                {
                    var stream = File.OpenRead(item.ItemImage);
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                    formData.Add(fileContent, "ItemImage", Path.GetFileName(item.ItemImage));
                }

                var response = await _httpClient.PutAsync($"api/Item/{id}", formData);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateItem error: {ex.Message}");
                return false;
            }

        }

        public async Task<ItemResponseDto> GetItemByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Item/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<ItemResponseDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetItemById error: {ex.Message}");
            }

            return null;
        }
    }
}