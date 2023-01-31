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
        private readonly CykleContext _context;
        private readonly IWebHostEnvironment _environment;

        public ImageController(CykleContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Image/5/Images
        [HttpGet("{id}/Images")]
        public async Task<ActionResult<IEnumerable<Image>>> GetMemberImages(string id)
        {
            return await _context.Images.Where(x => x.MemberId == id).ToListAsync();
        }

        [HttpPost("{memberId}")]
        public async Task<ActionResult> UploadImage(string memberId, IFormFile formFile)
        {
            if (!MemberExists(memberId))
            {
                return BadRequest("Member does not exist!");
            }

            bool results = false;
            string Filename = "", Filepath = "", imagePath = "";

            try
            {
                Filename = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(formFile.FileName)}";
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
                    MemberId = memberId,
                    Name = formFile.Name
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
            return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
