using Domain.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using ZstdSharp;

namespace BackEnd.Utilities
{
    public static class ExtensionMethods
    {
        public static string GetUserId(this ClaimsPrincipal principle)
        {
            if (principle == null)
            {
                throw new ArgumentNullException(nameof(principle));
            }

            var claim = principle.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null || claim.Value == null) throw new Exception("User Name Identifier claim or value is null");

            return claim.Value;
        }

        public static string GetUsername(this ApplicationUser user)
        {
            if (user.UserName == null) throw new Exception("Username is null");

            return user.UserName;
        }

        // TODO: Break this out to a service
        // TODO: Also make it a little more readable
        public static decimal TotalPayThisMonth(this ApplicationUser user)
        {
            if (user.PreviousPayDate == null) return 0;

            decimal monthlyAmount = 0;

            var today = DateTime.Now.Date;
            var firstOfMonth = new DateTime(today.Year, today.Month, 1);

            var year = firstOfMonth.Year;
            var month = firstOfMonth.Month + 1;

            var lastPayDay = user.PreviousPayDate.Value.Date;

            if (firstOfMonth.Month == 12)
            {
                year = firstOfMonth.Year + 1;
                month = 1;
            }

            var nextMonth = new DateTime(year, month, firstOfMonth.Day);

            // The last pay day falls after the first of the current month. Will need to look in the past for when the first paycheck will be for the month
            if (firstOfMonth < lastPayDay)
            {
                var keepGoing = true;

                while (keepGoing)
                {
                    var testDate = new DateTime(lastPayDay.Year, lastPayDay.Month, lastPayDay.Day).AddDays(-14);

                    if (testDate.Month != today.Month)
                    {
                        keepGoing = false;
                    }
                    else
                    {
                        lastPayDay = lastPayDay.AddDays(-14);
                    }
                }
            }
            else
            {
                // Last paycheck is in the past, just need to keep adding until we make it past the first of the month
                while (firstOfMonth > lastPayDay)
                {
                    lastPayDay = lastPayDay.AddDays(14);
                }
            }

            // The first paycheck of the month falls in the current month

            while (lastPayDay < nextMonth)
            {
                monthlyAmount += user.BiWeeklySalary;
                lastPayDay = lastPayDay.AddDays(14);
            }

            return monthlyAmount;
        }
    }
}
