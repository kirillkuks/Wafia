using NpgsqlTypes;
using WAFIA.Database.Types;

namespace WAFIA.Algorithms {
    public static class GeoAlgorithms {
        private const double deltaNet = 0.001d;
        private static readonly Dictionary<Value, double> valueDistMap = new() {
        { Value.NoMatter, double.PositiveInfinity },
        { Value.Desirable, deltaNet * 3 },
        { Value.Important, deltaNet * 2 },
        { Value.VeryImportant, deltaNet }
        };

        public static List<PriorityPoint> FindZones(Request req, List<InfrastructureObject> objs, out List<InfrastructureObject> outObjs)
        {
            List<PriorityPoint> zones = new();
            outObjs = new();

            if (req.Border == null || req.Border.Count == 0) {
                foreach (var obj in objs) {
                    foreach (var param in req.Parameters) {
                        if (obj.InfrElem == param.InfrElement
                            && (param.Value == Value.VeryImportant || param.Value == Value.Important)) {
                            outObjs.Add(obj);
                            zones.Add(new(obj.Coord, 1.0f));
                        }
                    }
                }
            } 
            else {
                Point botLeftPoint = objs[0].Coord;
                Point topRightPoint = objs[0].Coord;

                for (int i = 1; i < objs.Count; ++i) {
                    if (objs[i].Coord.X > topRightPoint.X) {
                        topRightPoint.X = objs[i].Coord.X;
                    }
                    if (objs[i].Coord.X < botLeftPoint.X) {
                        botLeftPoint.X = objs[i].Coord.X;
                    }
                    if (objs[i].Coord.Y > topRightPoint.Y) {
                        topRightPoint.Y = objs[i].Coord.Y;
                    }
                    if (objs[i].Coord.Y < botLeftPoint.Y) {
                        botLeftPoint.Y = objs[i].Coord.Y;
                    }

                    outObjs.Add(objs[i]);
                }

                topRightPoint.X = Math.Round(topRightPoint.X, 3, MidpointRounding.ToPositiveInfinity); //
                topRightPoint.Y = Math.Round(topRightPoint.Y, 3, MidpointRounding.ToPositiveInfinity); //
                botLeftPoint.X = Math.Round(botLeftPoint.X, 3, MidpointRounding.ToNegativeInfinity); //
                botLeftPoint.Y = Math.Round(botLeftPoint.Y, 3, MidpointRounding.ToNegativeInfinity); //

                double eps = 0.00001;
                Point point = botLeftPoint;

                zones.Add(new(point, GetPriority(point, objs, req.Parameters)));
                
                do {
                    if (Math.Abs(point.X - topRightPoint.X) < eps) {
                        point.X = botLeftPoint.X;
                        point.Y += deltaNet;
                    }
                    zones.Add(new(point, GetPriority(point, objs, req.Parameters)));
                } while (Math.Abs(point.Y - topRightPoint.Y) > eps);
            }
            return zones;
        }
        
        private static float GetPriority(Point point, List<InfrastructureObject> objs, List<Parameter> parameters) {
            Dictionary<InfrastructureElement, double> elementsDist = new() {
                { InfrastructureElement.Healthcare, double.PositiveInfinity },
                { InfrastructureElement.PlaceOfWorship, double.PositiveInfinity },
                { InfrastructureElement.University, double.PositiveInfinity },
                { InfrastructureElement.Subway, double.PositiveInfinity },
                { InfrastructureElement.School, double.PositiveInfinity },
                { InfrastructureElement.FireStation, double.PositiveInfinity },
                { InfrastructureElement.Police, double.PositiveInfinity },
                { InfrastructureElement.Mall, double.PositiveInfinity }
            };

            foreach (var obj in objs) {
                double xDist = Math.Abs(obj.Coord.X - point.X);
                double yDist = Math.Abs(obj.Coord.Y - point.Y);
                double dist = Math.Sqrt(xDist * xDist + yDist * yDist);
                if (elementsDist[obj.InfrElem] < dist) {
                    elementsDist[obj.InfrElem] = dist;
                }
            }

            float priority = 0.0f;
            foreach(var param in parameters) {
                if (valueDistMap[param.Value] > elementsDist[param.InfrElement]) {
                    priority += 1.0f;
                }
            }
            priority /= parameters.Count;
            return priority;
        }        
    }
}