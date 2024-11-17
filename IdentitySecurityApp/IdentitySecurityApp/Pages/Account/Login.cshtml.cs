using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace IdentityServerApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if(!ModelState.IsValid) return Page();
            
            //Kimlik Bilgilerini Do�rula / Verify the credentials
            if(Credential.UserName == "admin" && Credential.Password == "password") 
            {
                //G�venlik Kriterlerini Olu�turuyoruz / Creating the security context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@haaliron.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin","true"),
                    new Claim("Manager","true"),
                    new Claim("EmploymentDate","2024-01-01"),
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);

                return RedirectToPage("/Index");
            }
            return Page();
        }
    }

    public class Credential 
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
