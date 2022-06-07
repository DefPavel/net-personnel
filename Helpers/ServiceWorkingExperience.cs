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
        long totalTicks = 0;

       
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsOver) continue;
            // Если стаж прирывался 
            if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, t.CreateAt);

            currentDate = t.CreateAt;
            trigger = t.IsOver;
        }
        // Если стаж не прирывался 
        if (trigger)
        {
            totalTicks += ServiceDate.ItervalDate(currentDate, date);     
        }

            // Является ли год високосным
        var last = histories.Last();
        var isLeap = DateTime.IsLeapYear(last.CreateAt.Year);
        
        return ServiceDate.ConvertTicksToDateTime(totalTicks, isLeap);

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
        long totalTicks = 0;
        var age = new List<Age>();
        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsPedagogical) continue;

            if (trigger)
            {
                age.Add(ServiceDate.GetDate(currentDate, t.CreateAt));
                //age += ServiceDate.GetDate(currentDate, t.CreateAt);
            }
            currentDate = t.CreateAt;
            trigger = t.IsPedagogical;
        }
        
        if (trigger)
        {
            //totalTicks += ServiceDate.ItervalDate(currentDate, date);    
            age.Add(ServiceDate.GetDate(currentDate, date));
        }
        
        var sumYears = age.Sum(x => x.Years); //1
        var sumMounth = age.Sum(x => x.Mounths); //12
        var sumDays = age.Sum(x => x.Days); //365

        //var last = histories.Last();
        //var isLeap = DateTime.IsLeapYear(last.CreateAt.Year);
        
        return ServiceDate.ConvertTicksToDateTime(totalTicks ,false);

    }
    // В универе
    public static string GetStageIsUniver(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsUniver) continue;
            // Если стаж прирывался 
            if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, t.CreateAt);

            currentDate = t.CreateAt;
            trigger = t.IsUniver;
        }
        if (trigger)
        {
            totalTicks += ServiceDate.ItervalDate(currentDate, date);     
        }

        var last = histories.Last();
        var isLeap = DateTime.IsLeapYear(last.CreateAt.Year);
        return ServiceDate.ConvertTicksToDateTime(totalTicks ,isLeap);

    }
    // Научный
    public static string GetStageIsScience(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsScience) continue;
            // Если стаж прирывался 
            if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, t.CreateAt);

            currentDate = t.CreateAt;
            trigger = t.IsScience;
        }
        if (trigger)
        {
            totalTicks += ServiceDate.ItervalDate(currentDate, date);     
        }

        var last = histories.Last();
        var isLeap = DateTime.IsLeapYear(last.CreateAt.Year);
        
        return ServiceDate.ConvertTicksToDateTime(totalTicks ,isLeap);

    }
    // Медицинский
    public static string GetStageIsMedical(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsMedical) continue;
            // Если стаж прирывался 
            if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, t.CreateAt);

            currentDate = t.CreateAt;
            trigger = t.IsMedical;
        }
        if (trigger)
        {
            totalTicks += ServiceDate.ItervalDate(currentDate, date);     
        }

        var last = histories.Last();
        var isLeap = DateTime.IsLeapYear(last.CreateAt.Year);
        return ServiceDate.ConvertTicksToDateTime(totalTicks,isLeap);

    }
    // Музей
    public static string GetStageIsMuseum(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsMuseum) continue;
            // Если стаж прирывался 
            if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, t.CreateAt);

            currentDate = t.CreateAt;
            trigger = t.IsMuseum;
        }
        // В случае если стаж не прирывный
        if (trigger)
        {
            totalTicks += ServiceDate.ItervalDate(currentDate, date);     
        }
        var last = histories.Last();
        var isLeap = DateTime.IsLeapYear(last.CreateAt.Year);
        return ServiceDate.ConvertTicksToDateTime(totalTicks, isLeap);

    }
    // Библиотека
    public static string GetStageIsLibrary(ObservableCollection<HistoryEmployment> histories , DateTime date)
    {
        var currentDate = DateTime.Now.Date;
        var trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        foreach (var t in histories)
        {
            if (trigger == t.IsLibrary) continue;
            // Если стаж прирывался 
            if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, t.CreateAt);

            currentDate = t.CreateAt;
            trigger = t.IsLibrary;
        }

        if (trigger)
        {
            totalTicks += ServiceDate.ItervalDate(currentDate, date);  
        }
        // В случае если стаж не прирывный
        //totalTicks += ServiceDate.ItervalDate(currentDate, date);

        var last = histories.Last();
        var isLeap = DateTime.IsLeapYear(last.CreateAt.Year);
        return ServiceDate.ConvertTicksToDateTime(totalTicks, isLeap);

    }

}

