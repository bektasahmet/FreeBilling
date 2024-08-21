using FreeBilling.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreeBilling.Web.Data
{
    public class BillingRepository : IBillingRepository
    {
        private readonly BillingContext _context;
        private readonly ILogger _logger;

        public BillingRepository(BillingContext context, ILogger<BillingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            try
            {
                return await _context.Employees
                .OrderBy(e => e.Name)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Couldn't get the employee: {ex.Message}");
                throw;
            }

        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            try
            {
                return await _context.Customers
                .OrderBy(e => e.CompanyName)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Couldn't get the custoer: {ex.Message}");
                throw;
            }
            
        }

        public async Task<IEnumerable<Customer>> GetCustomersWithAddresses()
        {
            try
            {
                return await _context.Customers
                .Include(e => e.Address)
                .OrderBy(e => e.CompanyName)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Couldn't get the custoer: {ex.Message}");
                throw;
            }

        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Customer?> GetCustomerById(int id)
        {
            return await _context.Customers
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Customer?> GetCustomerByName(string name)
        {
            return await _context.Customers
                .Where(c => c.CompanyName == name)
                .FirstOrDefaultAsync();
        }

        public async Task<TimeBill?> GetTimeBill(int id)
        {
            var bill = await _context.TimeBills
                .Include(b => b.Employee)
                .Include(b => b.Customer)
                .ThenInclude(c => c!.Address)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            return bill;
        }

        public void AddEntity<T>(T entity) where T : notnull
        {
            _context.Add(entity);
        }

        public async Task<IEnumerable<TimeBill>> GetTimeBillsForCustomer(int id)
        {
            return await _context.TimeBills
                .Where(c => c.Customer != null && c.CustomerId == id)
                .Include(c => c.Customer)
                .Include(c => c.Employee)
                .ToListAsync();
        }

        public async Task<TimeBill?> GetTimeBillForCustomer(int id, int billId)
        {
            return await _context.TimeBills
            .Where(c => c.Customer != null && c.CustomerId == id && c.Id == billId)
            .Include(c => c.Customer)
            .Include(c => c.Employee)
            .FirstOrDefaultAsync();
        }

        public async Task<Employee?> GetEmployee(string? name)
        {
            return await _context.Employees
                .Where(c => c.Email == name)
                .FirstOrDefaultAsync();
        }
    }
}
