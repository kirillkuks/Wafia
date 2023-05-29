using System.Security.Cryptography;
using WAFIA.Database.Types;

namespace WAFIA.Loader
{
    internal static class OverpassQueryBuilder {

        private static readonly string api = "http://overpass-api.de/api/interpreter?";
        internal static string GetCountryLatQuery() {
            return api +
                   "data=[out:csv(::lat)]; " +
                   $"relation[\"admin_level\"=\"2\"][\"ISO3166-1\"~\"^..$\"];" +
                   "out center;";
        }

        internal static string GetCountryLonQuery() {
            return api +
                   "data=[out:csv(::lon)]; " +
                   $"relation[\"admin_level\"=\"2\"][\"ISO3166-1\"~\"^..$\"];" +
                   "out center;";
        }

        internal static string GetCountryNameQuery() {
            return api +
                   "data=[out:csv(\"name:en\")];" +
                   $"relation[\"admin_level\"=\"2\"][\"ISO3166-1\"~\"^..$\"];" +
                   "out;";
        }

        internal static string GetCityLatQuery(Country country) {
            return api +
                   "data=[out:csv(::lat)]; " +
                   $"area[\"name:en\"=\"{country.Name}\"];" +
                   $"nwr[\"place\"=\"city\"](area);" +
                   "out center;";
        }

        internal static string GetCityLonQuery(Country country) {
            return api +
                   "data=[out:csv(::lon)]; " +
                   $"area[\"name:en\"=\"{country.Name}\"];" +
                   $"nwr[\"place\"=\"city\"](area);" +
                   "out center;";
        }

        internal static string GetCityNameQuery(Country country) {
            return api +
                   "data=[out:csv(\"name:en\")];" +
                   $"area[\"name:en\"=\"{country.Name}\"];" +
                   $"nwr[\"place\"=\"city\"](area);" +
                   "out;";
        }

        internal static string GetQueryName(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"name\"" +
                   ")];" +
                   $"area[\"name:en\"=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryLat(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "::lat" +
                   ")];" +
                   $"area[\"name:en\"=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryLon(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "::lon" +
                   ")];" +
                   $"area[\"name:en\"=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryPhone(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"contact:phone\"" +
                   ")];" +
                   $"area[\"name:en\"=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryWebsite(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"contact:website\"" +
                   ")];" +
                   $"area[\"name:en\"=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryOpeningHours(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "opening_hours" +
                   ")];" +
                   $"area[\"name:en\"=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryNumber(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"addr:housenumber\"" +
                   ")];" +
                   $"area[\"name:en\"=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryStreet(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"addr:street\"" +
                   ")];" +
                   $"area[\"name:en\"=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        private static string InfrElementTranslate(InfrastructureElement infrElement) {
            return infrElement switch {
                InfrastructureElement.Healthcare => "(" +
                                                    TagQuery("amenity", "clinic") +
                                                    TagQuery("amenity", "doctors") +
                                                    TagQuery("amenity", "hospital") +
                                                    TagQuery("building", "hospital") +
                                                    ");",
                InfrastructureElement.University => "(" +
                                                    TagQuery("amenity", "university") +
                                                    TagQuery("building", "university") +
                                                    ");",
                InfrastructureElement.PlaceOfWorship => "(" +
                                                        TagQuery("amenity", "monastery") +
                                                        TagQuery("amenity", "place_of_worship") +
                                                        TagQuery("building", "cathedral") +
                                                        TagQuery("building", "chapel") +
                                                        TagQuery("building", "church") +
                                                        TagQuery("building", "monastery") +
                                                        TagQuery("building", "mosque") +
                                                        TagQuery("building", "synagogue") +
                                                        TagQuery("building", "temple") +
                                                        TagQuery("historic", "church") +
                                                        TagQuery("historic", "monastery") +
                                                        ");",
                InfrastructureElement.School => "(" +
                                                TagQuery("amenity", "driving_school") +
                                                TagQuery("amenity", "language_school") +
                                                TagQuery("amenity", "music_school") +
                                                TagQuery("amenity", "school") +
                                                ");",
                InfrastructureElement.FireStation => "(" +
                                                     TagQuery("amenity", "fire_station") +
                                                     TagQuery("building", "fire_station") +
                                                     ");",
                InfrastructureElement.Police => "(" +
                                                TagQuery("amenity", "police") +
                                                ");",
                InfrastructureElement.Mall => "(" +
                                              TagQuery("shop", "mall") +
                                              ");",
                InfrastructureElement.Subway => "(" +
                                                TagQuery("railway", "subway") +
                                                TagQuery("railway", "subway_entrance") +
                                                ");",

                _ => "",
            };
        }

        private static string TagQuery(string tagKey, string tag) {
            return $"node(area.a)[{tagKey}={tag}];" +
                   $"way(area.a)[{tagKey}={tag}];" +
                   $"rel(area.a)[{tagKey}={tag}];";
        }
    }
}
