using System.Collections.Generic;
using Squares.Core.Models;

namespace Squares.Core.Repositories
{
    public interface IPointRepository
    {
        IEnumerable<Point> GetAll();
        Point? GetById(int id);
        Point? GetByCoordinates(int x, int y);
        Point Add(int x, int y);
        bool Delete(int id);
        void Clear();
    }
}
