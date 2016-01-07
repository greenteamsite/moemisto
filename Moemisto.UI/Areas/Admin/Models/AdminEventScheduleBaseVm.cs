using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using Moemisto.UI.Models;

namespace Moemisto.UI.Areas.Admin.Models
{
    public class AdminEventScheduleBaseVm
    {
        public int EventId { get; set; }

        public int EventScheduleId { get; set; }

        public string EventTitle { get; set; }
        /// <summary>
        /// Список дат та часу проведення івента в заданому місці Place
        /// </summary>
        public List<DateTime> StartEventList { get; set; }
        public int PlaceId { get; set; }
        public PlaceBaseVm Place { get; set; }
        /// <summary>
        /// Ціна з (грн.)
        /// </summary>
        [Display(Name = "Вартість з")]
        public int PriceFrom { get; set; }
        /// <summary>
        /// Ціна по (грн.)
        /// </summary>
        [Display(Name = "Вартість по")]
        public int PriceTo { get; set; }

        /// <summary>
        /// For inputs and save states
        /// </summary>
        public string AddDatePeriodFromInput { get; set; }

        public string AddDatePeriodToInput { get; set; }

        public string AddTimesPeriodInput { get; set; }

        public bool EnableTimePeriod { get; set; }

        public string GetStartEventsStr
        {
            get
            {
                if (StartEventList != null && StartEventList.Count > 0)
                {
                    string res = StartEventList.Aggregate(String.Empty,
                        (current, datetime) => String.Format("{0} {1}, ", current, datetime.ToString("dd.MM HH:mm")));

                    return res.Trim();
                }
                return string.Empty;
            }
        }
    }
}