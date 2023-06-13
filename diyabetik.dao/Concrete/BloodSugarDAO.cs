using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Abstract;
using diyabetik.model;
using Firebase.Database.Query;
using Firebase.Database;

namespace diyabetik.dao.Concrete
{
    public class BloodSugarDAO : IDAO<BloodSugar>
    {   
    
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");

        public Task<BloodSugar> Add(BloodSugar t)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id, string uid)
        {
            await firebaseClient.Child("blood_sugar_data").Child(uid).Child(id).DeleteAsync();
        }

        public async Task<List<BloodSugar>> GetAll()
        {
            var response = await firebaseClient.Child("blood_sugar_data").OnceAsync<BloodSugar>();
    
            List<BloodSugar> bloodSugarList = new List<BloodSugar>();

            foreach (var bloodSugarSnapshot in response)
            {
                BloodSugar bloodSugar = new BloodSugar
                {
                    Id = bloodSugarSnapshot.Key,
                    Date = bloodSugarSnapshot.Object.Date,
                    BloodSugarValue = bloodSugarSnapshot.Object.BloodSugarValue,
                };

                bloodSugarList.Add(bloodSugar);
            }

            return bloodSugarList;
        }

        public async Task<List<BloodSugar>> GetAllByUid(string id)
        {   
            UserDAO userDAO = new UserDAO();
            User user = await userDAO.GetById(id);
            string uid = user.Id;
            var response = await firebaseClient.Child("blood_sugar_data").Child(uid).OnceAsync<BloodSugar>();

            List<BloodSugar> bloodSugarList = new List<BloodSugar>();

            foreach (var bloodSugarSnapshot in response)
            {
                BloodSugar bloodSugar = new BloodSugar
                {
                    Id = bloodSugarSnapshot.Key,
                    Date = bloodSugarSnapshot.Object.Date,
                    BloodSugarValue = bloodSugarSnapshot.Object.BloodSugarValue,
                };

                bloodSugarList.Add(bloodSugar);
            }
            return bloodSugarList;
        }

        public Task<BloodSugar> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BloodSugar> Update(BloodSugar t)
        {
            throw new NotImplementedException();
        }
    }
}