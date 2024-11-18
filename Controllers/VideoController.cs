using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Route("api/videos")]
[ApiController]
public class VideosController : ControllerBase
{
    private readonly string _videoUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedVideos");

    public VideosController()
    {
        // Ensure the upload directory exists
        if (!Directory.Exists(_videoUploadPath))
        {
            Directory.CreateDirectory(_videoUploadPath);
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadVideo(IFormFile videoFile)
    {
        if (videoFile == null || videoFile.Length == 0)
        {
            return BadRequest(new { message = "No video file uploaded." });
        }

        // Validate file extension (you can add more formats if needed)
        var allowedExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv" };
        var extension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { message = "Invalid file type. Only video files are allowed." });
        }

        // Sanitize the filename
        var sanitizedFileName = Path.GetFileNameWithoutExtension(videoFile.FileName) + extension;
        var filePath = Path.Combine(_videoUploadPath, sanitizedFileName);

        // Ensure unique file name if the file already exists
        int fileCount = 1;
        while (System.IO.File.Exists(filePath))
        {
            sanitizedFileName = $"{Path.GetFileNameWithoutExtension(videoFile.FileName)}_{fileCount++}{extension}";
            filePath = Path.Combine(_videoUploadPath, sanitizedFileName);
        }

        Console.WriteLine($"Uploading video to: {filePath}");

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error uploading file." });
        }

        return Ok(new { message = "Video uploaded successfully.", fileName = sanitizedFileName });
    }

    [HttpGet]
    public IActionResult GetVideos()
    {
        var files = Directory.GetFiles(_videoUploadPath)
                             .Select(file => new
                             {
                                 FileName = Path.GetFileName(file),
                                 DownloadUrl = Url.Action("GetVideo", new { fileName = Path.GetFileName(file) })
                             })
                             .ToList();

        return Ok(files);
    }

    [HttpGet("download/{fileName}")]
    public IActionResult GetVideo(string fileName)
    {
        var filePath = Path.Combine(_videoUploadPath, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(new { message = "Video not found." });
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "video/mp4", fileName);
    }
}