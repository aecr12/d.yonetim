using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Concrete;
using diyabetik.web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.Controllers
{
    public class MedicationController:Controller
    {   
        // BloodSugar controllerde yapılan işlemler aynı mantıkta devam ediyor
        MedicationDAO medicationDAO = new MedicationDAO();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MedicationController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> MedicationData(){
            var medicationDataViewModel = new MedicationDataViewModel();
            medicationDataViewModel.MedicationDatas= await medicationDAO.GetAllByUid(GetId());
            return View(medicationDataViewModel);
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

        public async Task<IActionResult> MedicationDelete(string id)
        {
            await medicationDAO.Delete(id, GetId());
            return RedirectToAction("MedicationData");
        }
    }
}