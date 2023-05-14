using System.Xml.Linq;
using WAFIA.Database;
using WAFIA.Database.Types;

namespace WAFIA.Loader
{
    internal static class OverpassQueryBuilder {

        private static readonly string api = "http://overpass-api.de/api/interpreter?";

        internal static string GetQueryName(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"name\"" +
                   ")];" +
                   $"area[name=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryLat(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "::lat" +
                   ")];" +
                   $"area[name=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryLon(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "::lon" +
                   ")];" +
                   $"area[name=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryPhone(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"contact:phone\"" +
                   ")];" +
                   $"area[name=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryWebsite(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"contact:website\"" +
                   ")];" +
                   $"area[name=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryOpeningHours(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "opening_hours" +
                   ")];" +
                   $"area[name=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryNumber(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"addr:housenumber\"" +
                   ")];" +
                   $"area[name=\"{city.Name}\"]->.a;" +
                   InfrElementTranslate(infrElement) +
                   "out center;";
        }

        internal static string GetQueryStreet(City city, InfrastructureElement infrElement) {
            return api +
                   "data=[out:csv(" +
                   "\"addr:street\"" +
                   ")];" +
                   $"area[name=\"{city.Name}\"]->.a;" +
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
