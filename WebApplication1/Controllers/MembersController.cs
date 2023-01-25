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

        public MembersController(CykleContext context)
        {
            _context = context;
        }
        private static MemberDTO MemberToDTO(Member member) => new()
        {
            Id = member.Id,
            Name = member.Name,
            Email = member.Email
        };

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            return await _context.Members
                .Select(x => MemberToDTO(x))
                .ToListAsync();
        }

        // GET: api/Members/5/Images
        [HttpGet("{id}/Images")]
        public async Task<ActionResult<IEnumerable<Image>>> GetMemberImages(string id)
        {
            return await _context.Images.Where(x => x.MemberId == id).ToListAsync();
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(string id, Member memberDTO)
        {
            if (id != memberDTO.Id)
            {
                return BadRequest();
            }

            Member? member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            member.Name = memberDTO.Name;
            member.Pass = memberDTO.Pass.ToSHA256String();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!MemberExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // Post: api/Members/5
        [HttpPost("{id}")]
        public async Task<ActionResult<Image>> AddPicture(string id, Image image)
        {
            if (id != image.MemberId)
            {
                return BadRequest();
            }
            if (!MemberExists(id))
            {
                return NotFound();
            }

            _context.Images.Add(image);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMemberImages), new { id }, image);
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MemberDTO>> PostMember(Member member)
        {
            member.Pass = member.Pass.ToSHA256String();

            _context.Members.Add(member);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, MemberToDTO(member));
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(string id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            var images = await _context.Images.Where(x => x.MemberId == id).ToListAsync();

            _context.Members.Remove(member);
            _context.Images.RemoveRange(images);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Members/5/5
        [HttpDelete("{id}/{name}")]
        public async Task<IActionResult> DeleteImage(string id, string name)
        {
            if (!MemberExists(id))
            {
                return NotFound();
            }

            Image? image = _context.Images.FirstOrDefault(x => x.MemberId == id && x.Name == name);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(string id)
        {
            return (_context.Members?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
