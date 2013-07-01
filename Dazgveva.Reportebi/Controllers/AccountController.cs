using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Dazgveva.Reportebi.Models;

namespace Dazgveva.Reportebi.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectFromLoginPage();
            }

            return View(new LoginInput { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public ActionResult Login(LoginInput input)
        {
            var dc = new UsersDataContext();
            var user = dc.Users.FirstOrDefault(x => x.USER_NAME == input.Email);
            dc.Dispose();

            if (user == null || GetPasswordHash(input.Email, input.Password) != user.PSWHASH)
            {
                ModelState.AddModelError("UserNotExistOrPasswordNotMatch", "მომხმარებლის სახელი ან პაროლი არ ემთხვევა");
            }
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(input.Email, createPersistentCookie: false);
                return RedirectFromLoginPage(input.ReturnUrl);
            }

            return View(new LoginInput { Email = input.Email, ReturnUrl = input.ReturnUrl });
        }

        [ChildActionOnly]
        public ActionResult CurrentUser()
        {
            if (Request.IsAuthenticated == false)
                return View();
            var dc = new UsersDataContext();
            var user = dc.Users.First(x => x.USER_NAME == HttpContext.User.Identity.Name);
            ViewBag.UserName = user.USER_FULL_NAME;
            return View();
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
            var bytes = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", userName.ToUpper(), password.ToUpper()));
            var computeHash = md5.ComputeHash(bytes);
            return BitConverter.ToString(computeHash).Replace("-", "");
        }
        public void Test()
        {
            Console.WriteLine(GetPasswordHash("bimqu", "1973"));
            Console.WriteLine(GetPasswordHash("BIMQU", "1973"));
            Console.WriteLine(GetPasswordHash("mediacia", "med1@cia"));

            Console.WriteLine(GetPasswordHash("t.kotishadze@evillage.gov.ge", "123"));

        }
    }
}