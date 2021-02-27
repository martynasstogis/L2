using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L1
{
    /// <summary>
    /// stores information about a single worker
    /// </summary>
    class Workers
    {
        public string fullName { get; set; } //full name of worker
        public string profession { get; set; } //profession of worker
        public int experience { get; set; } //experience of worker in years
        public int hoursDaily { get; set; } //daily requested hours of worker
        public int hoursMonthly { get; set; } //monthly requested hours of worker
        public int requestedPay { get; set; } //requested montly pay of worker in euros
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">full name of worker</param>
        /// <param name="prof">profession of worker</param>
        /// <param name="exp">years of experienvce of worker</param>
        /// <param name="hoursD">requested daily hours of worker</param>
        /// <param name="hoursM">requested monthly hours of worker</param>
        /// <param name="pay">requested pay of worker</param>
        public Workers(string name, string prof, int exp, int hoursD, int hoursM, int pay)
        {

        }
    }
}
