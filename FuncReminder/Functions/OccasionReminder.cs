using System;
using FuncOccasionReminder.Occasions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FuncOccasionReminder
{
    public static class OccasionReminder
    {
        [FunctionName("OccasionReminder")]
        public static void Run([TimerTrigger("%OccasionSchedule%")] TimerInfo occasionTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"OccasionReminder function executed at: {DateTime.Now}");
                OccasionHandler OccasionProcessor = new();
                OccasionProcessor.SendOccasionAlerts();
            }
            catch (Exception ex)
            {
                log.LogError($"OccasionReminder Error: {ex.Message}");
            }
        }
    }
}
