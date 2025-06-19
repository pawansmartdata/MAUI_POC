using App.Core.Dtos;
using App.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MAUIAssessmentBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IWebHostEnvironment _env;

        public ItemController(IItemService itemService, IWebHostEnvironment env)
        {
            _itemService = itemService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromForm] ItemDto dto)
        {
            try
            {
                var result = await _itemService.CreateItemAsync(dto, _env.WebRootPath);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromForm] ItemDto dto)
        {
            try
            {
                var success = await _itemService.UpdateItemAsync(id, dto, _env.WebRootPath);
                if (!success) return NotFound(new { error = "Item not found" });
                return Ok(new { message = "Item updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _itemService.DeleteItemAsync(id);
            if (!deleted)
                return NotFound();
            return Ok("Item Deleted SuccessFully");
        }
    }
}
