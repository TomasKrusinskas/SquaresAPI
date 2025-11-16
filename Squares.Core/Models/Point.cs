using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Squares.Core.Models
{
    public class Point
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the squared distance between two points using NetTopologySuite Coordinate types.
        /// Uses NetTopologySuite's Coordinate class for geometric operations while maintaining precision for integer coordinates.
        /// </summary>
        public long DistanceSquared(Point other)
        {
            var coord1 = new Coordinate(X, Y);
            var coord2 = new Coordinate(other.X, other.Y);
            var dx = coord1.X - coord2.X;
            var dy = coord1.Y - coord2.Y;
            return (long)(dx * dx + dy * dy);
        }

        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Point other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}