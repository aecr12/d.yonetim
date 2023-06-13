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
    public class StepCounterDAO:IDAO<StepCounter>
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");

        public Task<StepCounter> Add(StepCounter t)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id, string uid)
        {   
            var response = await firebaseClient.Child("step_counter_data").Child(uid).OnceAsync<StepCounter>();

            foreach (var dateSnapshot in response)
            {
                var date = dateSnapshot.Key;
                var stepCounterData = await firebaseClient.Child("step_counter_data").Child(uid).Child(date).OnceAsync<StepCounter>();

                foreach (var stepCounterSnapshot in stepCounterData)
                {
                    if (stepCounterSnapshot.Key == id)
                    {
                        await firebaseClient.Child("step_counter_data").Child(uid).Child(date).Child(id).DeleteAsync();
                        return;
                    }
                }
            }
        }

        public async Task<List<StepCounter>> GetAll()
        {
            var response = await firebaseClient.Child("step_counter_data").OnceAsync<StepCounter>();
    
            List<StepCounter> stepCounterList = new List<StepCounter>();

            foreach (var stepCounterSnapshot in response)
            {
                StepCounter stepCounter = new StepCounter
                {
                    Id = stepCounterSnapshot.Key,
                    StepCount = stepCounterSnapshot.Object.StepCount,
                };

                stepCounterList.Add(stepCounter);
            }

            return stepCounterList;
        }

        public async Task<List<StepCounter>> GetAllByUid(string id)
        {
            UserDAO userDAO = new UserDAO();
            User user = await userDAO.GetById(id);
            string uid = user.Id;
            var response = await firebaseClient.Child("step_counter_data").Child(uid).OnceAsync<StepCounter>();

            List<StepCounter> stepCounterList = new List<StepCounter>();

            foreach (var dateSnapshot in response)
            {
                var date = dateSnapshot.Key;
                var stepCounterData = await firebaseClient.Child("step_counter_data").Child(uid).Child(date).OnceAsync<StepCounter>();

                foreach (var stepCounterSnapshot in stepCounterData)
                {
                    StepCounter stepCounter = new StepCounter
                    {
                        Id = stepCounterSnapshot.Key,
                        StepCount = stepCounterSnapshot.Object.StepCount,
                    };

                    stepCounterList.Add(stepCounter);
                }
            }

            return stepCounterList;
        }

        public Task<StepCounter> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<StepCounter> Update(StepCounter t)
        {
            throw new NotImplementedException();
        }
    }
}