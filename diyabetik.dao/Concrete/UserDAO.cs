using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.dao.Abstract;
using diyabetik.model;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
namespace diyabetik.dao.Concrete
{
    public class UserDAO : IDAO<User>
    {   

        
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");
    
        public async Task<List<User>> GetAll()
        {
            var response = await firebaseClient.Child("users").OnceAsync<User>();

            List<User> userList = new List<User>();

            foreach (var userSnapshot in response)
            {
                User user = new User
                {
                    Id = userSnapshot.Key,
                    adSoyad = userSnapshot.Object.adSoyad,
                    Mail = userSnapshot.Object.Mail,
                    Password = userSnapshot.Object.Password
                };

                userList.Add(user);
            }

            return userList;
        }

        public Task<List<User>> GetAllByUid(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(string id)
        {
            List<User> userList = new List<User>();
            userList = await GetAll();
            User user = userList.FirstOrDefault(u=>u.Id==id);
            return user;
        }

        public async Task DeleteUserFromAuthentication(string id)
        {
            FirebaseAdmin firebaseAdmin = new FirebaseAdmin();
            var auth = FirebaseAuth.GetAuth(firebaseAdmin.GetFirebaseApp());
            await auth.DeleteUserAsync(id);
        }

        public async Task Delete(string id, string uid)
        {
            await firebaseClient.Child("users").Child(id).DeleteAsync();
            await firebaseClient.Child("blood_sugar_data").Child(id).DeleteAsync();
            await firebaseClient.Child("medications").Child(id).DeleteAsync();
            await firebaseClient.Child("step_counter_data").Child(id).DeleteAsync();
            await firebaseClient.Child("tension_data").Child(id).DeleteAsync();
            await firebaseClient.Child("treatment_choices").Child(id).DeleteAsync();
            await firebaseClient.Child("user_informations").Child(id).DeleteAsync();
            await firebaseClient.Child("water_count_data").Child(id).DeleteAsync();
            await firebaseClient.Child("meals").Child("breakfast_data").Child(id).DeleteAsync();
            await firebaseClient.Child("meals").Child("dinner_data").Child(id).DeleteAsync();
            await firebaseClient.Child("meals").Child("lunch_data").Child(id).DeleteAsync();
            await firebaseClient.Child("meals").Child("snack_data").Child(id).DeleteAsync();
        }

        public async Task<User> Add(User user)
        {
            var newChildRef = await firebaseClient.Child("users").PostAsync(user);
            user.Id = newChildRef.Key;
            await firebaseClient.Child("users").Child(user.Id).PutAsync(user);
            return user;
        }

        public async Task<UserRecord> CreateFirebaseUser(User user)
        {   
            FirebaseAdmin firebaseAdmin = new FirebaseAdmin();
            FirebaseApp app = firebaseAdmin.GetFirebaseApp();
            var auth = FirebaseAuth.GetAuth(app);
            if (auth == null)
            {
                throw new Exception("Firebase Authentication başlatılamadı.");
            }

            var userRecordArgs = new UserRecordArgs()
            {
                Email = user.Mail,
                Password = user.Password,
                DisplayName = user.adSoyad,
            };
            var userRecord = await auth.CreateUserAsync(userRecordArgs);

            user.Id = userRecord.Uid;
            await firebaseClient.Child("users").Child(user.Id).PutAsync(user);

            return userRecord;
        }

        public async Task<User> Update(User user)
        {
            await firebaseClient.Child("users").Child(user.Id).PutAsync(user);
            return user;
        }
    }
}