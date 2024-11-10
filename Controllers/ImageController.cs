using Microsoft.AspNetCore.Mvc;
using youtubeApi.Model;

namespace youtubeApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]

    public class ImageItemController : ControllerBase {
        private static List<ImageItem> ImageItems = new List<ImageItem>();
        
         private static int currentId = 1;

          [HttpPost]
        public IActionResult PostItems([FromBody] ImageItem item) {
           
            if (item == null) {
                return BadRequest("Item cannot be null");
            }

            if (item.Id != 0 && ImageItems.Any(u => u.Id == item.Id)) {
                return BadRequest("Id already exists");
            }

            if (item.Id == 0) {
                item.Id = ImageItems.Any() ? ImageItems.Max(i => i.Id) + 1 : 1;
            }

if (item == null || string.IsNullOrWhiteSpace(item.Image))
            {
                return BadRequest("Invalid image data.");
            }

            item.Id = currentId++;
            ImageItems.Add(item);

            return CreatedAtAction(nameof(PostItems), new { id = item.Id }, item);
        }



        [HttpGet]
        public IActionResult GetItems() {
            var result = ImageItems.Select(u => new {
                u.Id,
                u.Name,
                u.Image
               
        
            });

            return Ok(result);
        }
    }
}