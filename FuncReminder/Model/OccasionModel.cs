using FuncOccasionReminder.Utils;
using System;

namespace FuncOccasionReminder.Model
{
    internal class OccasionModel
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Occasion { get; set; }
        public PersonType Class { get; set; }
        public DateTime DOB
        {
            get
            {
                if (Date.Year == 2000)
                    return new DateTime(DateTime.Today.Year, Date.Month, Date.Day);
                else
                    return Date;
            }
        }
    }
}
