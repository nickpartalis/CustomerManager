using CustomerManager.Models;

namespace CustomerManager.DataAccess
{
    public interface IDataAccess
    {
        Task<IEnumerable<CustomerWithNumbersDTO>> GetCustomers();
        Task<CustomerWithNumbersDTO?> GetCustomer(int id);
        Task<CustomerWithNumbersDTO> CreateCustomer(CustomerWithNumbersDTO customerDTO);
        Task<CustomerWithNumbersDTO> UpdateCustomer(int id, CustomerWithNumbersDTO customerDTO);
        Task DeleteCustomer(int id);
    }
}
