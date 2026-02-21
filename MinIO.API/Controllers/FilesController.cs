using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinIO.API.Services;

namespace MinIO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileStorageService _storage;

        public FilesController(FileStorageService storage)
        {
            _storage = storage;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            await _storage.UploadAsync(file);

            return Ok("File uploaded successfully");
        }

        [HttpGet("upload-url")]
        public async Task<IActionResult> GetUploadUrl(string fileName)
        {
            var url = await _storage.GenerateUploadUrl(fileName);
            return Ok(new { uploadUrl = url });
        }
    }
}
