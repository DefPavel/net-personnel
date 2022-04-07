namespace AlphaPersonel.Services;
internal static class ServiceDate
{
    // Вычесление интервала
    public static long ItervalDate(DateTime startDate, DateTime endDate)
    {
        TimeSpan span = endDate - startDate;
        return span.Ticks;
    }
    // Конвертирование Ticks 
    // Дни не совсем верные
    public static string ConvertTicksToDateTime(long ticks)
    {
        DateTime d = new(ticks);
        return string.Format("{0} г. {1} м. {2} д.", (d.Year - 1), (d.Month - 1), (d.Day - 1));
    }

    // 100% правильно
    public static string GetDate(DateTime dt1, DateTime dt2)
    {
        DateTime tmp = dt1;
        int years = 0;
        int mouths = 0;

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

        int day = dt2.Day - dt1.Day;
        if (day < 0)
        {
            mouths--;
            day = DateTime.DaysInMonth(dt1.Year, dt1.Month) + day;
        }

        return string.Format("{0} г. / {1} м. / {2} д./ ", years, mouths, day);

    }
}
