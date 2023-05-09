namespace WAFIA.Database {

    public class Country {
        public long Id { get; set; }
        public string Name { get; set; }
        public Country(long id, string name) {
            Id = id;
            Name = name;
        }
    }
    public class City {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Country { get; set; }
        public City(long id, string name, long country) {
            Id = id;
            Name = name;
            Country = country;
        }
    }
}
