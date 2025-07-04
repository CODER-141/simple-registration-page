using complaint.api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace complaint.api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsermasterController : ControllerBase {

        private readonly ApplicationDbContext _context;

        public UsermasterController(ApplicationDbContext context) {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers() {
            try {
                return await _context.Users.ToListAsync();
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id) {
            try {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                return user;
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser(UserModel user) {
            try {
                // Check if mobile already exists
                if (await _context.Users.AnyAsync(u => u.mobile == user.mobile)) {
                    return BadRequest("Mobile number already exists.");
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // return 201 Created with location header
                return CreatedAtAction(nameof(GetUser), new { id = user.userId }, user);
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserModel user) {
            if (id != user.userId)
                return BadRequest("User ID mismatch.");

            try {
                // Check if mobile number is changed and conflicts with existing
                var existingUserWithMobile = await _context.Users
                    .FirstOrDefaultAsync(u => u.mobile == user.mobile && u.userId != id);

                if (existingUserWithMobile != null) {
                    return BadRequest("Mobile number already exists.");
                }

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent(); // 204
            } catch (DbUpdateConcurrencyException) {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {
            try {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool UserExists(int id) {
            return _context.Users.Any(e => e.userId == id);
        }
    }
}

