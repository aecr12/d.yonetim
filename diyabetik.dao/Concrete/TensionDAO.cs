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
    public class TensionDAO:IDAO<Tension>
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");

        public Task<Tension> Add(Tension t)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id, string uid)
        {
            await firebaseClient.Child("tension_data").Child(uid).Child(id).DeleteAsync();
        }

        public async Task<List<Tension>> GetAll()
        {
            var response = await firebaseClient.Child("tension_data").OnceAsync<Tension>();
    
            List<Tension> tensionList = new List<Tension>();

            foreach (var tensionSnapshot in response)
            {
                Tension tension = new Tension
                {
                    Id = tensionSnapshot.Key,
                    Date = tensionSnapshot.Object.Date,
                    Diastolic = tensionSnapshot.Object.Diastolic,
                    Systolic = tensionSnapshot.Object.Systolic,
                };

                tensionList.Add(tension);
            }

            return tensionList;
        }

        public async Task<List<Tension>> GetAllByUid(string id)
        {
            UserDAO userDAO = new UserDAO();
            User user = await userDAO.GetById(id);
            string uid = user.Id;
            var response = await firebaseClient.Child("tension_data").Child(uid).OnceAsync<Tension>();

            List<Tension> tensionList = new List<Tension>();

            foreach (var tensionSnapshot in response)
            {
                Tension tension = new Tension
                {
                    Id = tensionSnapshot.Key,
                    Date = tensionSnapshot.Object.Date,
                    Diastolic = tensionSnapshot.Object.Diastolic,
                    Systolic = tensionSnapshot.Object.Systolic,
                };

                tensionList.Add(tension);
            }
            return tensionList;
        }

        public Task<Tension> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Tension> Update(Tension t)
        {
            throw new NotImplementedException();
        }
    }
}