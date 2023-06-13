using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Concrete;
using diyabetik.web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.Controllers
{
    public class TreatmentChoiceController:Controller
    {   
        TreatmentChoiceDAO treatmentChoiceDAO = new TreatmentChoiceDAO();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TreatmentChoiceController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> TreatmentChoiceData(){
            var treatmentChoiceDataViewModel = new TreatmentChoiceDataViewModel();
            treatmentChoiceDataViewModel.TreatmentChoiceDatas = await treatmentChoiceDAO.GetAllByUid(GetId());
            return View(treatmentChoiceDataViewModel);
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

        public async Task<IActionResult> TreatmentChoiceDelete(string id)
        {
            await treatmentChoiceDAO.Delete(id, GetId());
            return RedirectToAction("TreatmentChoiceData");
        }
    }
}