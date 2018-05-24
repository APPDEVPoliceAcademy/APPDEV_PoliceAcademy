using System;
namespace WorkshopScheduler
{
    public enum SortingsEnum { ByDateAscending, ByDateDescending, ByTitleAscending, ByTitleDescending };

    public enum Unit { None,Nord,Soud, Oost, Westen  };

    public enum CategoryEnum { Zwolle, Apeldoorn };

    public enum WorkshopBrowserType
    {
        All,
        Reserved
    };

}
