using System.Diagnostics.Contracts;

namespace AlphaPersonel.Helpers;
internal static class ServiceDate
{
    // Вычесление интервала
    public static long ItervalDate(DateTime startDate, DateTime endDate)
    {
        var diff = endDate - startDate;

        //var diff = DateToTicks(endDate.Year , endDate.Month , endDate.Day) - DateToTicks(startDate.Year , startDate.Month , startDate.Day);
        return diff.Ticks;
    }

    #region Новый метод расчета

    private static readonly int[] DaysToMonth365 = {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365};
    private static readonly int[] DaysToMonth366 = {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366};

    private const long TicksPerMillisecond = 10000;
    private const long TicksPerSecond = TicksPerMillisecond * 1000;
    private const long TicksPerMinute = TicksPerSecond * 60;
    private const long TicksPerHour = TicksPerMinute * 60;
    private const long TicksPerDay = TicksPerHour * 24;

    public static bool IsLeapYear(int year)
    {
        if (year < 1 || year > 9999)
        {
            throw new ArgumentOutOfRangeException();
        }
        Contract.EndContractBlock();
        return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
    }
    private static long DateToTicks(int year, int month, int day)
    {
        if (year >= 1 && year <= 9999 && month >= 1 && month <= 12)
        {
            int[] days = IsLeapYear(year) ? DaysToMonth366 : DaysToMonth365;
            if (day >= 1 && day <= days[month] - days[month - 1])
            {
                int y = year - 1;
                int n = y * 365 + y / 4 - y / 100 + y / 400 + days[month - 1] + day - 1;
                return n * TicksPerDay;
            }
        }
        throw new ArgumentOutOfRangeException();
    }

    #endregion

    // Конвертирование Ticks 
    public static string ConvertTicksToDateTime(long ticks, bool isLeap)
    {
        DateTime d = new(ticks);

        var day = d.Day;
        var month = d.Month;
        var year = d.Year;

        if(isLeap)
        {

            /*if (day == 30)
            {
                month += 1;
                day = 1;
            }
            else
            {
                month -= 1;
                day -= 2;
            }*/
            return $"{(year - 1)} г. {(month -1)} м. {((day - 1 < 0 ? 0 : day - 1))} д.";
        }
        else
        {
           /* if (month < 0)
            {
                month += 12;
                year -= 1;
            }
            if (day < 0)
            {
                day += 30;
                month -= 1;
            }
           */
            /*if (day == 30)
            {
                month += 1;
                day = 1;
            }
            else
            {
                month -= 1;
                day -= 1;
            }*/
            return $"{(year - 1)} г. {(month - 1)} м. {(day -2 < 0 ? 0 : day - 2)} д.";
        }

    }

    // 100% правильно
    public static Age GetDate(DateTime dt1, DateTime dt2)
    {
        var tmp = dt1;
        var years = 0;
        var mouths = 0;

        while (tmp < dt2)
        {
            years++;
            tmp = tmp.AddYears(1);
        }
        years--;
        tmp = dt1.AddYears(years);
        while (tmp < dt2)
        {
            mouths++;
            tmp = tmp.AddMonths(1);
        }
        if (dt1.Day < dt2.Day)
        {
            mouths--;
        }

        var day = dt2.Day - dt1.Day;
        if (day >= 0) return new Age
        {
            Years = years,
            Mounths = mouths,
            Days = day
        };
        mouths--;
        day = DateTime.DaysInMonth(dt1.Year, dt1.Month) + day;

        return new Age
        {
            Years = years,
            Mounths = mouths,
            Days = day
        };
    }
}
