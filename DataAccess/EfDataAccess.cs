using CustomerManager.Data;
using CustomerManager.Exceptions;
using CustomerManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManager.DataAccess
{
    public class EfDataAccess : IDataAccess
    {
        private readonly AppDbContext _context;

        public EfDataAccess(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerWithNumbersDTO>> GetCustomers()
        {
            var customers = await _context.Customers
                .Include(c => c.ContactNumbers)
                .ToListAsync();

            var customerDTOs = customers.Select(customer => new CustomerWithNumbersDTO
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Email = customer.Email,
                HomeNumber = customer.ContactNumbers?.HomeNumber,
                WorkNumber = customer.ContactNumbers?.WorkNumber,
                MobileNumber = customer.ContactNumbers?.MobileNumber
            });

            return customerDTOs;
        }

        public async Task<CustomerWithNumbersDTO?> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.ContactNumbers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return null;
            }

            var customerDTO = new CustomerWithNumbersDTO
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

        public async Task<CustomerWithNumbersDTO> CreateCustomer(CustomerWithNumbersDTO customerDTO)
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
                CustomerId = customer.Id
            };

            _context.ContactNumbers.Add(contactNumbers);
            await _context.SaveChangesAsync();

            customerDTO.Id = customer.Id;

            return customerDTO;
        }
        public async Task<CustomerWithNumbersDTO> UpdateCustomer(int id, CustomerWithNumbersDTO customerDTO)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {id} not found.");
            }

            customer.FirstName = customerDTO.FirstName;
            customer.LastName = customerDTO.LastName;
            customer.Address = customerDTO.Address;
            customer.Email = customerDTO.Email;

            var contactNumbers = await _context.ContactNumbers.FirstOrDefaultAsync(cn => cn.CustomerId == id);

            if (contactNumbers == null)
            {
                throw new NotFoundException($"ContactNumbers for customer with ID {id} not found.");
            }

            contactNumbers.HomeNumber = customerDTO.HomeNumber;
            contactNumbers.WorkNumber = customerDTO.WorkNumber;
            contactNumbers.MobileNumber = customerDTO.MobileNumber;

            await _context.SaveChangesAsync();

            return customerDTO;
        }

        public async Task DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with ID {id} not found.");
            }

            var contactNumbers = await _context.ContactNumbers.FirstOrDefaultAsync(cn => cn.CustomerId == id);

            if (contactNumbers != null)
            {
                _context.ContactNumbers.Remove(contactNumbers);
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
