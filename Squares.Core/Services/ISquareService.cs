using System.Collections.Generic;
using Squares.Core.Models;

namespace Squares.Core.Services
{
    public interface ISquareService
    {
        void ImportPoints(IEnumerable<(int x, int y)> points);
        Point AddPoint(int x, int y);
        bool DeletePoint(int id);
        IEnumerable<Point> GetPoints();
        IEnumerable<IEnumerable<Point>> GetSquares();
        int CountSquares();
    }
}
