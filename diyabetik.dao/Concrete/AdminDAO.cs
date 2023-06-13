using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Abstract;
using diyabetik.model;
using Firebase.Database;

namespace diyabetik.dao.Concrete
{
    public class AdminDAO : IDAO<Admin>
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");
        public Task<Admin> Add(Admin t)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id, string uid)
        {
            throw new NotImplementedException();
        }

        // Adminlerin getirilmesi
        public async Task <List<Admin>> GetAll()
        {
            var response = await firebaseClient.Child("Admin").OnceAsync<Admin>();
            Console.WriteLine("Response: "+ response);
            List<Admin> adminList = new List<Admin>();

            foreach (var adminSnapshot in response)
            {
                Admin admin = new Admin
                {
                    AdminId = adminSnapshot.Key,
                    Mail = adminSnapshot.Object.Mail,
                    Password = adminSnapshot.Object.Password
                };

                adminList.Add(admin);
            }

            return adminList;
        }

        // Giriş yaparken doğrulama yapılmasını sağlayacak metot
        public async Task<Admin> GetByMailAndPassword(string Mail, string Password)
        {
            List<Admin> adminList = new List<Admin>();
            adminList = await GetAll();
            Admin admin = adminList.FirstOrDefault(a=>a.Mail==Mail && a.Password==Password);
            return admin;
        }
        public Task<List<Admin>> GetAllByUid(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> Update(Admin t)
        {
            throw new NotImplementedException();
        }
    }
}