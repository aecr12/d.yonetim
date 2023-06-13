using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diyabetik.model;
using diyabetik.web.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace diyabetik.web.Controllers
{
    public class NotificationController:Controller
    {   
        OneSignalService oneSignalService = new OneSignalService();

        // Bildirimler sayfası
        public async Task<IActionResult> Notifications()
        {
            return View();
        }
        
        // Push bildirim gönderme sayfası partial view üzerinden çağrıldı, bu partial view Invoke olarak da çağrılabilirdi
        // farklı kullanım şekillerini öğrenmek için bu şekilde kullanıldı
        public IActionResult SendNotificationPartialView()
        {
            return PartialView("~/Views/Shared/PartialViews/_SendNotificationPartialView.cshtml");
        }
        
        // Planlı bildirim göndermek için kullanılan sayfa çağrıldı
        public IActionResult PlannedNotificationsPartialView()
        {
            return PartialView("~/Views/Shared/PartialViews/_PlannedNotificationsPartialView.cshtml");
        }

        // gönderilmiş bildirimlerin görntülenmesi için kullanılan partial view
        public IActionResult SentNotificationsPartialView()
        {
            return PartialView("~/Views/Shared/PartialViews/_SentNotificationsPartialView.cshtml");
        }
        
        // Push bildirim gönderme için kullanılan formdan gelen bilgiler yakalandı
        [HttpPost]
        public async Task <IActionResult> ReceiveNotification(Notification notification)
        {
            await oneSignalService.SendNotificationToAllUsers(notification);
            return RedirectToAction("Notifications");
        }

        // Zaman ayarlı push bildirim gönderme için kullanılan formdan gelen bilgiler yakalandı
        [HttpPost]
        public async Task <IActionResult> ReceivePlannedNotification(PlannedNotification plannedNotification)
        {
            await oneSignalService.SendPlannedNotificationToAllUsers(plannedNotification);
            return RedirectToAction("Notifications");
        }

        // Zaman ayarlı bildirim henüz gönderilmeden silme işlemi
        public async Task <JsonResult> DeleteNotification(PlannedNotification plannedNotification)
        {   
            bool isDeleted = await oneSignalService.DeleteNotification(plannedNotification.NotificationId);
            await oneSignalService.DeleteNotification(plannedNotification.NotificationId);
            
            return Json(new { success = plannedNotification });
        }
    }
}