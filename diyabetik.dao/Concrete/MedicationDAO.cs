using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Abstract;
using diyabetik.model;
using Firebase.Database;
using Firebase.Database.Query;

namespace diyabetik.dao.Concrete
{
    public class MedicationDAO : IDAO<Medication>
    {   
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");

        public Task<Medication> Add(Medication t)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id, string uid)
        {
            await firebaseClient.Child("medications").Child(uid).Child(id).DeleteAsync();
        }

        public async Task<List<Medication>> GetAll()
        {
            var response = await firebaseClient.Child("medications").OnceAsync<Medication>();
    
            List<Medication> medicationList = new List<Medication>();

            foreach (var medicationSnapshot in response)
            {
                Medication medication = new Medication
                {
                    Id = medicationSnapshot.Key,
                    TakenDate = medicationSnapshot.Object.TakenDate,
                    MedicineName = medicationSnapshot.Object.MedicineName,
                };

                medicationList.Add(medication);
            }

            return medicationList;
        }

        public async Task<List<Medication>> GetAllByUid(string id)
        {
            UserDAO userDAO = new UserDAO();
            User user = await userDAO.GetById(id);
            string uid = user.Id;
            var response = await firebaseClient.Child("medications").Child(uid).OnceAsync<Medication>();

            List<Medication> medicationList = new List<Medication>();

            foreach (var medicationSnapshot in response)
            {
                Medication medication = new Medication
                {
                    Id = medicationSnapshot.Key,
                    TakenDate = medicationSnapshot.Object.TakenDate,
                    MedicineName = medicationSnapshot.Object.MedicineName,
                };

                medicationList.Add(medication);
            }
            return medicationList;
        }

        public Task<Medication> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Medication> Update(Medication t)
        {
            throw new NotImplementedException();
        }
    }
}