using System;
namespace WorkshopScheduler
{
    public enum SortingsEnum { DatumStijgend, DatumDalend, TitelStijgend, TitelDalend };

    public enum Unit { None,Noord,Zuid, Oost, West  };

    public enum CategoryEnum { Zwolle, Apeldoorn };

    public enum WorkshopBrowserType
    {
        All,
        Reserved
    };

}
