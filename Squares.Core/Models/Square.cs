using System;
using System.Collections.Generic;
using System.Linq;

namespace Squares.Core.Models
{
    public class Square
    {
        public IReadOnlyList<Point> Points { get; }

        public Square(IEnumerable<Point> points)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));

            var pointList = points.ToList();
            
            if (pointList.Count != 4)
                throw new ArgumentException("A square must have exactly 4 points.", nameof(points));

            if (pointList.Distinct().Count() != 4)
                throw new ArgumentException("A square must have 4 distinct points.", nameof(points));

            Points = pointList.AsReadOnly();
        }

        public Square(Point p1, Point p2, Point p3, Point p4)
            : this(new[] { p1, p2, p3, p4 })
        {
        }

        public static bool IsValidSquare(Point p1, Point p2, Point p3, Point p4)
        {
            var points = new[] { p1, p2, p3, p4 };
            
            if (points.Distinct().Count() != 4)
                return false;

            var distances = new List<long>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    distances.Add(points[i].DistanceSquared(points[j]));
                }
            }

            var grouped = distances.GroupBy(d => d).ToList();
            
            if (grouped.Count != 2)
                return false;

            var counts = grouped.Select(g => g.Count()).OrderBy(c => c).ToList();
            return counts[0] == 2 && counts[1] == 4;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Square other)
            {
                return Points.Count == other.Points.Count &&
                       Points.All(p => other.Points.Contains(p));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Points
                .OrderBy(p => p.X)
                .ThenBy(p => p.Y)
                .Aggregate(0, (hash, p) => HashCode.Combine(hash, p.GetHashCode()));
        }

        public override string ToString()
        {
            return $"Square: [{string.Join(", ", Points.Select(p => $"({p.X}, {p.Y})"))}]";
        }
    }
}
