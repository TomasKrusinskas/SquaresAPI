using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Squares.Core.Models;

namespace Squares.Core.Repositories
{
    public class PointRepository : IPointRepository
    {
        private readonly ConcurrentDictionary<int, Point> _points = new();
        private int _nextId = 0;

        public IEnumerable<Point> GetAll()
        {
            return _points.Values.OrderBy(p => p.Id);
        }

        public Point? GetById(int id)
        {
            _points.TryGetValue(id, out var p);
            return p;
        }

        public Point? GetByCoordinates(int x, int y)
        {
            return _points.Values.FirstOrDefault(p => p.X == x && p.Y == y);
        }

        public Point Add(int x, int y)
        {
            var existing = GetByCoordinates(x, y);
            if (existing != null)
                return existing;

            var id = System.Threading.Interlocked.Increment(ref _nextId);
            var point = new Point(id, x, y);
            _points.TryAdd(point.Id, point);
            return point;
        }

        public bool Delete(int id)
        {
            return _points.TryRemove(id, out _);
        }

        public void Clear()
        {
            _points.Clear();
        }
    }
}
