using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurrLife
{
    
    interface ITimeCalculate
    {
        public string timeCalculate();
    }

    public class LeftForNextDay : ITimeCalculate
    {
        public string timeCalculate()
        {
            DateTime n = System.DateTime.Now;
            DateTime next = NextDay(n);
            string result = (next - n).ToString(@"hh\:mm\:ss");
            return result;
        }
        private DateTime NextDay(DateTime now)
        {
            DateTime next = now.AddDays(1);
            next = new DateTime(next.Year, next.Month, next.Day, 0, 0, 0);
            return next;
        }
    }

    class LeftForNextMonth : ITimeCalculate
    {
        public string timeCalculate()
        {
            DateTime now = DateTime.Now;
            DateTime next = NextMonth(now);
            string result = (next-now).ToString(@"dd\:hh\:mm\:ss");
            return result;
        }
        private DateTime NextMonth(DateTime now)
        {
            DateTime next = now.AddMonths(1);
            next = new DateTime(next.Year, next.Month, 1, 0, 0, 0);
            return next;

        }

    }

    class LeftForNextYear : ITimeCalculate
    {
        public string timeCalculate()
        {
            DateTime n = DateTime.Now;
            DateTime next = n.AddYears(1);
            next = new DateTime(next.Year, 1, 1, 0, 0, 0);
            string result = (next - n).ToString(@"dd\:hh\:mm\:ss");
            return result;
        }
    }
    class LeftForConcreteDay : ITimeCalculate
    {
        private DateTime current;
        
        public LeftForConcreteDay(DateTime current)
        {
            this.current = current;
        }
        public void setConcrete(DateTime current)
        {
            this.current = current;
        }
        public string timeCalculate()
        {
            if (current == null) throw new Exception("WRONG");
            if (current < DateTime.Now) 
                throw new ArgumentException("CurrentDate < DateTime.NOW");

            current = new DateTime(current.Year, current.Month, current.Day, 0, 0, 0);
            var today = DateTime.Now;
            string result = (current-today).ToString(@"dd\:hh\:mm\:ss");
            return result;

        }
    }


}