using WAFIA.Database.Types;

namespace WAFIA.Algorithms {
    public class PriorityPoint {
        public Point Point { get; set; }
        public float Priority { get; set; }
        public PriorityPoint(Point point, float priority) {
            Point = point;
            Priority = priority;
        }
    }
}

    