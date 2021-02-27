using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2
{
    /// <summary>
    /// stores information about a single worker
    /// </summary>
    class Worker : IComparable<Worker>
    {
        /// <summary>
        /// full name of worker
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// profession of worker
        /// </summary>
        public string Profession { get; set; }
        /// <summary>
        /// experience of worker in years
        /// </summary>
        public int Experience { get; set; }
        /// <summary>
        /// daily requested hours of worker
        /// </summary>
        public int HoursDaily { get; set; }
        /// <summary>
        /// monthly requested hours of worker
        /// </summary>
        public int HoursMonthly { get; set; }
        /// <summary>
        /// requested montly pay of worker
        /// </summary>
        public int RequestedPay { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">full name of worker</param>
        /// <param name="prof">profession of worker</param>
        /// <param name="exp">years of experienvce of worker</param>
        /// <param name="hoursD">requested daily hours of worker</param>
        /// <param name="hoursM">requested monthly hours of worker</param>
        /// <param name="pay">requested pay of worker</param>
        public Worker(string name, string prof, int exp, int hoursD,
                      int hoursM, int pay)
        {
            FullName = name;
            Profession = prof;
            Experience = exp;
            HoursDaily = hoursD;
            HoursMonthly = hoursM;
            RequestedPay = pay;
        }
        public override string ToString()
        {
            return string.Format("|{0,-19}|{1,-17}|{2,10}|{3,3}|{4,5}|{5,10}|",
                                 FullName, Profession, Experience, HoursDaily,
                                 HoursMonthly, RequestedPay);
        }
        public string ToStringCSV()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}",
                                 FullName, Profession, Experience, HoursDaily,
                                 HoursMonthly, RequestedPay);
        }
        /// <summary>
        /// used for sorting
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool Equals(Worker B)
        {
            return (this.Experience == B.Experience && 
                    this.Profession == B.Profession);
        }
        /// <summary>
        /// checks if the worker is already in the list
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public bool Exists(Worker B)
        {
            return ((this.FullName == B.FullName &&
                    this.Profession == B.Profession));
        }
        /// <summary>
        /// Worker A comes after B while sorting
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        static public bool operator >(Worker A, Worker B)
        {
            return ((A.Experience < B.Experience) || (A.Experience == B.Experience
                     && string.Compare(A.Profession, B.Profession) == 1));
        }
        /// <summary>
        /// Worker A comes before B while sorting
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        static public bool operator <(Worker A, Worker B)
        {
            return ((A.Experience > B.Experience) || (A.Experience == B.Experience
                     && string.Compare(A.Profession, B.Profession) == -1));
        }
        /// <summary>
        /// Compares workers (this and other) by experience (increasing) and 
        /// by profession alphabetically (decreasing)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Worker other)
        {
            if (((this.Experience < other.Experience) || 
                 (this.Experience == other.Experience
                  && string.Compare(this.Profession, other.Profession) == 1)))
                return 1;
            if (((this.Experience > other.Experience) || 
                 (this.Experience == other.Experience
                  && string.Compare(this.Profession, other.Profession) == -1)))
                return -1;
            else
                return 0;
        }
    }
}
