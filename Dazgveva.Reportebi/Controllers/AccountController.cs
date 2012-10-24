using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Dapper;

namespace Dazgveva.Reportebi.Controllers
{
    public class LoginInput
    {
        public string ReturnUrl { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
    }

    public class Account
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
    }

    public class AccountController : Controller
    {
        [RequireHttps]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectFromLoginPage();
            }

            return View("Avtorizacia", new LoginInput { ReturnUrl = returnUrl });
        }

        [RequireHttps]
        [HttpPost]
        public ActionResult Login(LoginInput input)
        {
            if (ModelState.IsValid)
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
                {
                    conn.Open();
                    Account user = conn.Query<Account>
                        (
                            "SELECT * FROM INSURANCEW.dbo.SadazgveoebisAccountebi (nolock) WHERE Name = @name",
                            new { name = input.Name }
                        )
                        .ToList()
                        .FirstOrDefault();

                    string pass = GetPasswordHash(input.Name, input.Pass);

                    if (user != null && user.Pass != string.Empty && user.Pass == pass)
                    {
                        FormsAuthentication.SetAuthCookie(input.Name, createPersistentCookie: true);
                        return RedirectFromLoginPage(input.ReturnUrl);
                    }
                    else {
                        // TODO: display error
                    }
                }

            return View("Avtorizacia", new LoginInput { Name = input.Name, ReturnUrl = input.ReturnUrl });
        }

        private ActionResult RedirectFromLoginPage(string retrunUrl = null)
        {
            if (string.IsNullOrEmpty(retrunUrl))
                return RedirectToRoute("Default");

            return Redirect(retrunUrl);
        }

        public ActionResult Logout(string returnurl)
        {
            FormsAuthentication.SignOut();
            return RedirectFromLoginPage("~/");
        }

        public string GetPasswordHash(string userName, string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bytes = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", userName, password));
            var computeHash = md5.ComputeHash(bytes);
            return BitConverter.ToString(computeHash).Replace("-", "");
        }
    }
}
