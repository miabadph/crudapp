using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUDApp.Db;
using CRUDApp.Models;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CRUDApp.Controllers
{
    [Route("api/Contacts")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactsContext _context;

        public ContactController(ContactsContext context)
        {
            _context = context;
        }

        [HttpGet("getallcontacts")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
        {
            try
            {
                return await _context.Contacts.ToListAsync();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("getcontact/{id:int}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);

                if (contact == null)
                {
                    return NotFound();
                }

                return contact;
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("updatecontact/{id}")]
        public async Task<IActionResult> UpdateContact(int id, Contact contact)
        {
            try
            {
                _context.Entry(contact).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }

            return NoContent();
        }

        [HttpPost("addcontact")]
        public async Task<ActionResult<Contact>> AddContact(Contact contact)
        {
            try
            {
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetContact", new { id = contact.ContactId }, contact);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteContact/{id}")]
        public async Task<ActionResult<Contact>> DeleteContact(int id)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    return NotFound();
                }

                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();

                return contact;
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
    }
}
