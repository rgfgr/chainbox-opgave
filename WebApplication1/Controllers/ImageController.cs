using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly static string[] extentions = { ".png", ".jpg", ".jpeg" };
        private static Random rand = new();
        private readonly CykleContext _context;
        private readonly IWebHostEnvironment _environment;

        public ImageController(CykleContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Image/5/Images
        [HttpGet("{memberId}/Images")]
        public async Task<ActionResult<IEnumerable<Image>>> GetMemberImages(string memberId)
        {
            return await _context.Images.Where(x => x.MemberName == memberId).ToListAsync();
        }

        // GET: api/Image/5/Random
        [HttpGet("{memberId}/Random")]
        public async Task<ActionResult<Image>> GetRandomImage(string memberId)
        {
            List<Image> images = await _context.Images.Where(x => x.MemberName == memberId).ToListAsync();
            return images[rand.Next(images.Count)];
        }

        [HttpPost("{memberId}&{name}")]
        public async Task<ActionResult> UploadImage(string memberId, string name, IFormFile formFile)
        {
            if (!MemberExists(memberId))
            {
                return BadRequest("Member does not exist!");
            }

            bool results = false;
            string Filename = "", Filepath = "", imagePath = "";

            try
            {
                string tempFileName = Path.GetRandomFileName();
                string tempName = Path.GetFileNameWithoutExtension(tempFileName);
                string tempExt = Path.GetExtension(formFile.FileName);
                if (!extentions.Contains(tempExt))
                {
                    return BadRequest("Invalid file type");
                }
                Filename = $"{tempName}{tempExt}";
                Filepath = GetFilePath(memberId);

                if (!Directory.Exists(Filepath))
                {
                    Directory.CreateDirectory(Filepath);
                }

                imagePath = Filepath + "\\" + Filename;
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await formFile.CopyToAsync(stream);
                    results = true;
                }
                _ = _context.Images.Add(new()
                {
                    Filepath = $"{memberId}/{Filename}",
                    MemberName = memberId,
                    Name = name
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex + $"\nFilename: {Filename}\nFilepath: {Filepath}\nimagePath {imagePath}");
            }
            return Ok(results);
        }

        [NonAction]
        private string GetFilePath(string filePath)
        {
            return _environment.WebRootPath + "\\Imgs\\" + filePath;
        }

        private bool MemberExists(string id)
        {
            return (_context.Members?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
