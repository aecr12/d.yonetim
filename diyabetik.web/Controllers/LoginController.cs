using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Concrete;
using diyabetik.model;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace diyabetik.web.Controllers
{   
    public class LoginController:Controller
    {   
        AdminDAO adminDAO = new AdminDAO();

        //Login sayfası, login sayfasına giriş yapmadan görüntülenebilmesi için AllowAnonymous eklendi
        [AllowAnonymous]
        public IActionResult Login()
        {   
            if (HttpContext.Session.GetString("LoggedIn") == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // giriş yapan adminin bilgileri form üzerinden gönderildi ve giriş yapan kullanıcı için session oluşturuldu
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ReceiveAdminInformations(Admin admin)
        {
            Admin administrator = await adminDAO.GetByMailAndPassword(admin.Mail, admin.Password);
            if (administrator != null)
            {   
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, admin.Mail)
                };

                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                HttpContext.Session.SetString("Mail", admin.Mail);
                HttpContext.Session.SetString("LoggedIn", "true");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Console.WriteLine("else içinde: ");
                return RedirectToAction("Login", "Login");
            }
        }

        // logout işlemiyle session sonlandırılıp, login sayfasına yönlendirme yapıldı
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("LoggedIn", "true");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
    }
}