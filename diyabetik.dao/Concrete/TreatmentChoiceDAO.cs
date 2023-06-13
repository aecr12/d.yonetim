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
    public class TreatmentChoiceDAO:IDAO<TreatmentChoice>
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");

        public Task<TreatmentChoice> Add(TreatmentChoice t)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id, string uid)
        {
            await firebaseClient.Child("treatment_choices").Child(uid).Child(id).DeleteAsync();
        }

        public async Task<List<TreatmentChoice>> GetAll()
        {
            var response = await firebaseClient.Child("treatment_choices").OnceAsync<TreatmentChoice>();
    
            List<TreatmentChoice> treatmentChoiceList = new List<TreatmentChoice>();

            foreach (var treatmentChoiceSnapshot in response)
            {
                TreatmentChoice treatmentChoice = new TreatmentChoice
                {
                    Id = treatmentChoiceSnapshot.Key,
                    Insulin = treatmentChoiceSnapshot.Object.Insulin,
                    Pump = treatmentChoiceSnapshot.Object.Pump,
                    InsulinAntidiabetic = treatmentChoiceSnapshot.Object.InsulinAntidiabetic,
                    OralAntidiabetic = treatmentChoiceSnapshot.Object.OralAntidiabetic,
                };

                treatmentChoiceList.Add(treatmentChoice);
            }

            return treatmentChoiceList;
        }

        public async Task<List<TreatmentChoice>> GetAllByUid(string id)
        {
            UserDAO userDAO = new UserDAO();
            User user = await userDAO.GetById(id);
            string uid = user.Id;
            var response = await firebaseClient.Child("treatment_choices").Child(uid).OnceAsync<TreatmentChoice>();

            List<TreatmentChoice> treatmentChoiceList = new List<TreatmentChoice>();

            foreach (var treatmentChoiceSnapshot in response)
            {
                TreatmentChoice treatmentChoice = new TreatmentChoice
                {
                    Id = treatmentChoiceSnapshot.Key,
                    Insulin = treatmentChoiceSnapshot.Object.Insulin,
                    Pump = treatmentChoiceSnapshot.Object.Pump,
                    InsulinAntidiabetic = treatmentChoiceSnapshot.Object.InsulinAntidiabetic,
                    OralAntidiabetic = treatmentChoiceSnapshot.Object.OralAntidiabetic,
                };

                treatmentChoiceList.Add(treatmentChoice);
            }

            return treatmentChoiceList;
        }

        public Task<TreatmentChoice> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TreatmentChoice> Update(TreatmentChoice t)
        {
            throw new NotImplementedException();
        }
    }
}