using diyabetik.dao.Concrete;
using diyabetik.model;
using diyabetik.web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.Controllers
{
    public class UserController:Controller
    {   
        UserDAO userDAO = new UserDAO();

        // kullanıcı listesinin görüntülenmesi
        public async Task<IActionResult> list(int? id, string query){
            var userViewModel = new UserViewModel();
            userViewModel.Users = await userDAO.GetAll();
            return View(userViewModel);
        }

        // kullanıcı seçildikten sonra ona ait verilerin görüntülenmesi ve yönetilmesi için oluşturulan sayfa
        public async Task<IActionResult> UserDetails(string id)
        {   
            return View(await userDAO.GetById(id));
        }

        // kullancıyı silme
        public async Task<IActionResult> UserDelete(string id)
        {
            await userDAO.DeleteUserFromAuthentication(id);
            await userDAO.Delete(id, id);
            return RedirectToAction("list");
        }

        [HttpGet]
        public IActionResult Create()
        { 
            return View(new User());
        }


        // Kullanıcı oluşturmak için Create formundan gelen bilgilerin  işlenmesi ve create işleminin yapılması
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {   
            // Giriş bilgileri kullanılan kısıtlamaya uygunsa kullanıcıyı oluştur.
            if(ModelState.IsValid)
            {   
                await userDAO.CreateFirebaseUser(user);
                return RedirectToAction("list");
            }else{
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(user);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return View(await userDAO.GetById(id));
        }

        // Kullanıcı bilgilerini değiştirmek için edit formdan gelen bilgilerin işlenmesi ve edit işleminin yapılması
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {   
            if(ModelState.IsValid)
            {
                await userDAO.Update(user);
                return RedirectToAction("userdetails", new {id = user.Id});
            }else
            {
                return View();
            }
            
        }
    }
}