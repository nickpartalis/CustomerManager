using CustomerManager.Exceptions;
using CustomerManager.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CustomerManager.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        private readonly string _connectionString;

        public SqlDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<CustomerWithNumbersDTO>> GetCustomers()
        {
            var customers = new List<CustomerWithNumbersDTO>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_GetCustomers", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var customer = MapCustomerFromReader(reader);
                            customers.Add(customer);
                        }
                    }
                }
            }

            return customers;
        }

        public async Task<CustomerWithNumbersDTO?> GetCustomer(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_GetCustomerById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapCustomerFromReader(reader);
                        }
                        else
                        {
                            return null; // Customer not found
                        }
                    }
                }
            }
        }

        public async Task<CustomerWithNumbersDTO> CreateCustomer(CustomerWithNumbersDTO customerDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_InsertCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    MapCustomerToParameters(command, customerDTO);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            customerDTO.Id = reader.GetInt32(reader.GetOrdinal("CustomerId"));
                        }
                    }
                }
            }

            return customerDTO;
        }

        public async Task<CustomerWithNumbersDTO> UpdateCustomer(int id, CustomerWithNumbersDTO customerDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                if (!await CustomerExists(id))
                {
                    throw new NotFoundException($"Customer with Id {id} not found.");
                }

                using (var command = new SqlCommand("usp_UpdateCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    MapCustomerToParameters(command, customerDTO);
                    command.Parameters.AddWithValue("@CustomerId", customerDTO.Id);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return customerDTO;
        }

        public async Task DeleteCustomer(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                if (!await CustomerExists(id))
                {
                    throw new NotFoundException($"Customer with Id {id} not found.");
                }

                using (var command = new SqlCommand("usp_DeleteCustomer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


        private async Task<bool> CustomerExists(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Customers] WHERE Id = @CustomerId", connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", id);

                    var userCount = (int)await command.ExecuteScalarAsync();

                    return userCount > 0;
                }
            }
        }

        private CustomerWithNumbersDTO MapCustomerFromReader(SqlDataReader reader)
        {
            return new CustomerWithNumbersDTO
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Address = reader.GetString(reader.GetOrdinal("Address")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                HomeNumber = reader.IsDBNull(reader.GetOrdinal("HomeNumber")) ? null : reader.GetString(reader.GetOrdinal("HomeNumber")),
                WorkNumber = reader.IsDBNull(reader.GetOrdinal("WorkNumber")) ? null : reader.GetString(reader.GetOrdinal("WorkNumber")),
                MobileNumber = reader.IsDBNull(reader.GetOrdinal("MobileNumber")) ? null : reader.GetString(reader.GetOrdinal("MobileNumber"))
            };
        }

        private void MapCustomerToParameters(SqlCommand command, CustomerWithNumbersDTO customer)
        {
            //command.Parameters.AddWithValue("@CustomerId", customer.Id);
            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@Address", customer.Address);
            command.Parameters.AddWithValue("@Email", customer.Email);
            command.Parameters.AddWithValue("@HomeNumber", customer.HomeNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@WorkNumber", customer.WorkNumber ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MobileNumber", customer.MobileNumber ?? (object)DBNull.Value);
        }
    }
}
