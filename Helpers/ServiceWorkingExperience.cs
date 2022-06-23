using System.Collections.Generic;
using System.Linq;

namespace AlphaPersonel.Helpers;
internal static class ServiceWorkingExperience
{
    // Общий стаж
    public static string GetStageIsOver(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        var age = new List<Age>();
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsOver) continue;

            if (trigger)
            {
                age.Add(ServiceDate.GetDate(currentDate, t.CreateAt));
            }
            currentDate = t.CreateAt;
            trigger = t.IsOver;
        }

        if (trigger)
        {
            age.Add(ServiceDate.GetDate(currentDate, date));
        }

        var sumDate = 0;

        foreach (var item in age)
        {
            sumDate += item.Days + item.Mounths * 30 + item.Years * 12 * 30;
        }
        int resultYear = sumDate / (12 * 30);
        int resultMounth = (sumDate - resultYear * 12 * 30) / 30;
        int resultDays = sumDate - resultYear * 12 * 30 - resultMounth * 30;

        return $"{resultYear} г. {resultMounth} м. {resultDays} д.";

    }
    // Нучно-Педагогический
    public static string GetStageIsPedagogical(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsPedagogical) continue;
            // Если стаж прирывался 
            if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, t.CreateAt);

            currentDate = t.CreateAt;
            trigger = t.IsPedagogical;
        }
        
        if (trigger)
        {
            totalTicks += ServiceDate.ItervalDate(currentDate, date);     
        }

        var last = histories.Last();
        var isLeap = DateTime.IsLeapYear(last.CreateAt.Year);
        
        return ServiceDate.ConvertTicksToDateTime(totalTicks ,isLeap);

    }
    public static string NewGetStageIsPedagogical(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        var age = new List<Age>();
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsPedagogical) continue;

            if (trigger)
            {
                age.Add(ServiceDate.GetDate(currentDate, t.CreateAt));
            }
            currentDate = t.CreateAt;
            trigger = t.IsPedagogical;
        }
        
        if (trigger)
        {
            age.Add(ServiceDate.GetDate(currentDate, date));
        }

        var sumDate = 0;

        foreach (var item in age)
        {
            sumDate += item.Days + item.Mounths * 30 + item.Years * 12 * 30;
        }
        int resultYear = sumDate / (12 * 30);
        int resultMounth = (sumDate - resultYear * 12 * 30)/ 30;
        int resultDays = sumDate - resultYear * 12 * 30 - resultMounth * 30;

        return $"{resultYear} г. {resultMounth} м. {resultDays} д.";
    }
    // В универе
    public static string GetStageIsUniver(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        var age = new List<Age>();
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsUniver) continue;

            if (trigger)
            {
                age.Add(ServiceDate.GetDate(currentDate, t.CreateAt));
            }
            currentDate = t.CreateAt;
            trigger = t.IsUniver;
        }

        if (trigger)
        {
            age.Add(ServiceDate.GetDate(currentDate, date));
        }

        var sumDate = 0;

        foreach (var item in age)
        {
            sumDate += item.Days + item.Mounths * 30 + item.Years * 12 * 30;
        }
        int resultYear = sumDate / (12 * 30);
        int resultMounth = (sumDate - resultYear * 12 * 30) / 30;
        int resultDays = sumDate - resultYear * 12 * 30 - resultMounth * 30;

        return $"{resultYear} г. {resultMounth} м. {resultDays} д.";

    }
    // Научный
    public static string GetStageIsScience(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        var age = new List<Age>();
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsScience) continue;

            if (trigger)
            {
                age.Add(ServiceDate.GetDate(currentDate, t.CreateAt));
            }
            currentDate = t.CreateAt;
            trigger = t.IsScience;
        }

        if (trigger)
        {
            age.Add(ServiceDate.GetDate(currentDate, date));
        }

        var sumDate = 0;

        foreach (var item in age)
        {
            sumDate += item.Days + item.Mounths * 30 + item.Years * 12 * 30;
        }
        int resultYear = sumDate / (12 * 30);
        int resultMounth = (sumDate - resultYear * 12 * 30) / 30;
        int resultDays = sumDate - resultYear * 12 * 30 - resultMounth * 30;

        return $"{resultYear} г. {resultMounth} м. {resultDays} д.";

    }
    // Медицинский
    public static string GetStageIsMedical(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        var age = new List<Age>();
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsMedical) continue;

            if (trigger)
            {
                age.Add(ServiceDate.GetDate(currentDate, t.CreateAt));
            }
            currentDate = t.CreateAt;
            trigger = t.IsMedical;
        }

        if (trigger)
        {
            age.Add(ServiceDate.GetDate(currentDate, date));
        }

        var sumDate = 0;

        foreach (var item in age)
        {
            sumDate += item.Days + item.Mounths * 30 + item.Years * 12 * 30;
        }
        int resultYear = sumDate / (12 * 30);
        int resultMounth = (sumDate - resultYear * 12 * 30) / 30;
        int resultDays = sumDate - resultYear * 12 * 30 - resultMounth * 30;

        return $"{resultYear} г. {resultMounth} м. {resultDays} д.";

    }
    // Музей
    public static string GetStageIsMuseum(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        var age = new List<Age>();
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsMuseum) continue;

            if (trigger)
            {
                age.Add(ServiceDate.GetDate(currentDate, t.CreateAt));
            }
            currentDate = t.CreateAt;
            trigger = t.IsMuseum;
        }

        if (trigger)
        {
            age.Add(ServiceDate.GetDate(currentDate, date));
        }

        var sumDate = 0;

        foreach (var item in age)
        {
            sumDate += item.Days + item.Mounths * 30 + item.Years * 12 * 30;
        }
        int resultYear = sumDate / (12 * 30);
        int resultMounth = (sumDate - resultYear * 12 * 30) / 30;
        int resultDays = sumDate - resultYear * 12 * 30 - resultMounth * 30;

        return $"{resultYear} г. {resultMounth} м. {resultDays} д.";

    }
    // Библиотека
    public static string GetStageIsLibrary(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        var age = new List<Age>();
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsLibrary) continue;

            if (trigger)
            {
                age.Add(ServiceDate.GetDate(currentDate, t.CreateAt));
            }
            currentDate = t.CreateAt;
            trigger = t.IsLibrary;
        }

        if (trigger)
        {
            age.Add(ServiceDate.GetDate(currentDate, date));
        }

        var sumDate = 0;

        foreach (var item in age)
        {
            sumDate += item.Days + item.Mounths * 30 + item.Years * 12 * 30;
        }
        int resultYear = sumDate / (12 * 30);
        int resultMounth = (sumDate - resultYear * 12 * 30) / 30;
        int resultDays = sumDate - resultYear * 12 * 30 - resultMounth * 30;

        return $"{resultYear} г. {resultMounth} м. {resultDays} д.";

    }

}

