using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudSystem.API.Data;
using CrudSystem.API.Model;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;
using System.Threading.Channels;

namespace CrudSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsWithEntityActionController : ControllerBase
    {
        private readonly StorDbContext _context;

        public ProductsWithEntityActionController(StorDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductsWithEntityAction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> Getproducts()
        {
          if (_context.products == null)
          {
              return NotFound();
          }
            return await _context.products.ToListAsync();
        }

        // GET: api/ProductsWithEntityAction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProducts(int id)
        {
          if (_context.products == null)
          {
              return NotFound();
          }
            var products = await _context.products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            return products;
        }

        // PUT: api/ProductsWithEntityAction/5
       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, Products products , string UserName , string UserEmail)
        {
            if (id != products.Id)
            {
                return BadRequest();
            }
            var oldProduct = await _context.products.FindAsync(id);
            if (oldProduct is null)
            {
                return NotFound();
            }
            _context.Entry(oldProduct).CurrentValues.SetValues(products);

            var auditLog = new AuditTrail()
            {
                UserEmail = UserEmail,
                UserName = UserName,
                Action = "Product Update",
                Timestamp = DateTime.UtcNow,
                EntityName = typeof(Products).Name,
                Changes = JsonSerializer.Serialize(products)
            };
            _context.auditTrails.Add(auditLog);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductsWithEntityAction
       
        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products , string UserEmail , string userName)
        {
          if (_context.products == null)
          {
              return Problem("Entity set 'StorDbContext.products'  is null.");
          }
           
            _context.products.Add(products);

            var auditLog = new AuditTrail()
            {
                UserEmail = UserEmail,
                UserName = userName,
                Action = "Product Created",
                Timestamp = DateTime.UtcNow,
                EntityName = typeof(Products).Name,
                Changes = JsonSerializer.Serialize(products)
            };
            _context.auditTrails.Add(auditLog);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducts", new { id = products.Id }, products);
        }



        // DELETE: api/ProductsWithEntityAction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id , string UserName , string UserEmail)
        {

            if (_context.products == null)
            {
                return NotFound();
            }
        
            var products = await _context.products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }

            _context.products.Remove(products);
            await _context.SaveChangesAsync();
      
          return CreatedAtAction("DeleteProducts", new { id = products.Id }, products);

        }

        private bool ProductsExists(int id)
        {
            return (_context.products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
