using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Concrete;
using diyabetik.web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.Controllers
{
    public class TensionController:Controller
    {   
        TensionDAO tensionDAO = new TensionDAO();
    
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TensionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> TensionData(){
            var tensionDataViewModel = new TensionDataViewModel();
            tensionDataViewModel.TensionDatas =await tensionDAO.GetAllByUid(GetId());
            return View(tensionDataViewModel);
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

        public async Task<IActionResult> TensionDelete(string id)
        {
            await tensionDAO.Delete(id, GetId());
            return RedirectToAction("TensionData");
        }
    }
}