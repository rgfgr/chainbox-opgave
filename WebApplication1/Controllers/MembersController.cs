using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly CykleContext _context;
        private readonly IWebHostEnvironment _environment;

        public MembersController(CykleContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        private static MemberDTO MemberToDTO(Member member) => new()
        {
            ID = member.ID,
            Email = member.Email
        };

        [HttpPost("Login/{email}")]
        public async Task<ActionResult<string>> LogIn(string email, [FromBody] string pass)
        {
            var member = await _context.Members.SingleOrDefaultAsync(x => x.Email == email);
            if (member == null)
            {
                return NotFound("Email not found");
            }
            if (member.Pass != pass.ToSHA256String())
            {
                return BadRequest("Password is wrong");
            }
            return Ok(member.ID);
        }

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            return await _context.Members
                .OrderBy(x => x.ID)
                .Select(x => MemberToDTO(x))
                .ToListAsync();
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetMember(string id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return MemberToDTO(member);
        }

        // PUT: api/Members/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{memberId}")]
        public async Task<IActionResult> PutMember(string memberId, Member memberDTO)
        {
            if (memberId != memberDTO.ID)
            {
                return BadRequest();
            }

            Member? member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                return NotFound();
            }

            member.ID = memberDTO.ID;
            member.Pass = memberDTO.Pass.ToSHA256String();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MemberExists(memberId))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostMember(Member member)
        {
            if (_context.Members.Any(x => x.Email == member.Email)) return BadRequest("Email already in use");
            if (_context.Members.Any(x => x.ID == member.ID)) return BadRequest("Username already in use");

            member.Pass = member.Pass.ToSHA256String();

            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            return Ok(_context.Members.Single(x => x.Email == member.Email).ID);
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(string id, [FromBody] string pass)
        {
            var member = _context.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }
            if (member.Pass != pass.ToSHA256String())
            {
                return BadRequest("Password is wrong");
            }

            var images = await _context.Images.Where(x => x.MemberName == id).ToListAsync();

            _context.Members.Remove(member);
            _context.Images.RemoveRange(images);
            await _context.SaveChangesAsync();

            Directory.Delete(_environment.WebRootPath + "\\Imgs\\" + id, true);

            return NoContent();
        }

        private bool MemberExists(string id)
        {
            return (_context.Members?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
