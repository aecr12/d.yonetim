using System.ComponentModel;
using diyabetik.dao.Concrete;
using diyabetik.web.Notifications;
using diyabetik.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.Controllers
{   
    // giriş yapmadan home sayfasına gidilmemesi için authorize eklendi
    [Authorize]
    public class HomeController:Controller
    {   
        
        UserDAO userDAO = new UserDAO();

        // home sayfasında kullamnıcılar listelendi, viewmodel kullanıldı
        public async Task<IActionResult> Index(){
            var userViewModel = new UserViewModel();
            userViewModel.Users = await userDAO.GetAll();
            return View(userViewModel);
        }
    }
}