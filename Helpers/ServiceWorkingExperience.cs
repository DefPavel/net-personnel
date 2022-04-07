using System.Collections.ObjectModel;

namespace AlphaPersonel.Helpers;
internal static class ServiceWorkingExperience
{
    // Общий стаж
    static public string GetStageIsOver(ObservableCollection<HistoryEmployment> histories)
    {
        DateTime currentDate = DateTime.Now.Date;
        bool trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        for (int i = 0; i < histories.Count; i++)
        {
            if (trigger != histories[i].IsOver)
            {
                // Если стаж прирывался 
                if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, histories[i].CreateAt);

                currentDate = histories[i].CreateAt;
                trigger = histories[i].IsOver;
            }
        }
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Нучно-Педагогический
    static public string GetStageIsPedagogical(ObservableCollection<HistoryEmployment> histories)
    {
        DateTime currentDate = DateTime.Now.Date;
        bool trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        for (int i = 0; i < histories.Count; i++)
        {
            if (trigger != histories[i].IsPedagogical)
            {
                // Если стаж прирывался 
                if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, histories[i].CreateAt);

                currentDate = histories[i].CreateAt;
                trigger = histories[i].IsPedagogical;
            }
        }
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // В универе
    static public string GetStageIsUniver(ObservableCollection<HistoryEmployment> histories)
    {
        DateTime currentDate = DateTime.Now.Date;
        bool trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        for (int i = 0; i < histories.Count; i++)
        {
            if (trigger != histories[i].IsUniver)
            {
                // Если стаж прирывался 
                if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, histories[i].CreateAt);

                currentDate = histories[i].CreateAt;
                trigger = histories[i].IsUniver;
            }
        }
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Научный
    static public string GetStageIsScience(ObservableCollection<HistoryEmployment> histories)
    {
        DateTime currentDate = DateTime.Now.Date;
        bool trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        for (int i = 0; i < histories.Count; i++)
        {
            if (trigger != histories[i].IsScience)
            {
                // Если стаж прирывался 
                if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, histories[i].CreateAt);

                currentDate = histories[i].CreateAt;
                trigger = histories[i].IsScience;
            }
        }
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Медицинский
    static public string GetStageIsMedical(ObservableCollection<HistoryEmployment> histories)
    {
        DateTime currentDate = DateTime.Now.Date;
        bool trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        for (int i = 0; i < histories.Count; i++)
        {
            if (trigger != histories[i].IsMedical)
            {
                // Если стаж прирывался 
                if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, histories[i].CreateAt);

                currentDate = histories[i].CreateAt;
                trigger = histories[i].IsMedical;
            }
        }
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Музей
    static public string GetStageIsMuseum(ObservableCollection<HistoryEmployment> histories)
    {
        DateTime currentDate = DateTime.Now.Date;
        bool trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        for (int i = 0; i < histories.Count; i++)
        {
            if (trigger != histories[i].IsMuseum)
            {
                // Если стаж прирывался 
                if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, histories[i].CreateAt);

                currentDate = histories[i].CreateAt;
                trigger = histories[i].IsMuseum;
            }
        }
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }
    // Библиотека
    static public string GetStageIsLibrary(ObservableCollection<HistoryEmployment> histories)
    {
        DateTime currentDate = DateTime.Now.Date;
        bool trigger = false;
        long totalTicks = 0;

        // Проходимся по списку стажа
        for (int i = 0; i < histories.Count; i++)
        {
            if (trigger != histories[i].IsLibrary)
            {
                // Если стаж прирывался 
                if (trigger) totalTicks += ServiceDate.ItervalDate(currentDate, histories[i].CreateAt);

                currentDate = histories[i].CreateAt;
                trigger = histories[i].IsLibrary;
            }
        }
        // В случае если стаж не прирывный
        totalTicks += ServiceDate.ItervalDate(currentDate, DateTime.Now.Date);

        return ServiceDate.ConvertTicksToDateTime(totalTicks);

    }

}

