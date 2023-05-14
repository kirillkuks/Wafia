using NpgsqlTypes;

namespace WAFIA.Database {

    using Polygon = NpgsqlPolygon;

    public enum Value : long {
        Important = 1,
        VeryImportant = 2,
        Desirable = 3,
        NoMatter = 4
    }

    public class Parameter {
        public InfrastructureElement InfrElement { get; set; }
        public Value Value { get; set; }
        public Parameter(InfrastructureElement infrElement, Value value) {
            InfrElement = infrElement;
            Value = value;
        }
    }
    public class Request {
        public long Id { get; set; }
        public long Account { get; set; }
        public Polygon? Border { get; set; }
        public List<Parameter> Parameters { get; set; }

        public long Country { get; set; }
        public long? City { get; set; }
        public DateTime Date { get; set; }
        public Request(long id, long account, DateTime date, long country, List<Parameter> parameters) {
            Id = id;
            Account = account;
            Date = date;
            Country = country;
            Parameters = parameters;
        }
    }
}
