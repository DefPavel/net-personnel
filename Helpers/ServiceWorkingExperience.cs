using System.Collections.ObjectModel;

namespace AlphaPersonel.Helpers;
internal static class ServiceWorkingExperience
{
    // Общий стаж
    public static string GetStageIsOver(ObservableCollection<HistoryEmployment> histories)
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
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Нучно-Педагогический
    public static string GetStageIsPedagogical(ObservableCollection<HistoryEmployment> histories)
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
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // В универе
    public static string GetStageIsUniver(ObservableCollection<HistoryEmployment> histories)
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
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Научный
    public static string GetStageIsScience(ObservableCollection<HistoryEmployment> histories)
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
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Медицинский
    public static string GetStageIsMedical(ObservableCollection<HistoryEmployment> histories)
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
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Музей
    public static string GetStageIsMuseum(ObservableCollection<HistoryEmployment> histories)
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
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Библиотека
    public static string GetStageIsLibrary(ObservableCollection<HistoryEmployment> histories)
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
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }

}

