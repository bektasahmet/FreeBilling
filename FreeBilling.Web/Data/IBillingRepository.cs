using FreeBilling.Data.Entities;

namespace FreeBilling.Web.Data
{
    public interface IBillingRepository
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<IEnumerable<Customer>> GetCustomersWithAddresses();
        Task<Customer?> GetCustomerById(int id);
        Task<Customer?> GetCustomerByName(string name);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<bool> SaveChanges();
        Task<TimeBill?> GetTimeBill(int id);
        void AddEntity<T>(T entity) where T : notnull;
        Task<IEnumerable<TimeBill>> GetTimeBillsForCustomer(int id);
        Task<TimeBill?> GetTimeBillForCustomer(int id, int billId);
        Task<Employee?> GetEmployee(string? name);
    }
}