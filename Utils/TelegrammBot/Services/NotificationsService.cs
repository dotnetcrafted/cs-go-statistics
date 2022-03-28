using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TelegramBot.Enums;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public interface INotificationService
    {
        public Task<IEnumerable<Notification>> GetAllBeforeGameNotifications();
        public Task<IEnumerable<Notification>> GetAllAfterGameNotifications();
        public Task<IEnumerable<Sticker>> GetAllStickers();
    }
    public class NotificationsService : BaseRequestService, INotificationService
    {
        public async Task<IEnumerable<Notification>> GetAllBeforeGameNotifications()
        {
            var notificationsRequest = await GetAsync<List<Notification>>(Settings.NotificationsApiUrl);

            return notificationsRequest.Code != HttpStatusCode.OK
                ? new List<Notification>()
                : notificationsRequest.Content.Where(x => x.Type.TypeId == (int)NotificationTypes.BeforeGame);
        }

        public async Task<IEnumerable<Notification>> GetAllAfterGameNotifications()
        {
            var notificationsRequest = await GetAsync<List<Notification>>(Settings.NotificationsApiUrl);

            return notificationsRequest.Code != HttpStatusCode.OK 
                ? new List<Notification>() 
                : notificationsRequest.Content.Where(x => x.Type.TypeId == (int)NotificationTypes.AfterGame);
        }

        public async Task<IEnumerable<Sticker>> GetAllStickers()
        {
            var stickersRequest = await GetAsync<List<Sticker>>(Settings.StickersApiUrl);
            return stickersRequest.Code != HttpStatusCode.OK
                ? new List<Sticker>()
                : stickersRequest.Content;
        }
    }
}