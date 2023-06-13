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
    public class UserInformationDAO:IDAO<UserInformation>
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");

        public Task<UserInformation> Add(UserInformation t)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id, string uid)
        {
            await firebaseClient.Child("user_informations").Child(uid).Child(id).DeleteAsync();
        }

        public async Task<List<UserInformation>> GetAll()
        {
            var response = await firebaseClient.Child("user_informations").OnceAsync<UserInformation>();
    
            List<UserInformation> userInformationList = new List<UserInformation>();

            foreach (var userInformationSnapshot in response)
            {
                UserInformation userInformation = new UserInformation
                {
                    Id = userInformationSnapshot.Key,
                    Height = userInformationSnapshot.Object.Height,
                    Weight = userInformationSnapshot.Object.Weight,
                    Waist = userInformationSnapshot.Object.Waist,
                    HbA1cPercent = userInformationSnapshot.Object.HbA1cPercent,
                };

                userInformationList.Add(userInformation);
            }

            return userInformationList;
        }

        public async Task<List<UserInformation>> GetAllByUid(string id)
        {
            UserDAO userDAO = new UserDAO();
            User user = await userDAO.GetById(id);
            string uid = user.Id;
            var response = await firebaseClient.Child("user_informations").Child(uid).OnceAsync<UserInformation>();

            List<UserInformation> userInformationList = new List<UserInformation>();

            foreach (var userInformationSnapshot in response)
            {
                UserInformation userInformation = new UserInformation
                {
                    Id = userInformationSnapshot.Key,
                    Height = userInformationSnapshot.Object.Height,
                    Weight = userInformationSnapshot.Object.Weight,
                    Waist = userInformationSnapshot.Object.Waist,
                    HbA1cPercent = userInformationSnapshot.Object.HbA1cPercent,
                };
                userInformationList.Add(userInformation);
            }
            return userInformationList;
        }

        public Task<UserInformation> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UserInformation> Update(UserInformation t)
        {
            throw new NotImplementedException();
        }
    }
}