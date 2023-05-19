using NpgsqlTypes;

namespace WAFIA.Database.Types {

    using Point = NpgsqlPoint;
    public enum InfrastructureElement : long {
        Healthcare = 56,
        University = 57,
        PlaceOfWorship = 58,
        School = 52,
        FireStation = 51,
        Police = 53,
        Mall = 54,
        Subway = 55
    }
    public class InfrastructureObject {
        public long Id { get; set; } = 0;
        public string Name { get; set; }
        public long City { get; set; }
        public string? House { get; set; }
        public string? Street { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? OpeningHours { get; set; }
        public Point Coord { get; set; }
        public InfrastructureElement InfrElem { get; set; }

        public InfrastructureObject(long id, Point coord, InfrastructureElement infrElem, string name, long city) { 
            Id = id;
            Coord = coord;
            InfrElem = infrElem;
            Name = name;
            City = city;
        }
    }
}
