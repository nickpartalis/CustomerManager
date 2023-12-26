using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerManager.Data;
using CustomerManager.Models;

namespace CustomerManager.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerWithContactNumbersDTO>>> GetCustomers()
        {
            var customers = await _context.Customers
                .Include(c => c.ContactNumbers)
                .ToListAsync();

            var customerDTOs = customers.Select(customer => new CustomerWithContactNumbersDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Email = customer.Email,
                HomeNumber = customer.ContactNumbers?.HomeNumber,
                WorkNumber = customer.ContactNumbers?.WorkNumber,
                MobileNumber = customer.ContactNumbers?.MobileNumber
            }).ToList();

            return customerDTOs;
        }

        // GET: api/v1/customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerWithContactNumbersDTO>> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.ContactNumbers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerDTO = new CustomerWithContactNumbersDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Email = customer.Email,
                HomeNumber = customer.ContactNumbers?.HomeNumber,
                WorkNumber = customer.ContactNumbers?.WorkNumber,
                MobileNumber = customer.ContactNumbers?.MobileNumber
            };

            return customerDTO;
        }

        // POST: api/v1/customers
        [HttpPost]
        public async Task<ActionResult<CustomerWithContactNumbersDTO>> PostCustomer(CustomerWithContactNumbersDTO customerDTO)
        {
            var customer = new Customer
            {
                FirstName = customerDTO.FirstName,
                LastName = customerDTO.LastName,
                Address = customerDTO.Address,
                Email = customerDTO.Email
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var contactNumbers = new ContactNumbers
            {
                HomeNumber = customerDTO.HomeNumber,
                WorkNumber = customerDTO.WorkNumber,
                MobileNumber = customerDTO.MobileNumber,
                CustomerId = customer.Id // Set the correct customer Id
            };

            _context.ContactNumbers.Add(contactNumbers);
            await _context.SaveChangesAsync();

            customerDTO.Id = customer.Id;
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customerDTO);
        }

        // PUT: api/v1/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerWithContactNumbersDTO updatedCustomerDTO)
        {
            if (id != updatedCustomerDTO.Id)
            {
                return BadRequest();
            }

            if (!CustomerExists(id))
            {
                return NotFound();
            }

            var existingCustomer = await _context.Customers
                .Include(c => c.ContactNumbers)
                .FirstOrDefaultAsync(c => c.Id == id);

            _context.Entry(existingCustomer).CurrentValues.SetValues(updatedCustomerDTO);
            _context.Entry(existingCustomer.ContactNumbers).CurrentValues.SetValues(updatedCustomerDTO);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/v1/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var contactNumbers = await _context.ContactNumbers.FirstOrDefaultAsync(cn => cn.CustomerId == id);

            if (contactNumbers != null)
            {
                _context.ContactNumbers.Remove(contactNumbers);
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
