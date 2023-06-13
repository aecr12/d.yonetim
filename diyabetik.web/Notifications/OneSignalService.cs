using System.Globalization;
using System;
using System.Collections.Generic;
using System.Text;
using diyabetik.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using diyabetik.dao.Concrete;

namespace diyabetik.web.Notifications
{
    public class OneSignalService
    {   
        // Rest api üzerinden one signale bağlantı
        private const string OneSignalRestApiUrl = "https://onesignal.com/api/v1/notifications";
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _appId;
        NotificationDAO notificationDAO = new NotificationDAO();
        public OneSignalService()
        {
            _apiKey = "NWEzNDExNWUtYThlOC00ODRkLWE5ZjktZDRkZDFmYTNlYmJk";
            _appId = "6ff7609c-241f-4518-aed1-84be6cde7710";
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://onesignal.com/api/v1/notifications");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {_apiKey}");
            
        }

        // Kullanıcılara push bildirim gönderildi
        public async Task<bool> SendNotificationToAllUsers(Notification notification)
        {
            var requestContent = new Dictionary<string, object>
            {
                { "app_id", _appId },
                { "included_segments", new List<string> { "All" } },
                { "headings", new Dictionary<string, string> { { "en", notification.Title } } },
                { "contents", new Dictionary<string, string> { { "en", notification.Body } } }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("notifications", content);

            return response.IsSuccessStatusCode;
        }


        // kullanıcılara zaman planlı bildirim gönderme
        public async Task<bool> SendPlannedNotificationToAllUsers(PlannedNotification plannedNotification)
        {
            var scheduledTime = plannedNotification.Date;
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(scheduledTime);
            var scheduleString = utcTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var requestContent = new Dictionary<string, object>
            {
                { "app_id", _appId },
                { "included_segments", new List<string> { "All" } },
                { "headings", new Dictionary<string, string> { { "en", plannedNotification.Title } } },
                { "contents", new Dictionary<string, string> { { "en", plannedNotification.Body } } },
                { "send_after", scheduleString }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(requestContent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("notifications", content);
            if(response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

                // bildirim henüz gönderilmediyse iptal edebilmek için responseden id çekildi
                plannedNotification.NotificationId = responseData["id"].ToString();
                plannedNotification.State = true;
                await notificationDAO.SaveNotification(plannedNotification);
                return true;
            }
            return response.IsSuccessStatusCode;
        }
        
        // Gönderilmiş bildirimlerin alınması
        public async Task<List<PlannedNotification>> GetSentNotifications()
        {
            var requestUrl = $"notifications?app_id={_appId}&limit=10&filter=scheduled";
            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                if (responseData.TryGetValue("notifications", out var notificationsData) && notificationsData is JArray notificationsArray)
                {
                    var plannedNotifications = new List<PlannedNotification>();
                    foreach (var notificationData in notificationsArray)
                    {
                        var title = notificationData["headings"]?["en"]?.ToString();
                        var body = notificationData["contents"]?["en"]?.ToString();
                        var scheduledTimeString = notificationData["send_after"]?.ToString();
                        var scheduledTimeUnix = long.Parse(scheduledTimeString);
                        var scheduledTime = DateTimeOffset.FromUnixTimeSeconds(scheduledTimeUnix).DateTime;
                        var plannedNotification = new PlannedNotification
                        {
                            Title = title,
                            Body = body,
                            Date = scheduledTime
                        };

                        plannedNotifications.Add(plannedNotification);
                    }
                    return plannedNotifications;
                }else
                {
                }
            }
            return null;
        }

        // Oluşturulmuş planlı bildirimler henüz gönderilmediyse listeleme işlemi için bu bilgilerin alınması
        public async Task<List<PlannedNotification>> GetPlannedNotificationsNotSendYet()
        {
            var plannedNotifications = new List<PlannedNotification>();
            var response = await _httpClient.GetAsync("notifications?app_id=" + _appId);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(responseContent);

                var notifications = responseJson["notifications"];
                foreach (var notification in notifications)
                {   
                    var sendAfter = notification.Value<string>("send_after");

                    if (!string.IsNullOrEmpty(sendAfter))
                    {   
                        var sendAfterString = sendAfter.ToString();
                        var sendAfterUnix=long.Parse(sendAfterString);
                        var sendAfterTime = DateTimeOffset.FromUnixTimeSeconds(sendAfterUnix).DateTime;
                        if (sendAfterTime > DateTime.UtcNow)
                        {
                            var notificationId = notification.Value<string>("id");
                            var plannedNotification = new PlannedNotification
                            {   
                                NotificationId= notificationId,
                                Title = notification["headings"]["en"].Value<string>(),
                                Body = notification["contents"]["en"].Value<string>(),
                                Date = sendAfterTime
                            };

                            plannedNotifications.Add(plannedNotification);
                        }
                    }
                }
            }

            return plannedNotifications;
        }

        // Planlı bildirim henüz gönderilmediyse silme işlemi
        public async Task<Boolean> DeleteNotification(string notificationId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://onesignal.com/api/v1/notifications/{notificationId}?app_id={_appId}"),
                Headers =
                {
                    { "accept", "application/json" },
                    { "Authorization", $"Basic {_apiKey}" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(body);
                var success = jsonResponse.Value<bool>("success");    
                var notification = await notificationDAO.GetNotificationByNotificationId(notificationId);
                notification.State = false;
                await notificationDAO.UpdateNotificationState(notification);
                return success;
            }
        }

    }
}