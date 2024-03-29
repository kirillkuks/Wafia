﻿using NpgsqlTypes;

namespace WAFIA.Database.Types {

    public enum Value : long {
        VeryImportant = 4,
        Important = 3,
        Desirable = 2,
        NoMatter = 1
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
        public List<Point>? Border { get; set; }
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
