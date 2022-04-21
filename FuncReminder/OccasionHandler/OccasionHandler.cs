using FuncOccasionReminder.Model;
using FuncOccasionReminder.Utils;
using FuncReminder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuncOccasionReminder.Occasions
{
    internal class OccasionHandler
    {
        private readonly OccasionRepoHandler _OccasionRepo;
        private readonly List<OccasionModel> _occasions;
        private readonly IBC.CoreLib.TelegramLib.TelegramOpenApiClient _telegramClient;

        public OccasionHandler()
        {
            _OccasionRepo = new OccasionRepoHandler();
            _occasions = _OccasionRepo?.Occasions;
            _telegramClient = new IBC.CoreLib.TelegramLib.TelegramOpenApiClient();
        }
        public List<OccasionModel> GetOccasionsForThisMonth()
        {
            DateTime today = System.DateTime.Today;
            var OccasionsThisMonth = _occasions.Where(x => x.DOB.Month == today.Month).ToList();
            return OccasionsThisMonth;
        }

        public static string OccasionMonthlyMessageFormat(List<OccasionModel> occasions)
        {
            var messgage = occasions.Select(x => $"{x.Name}'s {x.Occasion.ToFirstUpper()}: {x.Date:dd-MMM}");
            string strOccasions = string.Join(",\r\n", messgage);
            return $"{occasions[0].Class} Occasions this {DateTime.Today:MMMM-yy}\r\n\r\n{strOccasions}";
        }

        public static string OccasionReminderMessageFormat(List<OccasionModel> occasions)
        {
            var messgage = occasions.Select(x => $"{x.Name}'s {x.Occasion.ToFirstUpper()} ({x.Date:dd-MMM})");
            string strOccasions = string.Join(",\r\n", messgage);
            return $"Hey, Today its \r\n {strOccasions}";
        }

        public bool SendMonthlyReminder()
        {
            try
            {
                var occasions = GetOccasionsForThisMonth();
                var family = occasions.Where(x => x.Class == PersonType.Family).ToList();
                var friends = occasions.Where(x => x.Class == PersonType.Friend).ToList();

                {
                    var family_message = OccasionMonthlyMessageFormat(family);
                    _telegramClient.SendMessage(AppConstants.FAMILY_CHATID, family_message);
                }

                if (family?.Count > 0)
                {
                    var friends_message = OccasionMonthlyMessageFormat(friends);
                    _telegramClient.SendMessage(AppConstants.FRIENDS_CHATID, friends_message);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendBirthdayReminder()
        {
            try
            {
                var occasionsToday = _occasions.Where(x => x.DOB == DateTime.Today).ToList();
                var family = occasionsToday.Where(x => x.Class == PersonType.Family).ToList();
                var friends = occasionsToday.Where(x => x.Class == PersonType.Friend).ToList();

                if (family?.Count > 0)
                {
                    var family_message = OccasionReminderMessageFormat(family);
                    _telegramClient.SendMessage(AppConstants.FAMILY_CHATID, family_message);
                }

                if (friends?.Count > 0)
                {
                    var friends_message = OccasionReminderMessageFormat(friends);
                    _telegramClient.SendMessage(AppConstants.FRIENDS_CHATID, friends_message);
                }

                ////DELETE LATER - FOR REFERENCE ONLY
                //if(family?.Count == 0 && friends?.Count == 0)
                //    _telegramClient.SendMessage(AppConstants.FAMILY_CHATID, "Hello from Cloud");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void SendOccasionAlerts()
        {
            if (System.DateTime.Today.Day == 1)
            {
                //Its First Day of the Month - Send Monthly Reminder
                SendMonthlyReminder();
            }

            //Send Daily Occasion Notification
            SendBirthdayReminder();
        }
    }
}
