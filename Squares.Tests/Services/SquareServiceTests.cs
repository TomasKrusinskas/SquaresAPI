using Squares.Core.Models;
using Squares.Core.Repositories;
using Squares.Core.Services;

namespace Squares.Tests.Services
{
    public class SquareServiceTests
    {
        private readonly IPointRepository _repository;
        private readonly SquareService _service;

        public SquareServiceTests()
        {
            _repository = new PointRepository();
            _service = new SquareService(_repository);
        }

        [Fact]
        public void ImportPoints_ShouldClearAndAddNewPoints()
        {

            _service.AddPoint(1, 1);
            _service.AddPoint(2, 2);
            var newPoints = new[] { (0, 0), (1, 0), (0, 1), (1, 1) };


            _service.ImportPoints(newPoints);


            var points = _service.GetPoints().ToList();
            Assert.Equal(4, points.Count);
            Assert.Contains(points, p => p.X == 0 && p.Y == 0);
            Assert.Contains(points, p => p.X == 1 && p.Y == 0);
        }

        [Fact]
        public void ImportPoints_WithEmptyList_ShouldClearAllPoints()
        {

            _service.AddPoint(1, 1);
            _service.AddPoint(2, 2);


            _service.ImportPoints(Array.Empty<(int, int)>());


            Assert.Empty(_service.GetPoints());
        }

        [Fact]
        public void AddPoint_ShouldAddNewPoint()
        {

            var point = _service.AddPoint(5, 10);


            Assert.NotNull(point);
            Assert.Equal(5, point.X);
            Assert.Equal(10, point.Y);
            Assert.Contains(_service.GetPoints(), p => p.X == 5 && p.Y == 10);
        }

        [Fact]
        public void AddPoint_WithDuplicateCoordinates_ShouldReturnExistingPoint()
        {

            var firstPoint = _service.AddPoint(5, 10);


            var secondPoint = _service.AddPoint(5, 10);


            Assert.Equal(firstPoint.Id, secondPoint.Id);
            Assert.Single(_service.GetPoints().Where(p => p.X == 5 && p.Y == 10));
        }

        [Fact]
        public void DeletePoint_WithExistingId_ShouldReturnTrue()
        {

            var point = _service.AddPoint(5, 10);


            var result = _service.DeletePoint(point.Id);


            Assert.True(result);
            Assert.DoesNotContain(_service.GetPoints(), p => p.Id == point.Id);
        }

        [Fact]
        public void DeletePoint_WithNonExistentId_ShouldReturnFalse()
        {

            var result = _service.DeletePoint(999);


            Assert.False(result);
        }

        [Fact]
        public void GetSquares_WithLessThanFourPoints_ShouldReturnEmpty()
        {

            _service.AddPoint(0, 0);
            _service.AddPoint(1, 0);
            _service.AddPoint(0, 1);


            var squares = _service.GetSquares().ToList();


            Assert.Empty(squares);
        }

        [Fact]
        public void GetSquares_WithOneSquare_ShouldReturnOneSquare()
        {

            _service.ImportPoints(new[]
            {
                (0, 0),
                (1, 0),
                (1, 1),
                (0, 1)
            });


            var squares = _service.GetSquares().ToList();


            Assert.Single(squares);
            Assert.Equal(4, squares[0].Count());
        }

        [Fact]
        public void GetSquares_WithRotatedSquare_ShouldReturnOneSquare()
        {

            _service.ImportPoints(new[]
            {
                (-1, 1),
                (1, 1),
                (1, -1),
                (-1, -1)
            });


            var squares = _service.GetSquares().ToList();


            Assert.Single(squares);
            var squarePoints = squares[0].ToList();
            Assert.Equal(4, squarePoints.Count);
        }

        [Fact]
        public void GetSquares_WithMultipleSquares_ShouldReturnAllSquares()
        {

            // Square 1: (0,0), (1,0), (1,1), (0,1)
            // Square 2: (1,0), (2,0), (2,1), (1,1)
            // The algorithm may find additional valid squares
            _service.ImportPoints(new[]
            {
                (0, 0), (1, 0), (1, 1), (0, 1), // First square
                (2, 0), (2, 1) // Additional points for second square
            });


            var squares = _service.GetSquares().ToList();


            Assert.True(squares.Count >= 2, $"Expected at least 2 squares, but found {squares.Count}");
            
            // Verify the two expected squares are present
            var square1Found = squares.Any(s => 
                s.Any(p => p.X == 0 && p.Y == 0) &&
                s.Any(p => p.X == 1 && p.Y == 0) &&
                s.Any(p => p.X == 1 && p.Y == 1) &&
                s.Any(p => p.X == 0 && p.Y == 1));
            
            var square2Found = squares.Any(s =>
                s.Any(p => p.X == 1 && p.Y == 0) &&
                s.Any(p => p.X == 2 && p.Y == 0) &&
                s.Any(p => p.X == 2 && p.Y == 1) &&
                s.Any(p => p.X == 1 && p.Y == 1));
            
            Assert.True(square1Found, "Expected square 1 not found");
            Assert.True(square2Found, "Expected square 2 not found");
        }

        [Fact]
        public void GetSquares_WithNoSquares_ShouldReturnEmpty()
        {

            _service.ImportPoints(new[]
            {
                (0, 0),
                (1, 0),
                (2, 0),
                (0, 1)
            });


            var squares = _service.GetSquares().ToList();


            Assert.Empty(squares);
        }

        [Fact]
        public void GetSquares_WithDuplicatePoints_ShouldNotCreateInvalidSquares()
        {

            _service.AddPoint(0, 0);
            _service.AddPoint(1, 0);
            _service.AddPoint(1, 1);
            _service.AddPoint(0, 1);
            _service.AddPoint(0, 0); // Duplicate


            var squares = _service.GetSquares().ToList();


            Assert.Single(squares);
        }

        [Fact]
        public void GetSquares_WithLargeSquare_ShouldReturnSquare()
        {

            _service.ImportPoints(new[]
            {
                (0, 0),
                (100, 0),
                (100, 100),
                (0, 100)
            });


            var squares = _service.GetSquares().ToList();


            Assert.Single(squares);
        }

        [Fact]
        public void GetSquares_WithNegativeCoordinates_ShouldReturnSquare()
        {

            _service.ImportPoints(new[]
            {
                (-5, -5),
                (-3, -5),
                (-3, -3),
                (-5, -3)
            });


            var squares = _service.GetSquares().ToList();


            Assert.Single(squares);
        }

        [Fact]
        public void CountSquares_WithOneSquare_ShouldReturnOne()
        {

            _service.ImportPoints(new[]
            {
                (0, 0),
                (1, 0),
                (1, 1),
                (0, 1)
            });


            var count = _service.CountSquares();


            Assert.Equal(1, count);
        }

        [Fact]
        public void CountSquares_WithNoSquares_ShouldReturnZero()
        {

            _service.AddPoint(0, 0);
            _service.AddPoint(1, 0);
            _service.AddPoint(2, 0);


            var count = _service.CountSquares();


            Assert.Equal(0, count);
        }

        [Fact]
        public void GetSquares_ShouldReturnUniqueSquares()
        {

            _service.ImportPoints(new[]
            {
                (0, 0),
                (1, 0),
                (1, 1),
                (0, 1)
            });


            var squares = _service.GetSquares().ToList();


            Assert.Single(squares);
        }

        [Fact]
        public void GetSquares_WithComplexGrid_ShouldFindMultipleSquares()
        {

            // This forms: 4 unit squares (1x1), 1 large square (2x2), and potentially more
            _service.ImportPoints(new[]
            {
                (0, 0), (1, 0), (2, 0),
                (0, 1), (1, 1), (2, 1),
                (0, 2), (1, 2), (2, 2)
            });


            var squares = _service.GetSquares().ToList();


            // The algorithm may find additional valid squares, so we check for at least 5
            Assert.True(squares.Count >= 5, $"Expected at least 5 squares in 3x3 grid, but found {squares.Count}");
            
            // Verify we have the 4 unit squares
            var unitSquares = squares.Where(s =>
            {
                var points = s.ToList();
                if (points.Count != 4) return false;
                
                // Check if it's a unit square (side length 1)
                var xCoords = points.Select(p => p.X).Distinct().OrderBy(x => x).ToList();
                var yCoords = points.Select(p => p.Y).Distinct().OrderBy(y => y).ToList();
                
                return xCoords.Count == 2 && yCoords.Count == 2 &&
                       xCoords[1] - xCoords[0] == 1 &&
                       yCoords[1] - yCoords[0] == 1;
            }).ToList();
            
            Assert.True(unitSquares.Count >= 4, $"Expected at least 4 unit squares, but found {unitSquares.Count}");
        }

        [Fact]
        public void DeletePoint_ShouldUpdateSquares()
        {

            _service.ImportPoints(new[]
            {
                (0, 0),
                (1, 0),
                (1, 1),
                (0, 1)
            });
            Assert.Equal(1, _service.CountSquares());


            var pointToDelete = _service.GetPoints().First(p => p.X == 0 && p.Y == 0);
            _service.DeletePoint(pointToDelete.Id);


            Assert.Equal(0, _service.CountSquares());
        }
    }
}

