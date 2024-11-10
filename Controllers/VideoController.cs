using Microsoft.AspNetCore.Mvc;
using youtubeApi.Model;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace youtubeApi.Controllers
{
    [Route("video/[controller]")]
    [ApiController]
    public class UploadVideoController : ControllerBase
    {
        private static readonly string UploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        private static readonly List<VideoItem> videoItems = new List<VideoItem>();
        // private static readonly int currentId = 1;


        [HttpPost] 
        public IActionResult PostVideo([FromBody] VideoItem video) {

            if(video == null) {
                return BadRequest("video cannot be null");
            }

            if(video.id != 0 && videoItems.Any(u => u.id == video.id)) {
                return BadRequest("Id already exits");
            }

              if (video.id == 0)
            {
                video.id = videoItems.Count != 0 ? videoItems.Max(i => i.id) + 1 : 1;
            }

            videoItems.Add(video);
            return CreatedAtAction(nameof(PostVideo), new { Id = video.id }, video);


        }

         [HttpGet]
        public IActionResult GetItems()
        {
            var result = videoItems.Select(u => new
            {
                u.id,
                u.username,
                u.video
              
            });

            return Ok(result);
        }

       
        // [HttpGet("{id}")]
        // public IActionResult GetVideoById(int id)
        // {
        //     var video = videoItems.FirstOrDefault(v => v.id == id);

        //     if (video == null)
        //     {
        //         return NotFound("Video not found.");
        //     }

        //     return Ok(video);
        // }
    }
}
