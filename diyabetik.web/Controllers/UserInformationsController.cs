using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Concrete;
using diyabetik.web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.Controllers
{
    public class UserInformationsController:Controller
    {   
        UserInformationDAO userInformationDAO = new UserInformationDAO();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInformationsController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> UserInformationsData(){
            var userInformationsDataViewModel = new UserInformationsDataViewModel();
            userInformationsDataViewModel.UserInformationsDatas = await userInformationDAO.GetAllByUid(GetId());
            return View(userInformationsDataViewModel);
        }
        public string GetId()
        {
            var id = _httpContextAccessor.HttpContext.Session.GetString("Id");
            Console.WriteLine("GetId içinde: " + id);
            return id;
        }

        public void SetId(string id)
        {
            _httpContextAccessor.HttpContext.Session.SetString("Id", id);
        }
        public async Task<IActionResult> UserInformationDelete(string id)
        {
            await userInformationDAO.Delete(id, GetId());
            return RedirectToAction("UserInformationsData");
        }
    }
}