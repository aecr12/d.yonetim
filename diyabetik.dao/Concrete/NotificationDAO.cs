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
    public class NotificationDAO
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://diyabetik-82d33-default-rtdb.firebaseio.com/");

        public async Task<PlannedNotification> SaveNotification(PlannedNotification plannedNotification)
        {
            await firebaseClient.Child("notification_state").Child(plannedNotification.NotificationId).PutAsync(plannedNotification);
            return plannedNotification;
        }

        public async Task<List<PlannedNotification>> GetAll()
        {
            var response = await firebaseClient.Child("notification_state").OnceAsync<PlannedNotification>();

            List<PlannedNotification> plannedNotificationList = new List<PlannedNotification>();

            foreach (var plannedNotificationSnapshot in response)
            {
                PlannedNotification plannedNotification = new PlannedNotification
                {
                    NotificationId = plannedNotificationSnapshot.Key,
                    Title = plannedNotificationSnapshot.Object.Title,
                    Body = plannedNotificationSnapshot.Object.Body,
                    Date = plannedNotificationSnapshot.Object.Date,
                    State = plannedNotificationSnapshot.Object.State
                };

                plannedNotificationList.Add(plannedNotification);
            }

            return plannedNotificationList;
        }

        public async Task<PlannedNotification> GetNotificationByNotificationId(string notificationId)
        {
            List<PlannedNotification> plannedNotificationList = new List<PlannedNotification>();
            plannedNotificationList = await GetAll();
            PlannedNotification plannedNotification = plannedNotificationList.FirstOrDefault(n=>n.NotificationId==notificationId);
            return plannedNotification;
        }

        public async Task<PlannedNotification> UpdateNotificationState(PlannedNotification plannedNotification)
        {
            await firebaseClient.Child("notification_state").Child(plannedNotification.NotificationId).PutAsync(plannedNotification);
            return plannedNotification;
        }

        // public async Task<PlannedNotification> GetNotificationStateById(string notificationId)
        // {
        //     var notification = Get
        // }
    }
}