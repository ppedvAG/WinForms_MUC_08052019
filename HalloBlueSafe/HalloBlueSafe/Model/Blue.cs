using System;

namespace HalloBlueSafe.Model
{
    public class Blue
    {
        public decimal Speicher { get; set; }
        public string Titel { get; set; }
        public RegionCode RegionCode { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public enum RegionCode
    {
        All = 0,
        America = 1,
        Euro = 2,
        Asia = 3,
        SouthAmerica = 4,
        Africa = 5,
        China = 6,
        Pre = 7,
        Spacecraft = 8
    }
}
