using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerManager.Data;
using CustomerManager.Models;
using CustomerManager.DataAccess;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using CustomerManager.Exceptions;

namespace CustomerManager.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IDataAccess _dataAccess;
        private readonly IValidator<CustomerWithNumbersDTO> _validator;

        public CustomerController(IDataAccess dataAccess, IValidator<CustomerWithNumbersDTO> validator)
        {
            _dataAccess = dataAccess;
            _validator = validator;
        }

        // GET: api/v1/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerWithNumbersDTO>>> GetCustomers()
        {
            try
            {
                var customers = await _dataAccess.GetCustomers();
                return Ok(customers);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        // GET: api/v1/customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerWithNumbersDTO>> GetCustomer(int id)
        {
            var customer = await _dataAccess.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/v1/customers
        [HttpPost]
        public async Task<ActionResult<CustomerWithNumbersDTO>> PostCustomer(CustomerWithNumbersDTO customerDTO)
        {
            var validationResult = _validator.Validate(customerDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var customer = await _dataAccess.CreateCustomer(customerDTO);
                return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/v1/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerWithNumbersDTO customerDTO)
        {
            if (id != customerDTO.Id)
            {
                return BadRequest();
            }

            var validationResult = _validator.Validate(customerDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var customer = await _dataAccess.UpdateCustomer(id, customerDTO);
                return Ok(customerDTO);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        //DELETE: api/v1/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _dataAccess.DeleteCustomer(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
