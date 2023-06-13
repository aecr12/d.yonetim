using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Concrete;
using diyabetik.web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.Controllers
{
    public class StepCounterController:Controller
    {   
        StepCounterDAO stepCounterDAO = new StepCounterDAO();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StepCounterController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> StepCounterData(){
            var stepCounterDataViewModel = new StepCounterDataViewModel();
            stepCounterDataViewModel.StepCounterDatas=await stepCounterDAO.GetAllByUid(GetId());
            return View(stepCounterDataViewModel);
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
        public async Task<IActionResult> StepCounterDelete(string id)
        {
            await stepCounterDAO.Delete(id, GetId());
            return RedirectToAction("StepCounterData");
        }
    }
}