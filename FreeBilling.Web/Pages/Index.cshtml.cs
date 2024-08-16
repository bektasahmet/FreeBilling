using FreeBilling.Data.Entities;
using FreeBilling.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FreeBilling.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BillingContext _context;

        public IndexModel(BillingContext context)
        {
            _context = context;
        }

        public List<Customer>? Customers { get; set; }
        public async Task OnGetAsync()
        {
            Customers = await _context.Customers.ToListAsync(); 
        }
    }
}
