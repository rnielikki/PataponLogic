namespace PataRoad.Core.Map.Weather
{
    //the order "which is superior"
    [System.Flags]
    public enum WindType
    {
        None = 0,
        Changing = 1,
        HeadWind = 2,
        TailWind = 4
    }
}
