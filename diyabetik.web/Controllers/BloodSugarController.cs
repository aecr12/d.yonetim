using System.Reflection.PortableExecutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using diyabetik.dao.Concrete;

namespace diyabetik.web.Controllers
{
    public class BloodSugarController:Controller
    {
        BloodSugarDAO bloodSugarDAO = new BloodSugarDAO();
        
        // sayfa yenilenince veya başka sayfaya geçilince kullanıcı id bilgisinin kaybedilmemesi için context accessor kullanıldı
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BloodSugarController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // seçilen kullanıcıya ait verilerin getirilmesi
        public async Task<IActionResult> BloodSugarData(){
            var bloodSugarDataViewModel = new BloodSugarDataViewModel();
            bloodSugarDataViewModel.BloodSugarDatas=await bloodSugarDAO.GetAllByUid(GetId());
            
            return View(bloodSugarDataViewModel);
        }

        // sayfa yenilenince id bilgisini kaybetmemek için http contexte bu bilgi kaydediliyor
         public string GetId()
         {
             var id = _httpContextAccessor.HttpContext.Session.GetString("Id");
             return id;
         }

        // Ajaxdan gelen kullanıcı Id bilgisi yakalanıyor, bu bilgi seçilen kullanıcıya ait verilerin getirilmesini sağlayacak
        public void SetId(string id)
        {   
            
            _httpContextAccessor.HttpContext.Session.SetString("Id", id);
        }

        // datanın silinmesi
        public async Task<IActionResult> BloodSugarDelete(string id)
        {
            await bloodSugarDAO.Delete(id, GetId());
            return RedirectToAction("BloodSugarData");
        }
    }
}