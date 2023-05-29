using NpgsqlTypes;

namespace WAFIA.Database.Types {

    public class Country {
        public long Id { get; set; }
        public string Name { get; set; }
        public Point Center { get; set; }
        public Country(long id, string name, Point center) {
            Id = id;
            Name = name;
            Center = center;
        }
    }
    public class City {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Country { get; set; }

        public Point Center { get; set; }
        public City(long id, string name, long country, Point center) {
            Id = id;
            Name = name;
            Country = country;
            Center = center;
        }
    }
}
