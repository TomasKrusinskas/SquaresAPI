using System;
using System.Collections.Generic;
using System.Linq;
using NetTopologySuite.Geometries;
using Squares.Core.Models;
using Squares.Core.Repositories;
using Point = Squares.Core.Models.Point;

namespace Squares.Core.Services
{
    /// <summary>
    /// Square detection service.
    /// Uses the "same midpoint + same diagonal length" algorithm (a well-known computational geometry approach):
    /// For every pair of points (p1,p2) treat them as a potential diagonal of a square.
    /// Pairs that have same midpoint and same squared diagonal length belong to the same square(s).
    /// Combining two such pairs that are disjoint yields a square.
    /// 
    /// Uses NetTopologySuite library for geometric validation to ensure detected quadrilaterals are actual squares.
    /// </summary>
    public class SquareService : ISquareService
    {
        private readonly IPointRepository _repo;

        public SquareService(IPointRepository repo)
        {
            _repo = repo;
        }

        public void ImportPoints(IEnumerable<(int x, int y)> points)
        {
            _repo.Clear();

            foreach (var (x, y) in points)
            {
                _repo.Add(x, y);
            }
        }

        public Point AddPoint(int x, int y)
        {
            return _repo.Add(x, y);
        }

        public bool DeletePoint(int id)
        {
            return _repo.Delete(id);
        }

        public IEnumerable<Point> GetPoints()
        {
            return _repo.GetAll();
        }

        public IEnumerable<IEnumerable<Point>> GetSquares()
        {
            var pts = _repo.GetAll().ToList();

            if (pts.Count < 4)
                return Array.Empty<IEnumerable<Point>>();

            var dict = new Dictionary<(long cx2, long cy2, long distSq), List<(Point a, Point b)>>();

            for (int i = 0; i < pts.Count; i++)
            {
                for (int j = i + 1; j < pts.Count; j++)
                {
                    var a = pts[i];
                    var b = pts[j];

                    long cx2 = (long)a.X + b.X;
                    long cy2 = (long)a.Y + b.Y;
                    long distSq = a.DistanceSquared(b);

                    var key = (cx2, cy2, distSq);
                    if (!dict.TryGetValue(key, out var list))
                    {
                        list = new List<(Point a, Point b)>();
                        dict[key] = list;
                    }

                    list.Add((a, b));
                }
            }

            var results = new HashSet<string>();
            var squares = new List<List<Point>>();

            foreach (var kvp in dict)
            {
                var pairList = kvp.Value;
                if (pairList.Count < 2)
                    continue;

                for (int i = 0; i < pairList.Count; i++)
                {
                    for (int j = i + 1; j < pairList.Count; j++)
                    {
                        var p1 = pairList[i];
                        var p2 = pairList[j];

                        var ids = new[] { p1.a.Id, p1.b.Id, p2.a.Id, p2.b.Id };
                        if (ids.Distinct().Count() != 4)
                            continue;

                        var points = new[] { p1.a, p1.b, p2.a, p2.b };

                        if (!IsValidSquareUsingNTS(points))
                            continue;

                        var coordKey = string.Join("|", points
                            .Select(p => $"{p.X}:{p.Y}")
                            .OrderBy(s => s, StringComparer.Ordinal));

                        if (results.Add(coordKey))
                        {
                            squares.Add(points.OrderBy(p => p.Id).ToList());
                        }
                    }
                }
            }

            return squares;
        }

        public int CountSquares()
        {
            return GetSquares().Count();
        }

        /// <summary>
        /// Validates that 4 points form a square, uses geometric validation algorithms from NetTopologySuite.
        /// </summary>
        private static bool IsValidSquareUsingNTS(Point[] points)
        {
            if (points.Length != 4)
                return false;

            try
            {
                var centroidX = points.Average(p => p.X);
                var centroidY = points.Average(p => p.Y);

                var orderedPoints = points
                    .OrderBy(p => Math.Atan2(p.Y - centroidY, p.X - centroidX))
                    .ToArray();

                var coordinates = new Coordinate[5];
                for (int i = 0; i < 4; i++)
                {
                    coordinates[i] = new Coordinate(orderedPoints[i].X, orderedPoints[i].Y);
                }
                coordinates[4] = coordinates[0];

                var linearRing = new LinearRing(coordinates);
                var polygon = new Polygon(linearRing);

                if (!polygon.IsValid)
                    return false;

                var vertices = polygon.Coordinates.Take(4).ToArray();
                if (vertices.Length != 4)
                    return false;

                var sideLengths = new double[4];
                for (int i = 0; i < 4; i++)
                {
                    var next = (i + 1) % 4;
                    sideLengths[i] = vertices[i].Distance(vertices[next]);
                }

                const double tolerance = 0.001;
                var firstSide = sideLengths[0];
                if (sideLengths.Any(s => Math.Abs(s - firstSide) > tolerance))
                    return false;

                for (int i = 0; i < 4; i++)
                {
                    var prev = (i + 3) % 4;
                    var next = (i + 1) % 4;

                    var v1 = new Coordinate(vertices[i].X - vertices[prev].X, vertices[i].Y - vertices[prev].Y);
                    var v2 = new Coordinate(vertices[next].X - vertices[i].X, vertices[next].Y - vertices[i].Y);

                    var dotProduct = v1.X * v2.X + v1.Y * v2.Y;
                    if (Math.Abs(dotProduct) > tolerance * firstSide * firstSide)
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
