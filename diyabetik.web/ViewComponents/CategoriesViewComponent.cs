using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.web.Data;
using diyabetik.model;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.ViewComponents
{

    // kategoriler için view component kullanıldı
    public class CategoriesViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke(string id)
        {
            return View(CategoryRepository.Categories);
        }
        
    }
}