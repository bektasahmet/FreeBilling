using FreeBilling.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreeBilling.Web.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IEmailService _emailService;

        public ContactModel(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public string Title { get; set; } = "Contact Me";
       
       public string Message { get; set; } = "";

        [BindProperty]
        public ContactViewModel Contact { get; set; } = new ContactViewModel()
        {
            Name = "Ahmet Bektaþ"
        };
        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                _emailService.SendMail("ahmet.bektass81@hotmail.com", Contact.Email, Contact.Subject, Contact.Body);
                Contact = new ContactViewModel();
                ModelState.Clear();
                Message = "Success, Sent!";
            }
            else
            {
                Message = "Error!";
            }

        }
    }
}
