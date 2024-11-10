using Microsoft.AspNetCore.Mvc;
using youtubeApi.Model;
using System.Linq;

namespace youtubeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppApiController : ControllerBase
    {
        private static List<Items> itemList = new List<Items>();

        [HttpPost]
        public IActionResult PostItems([FromBody] Items item)
        {
            // Check if the item is null before accessing its properties
            if (item == null)
            {
                return BadRequest("Item cannot be null");
            }

            // Normalize the incoming item name by removing spaces
            string normalizedNewName = item.Name.Replace(" ", "").ToLower();

            // Check for existing name in the list by normalizing stored names
            if (itemList.Any(u => u.Name.Replace(" ", "").ToLower() == normalizedNewName))
            {
                return Conflict("A similar name already exists");
            }

            if (item.Id != 0 && itemList.Any(u => u.Id == item.Id))
            {
                return BadRequest("Id already exists");
            }

            // Assign a new Id if none is provided
            if (item.Id == 0)
            {
                item.Id = itemList.Any() ? itemList.Max(i => i.Id) + 1 : 1;
            }

            itemList.Add(item);
            return CreatedAtAction(nameof(PostItems), new { id = item.Id }, item);
        }

        [HttpGet]
        public IActionResult GetItems()
        {
            var result = itemList.Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Age,
                u.Password,
            });

            return Ok(result);
        }
    }
}
