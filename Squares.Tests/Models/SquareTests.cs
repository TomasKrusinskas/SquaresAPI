using Squares.Core.Models;

namespace Squares.Tests.Models
{
    public class SquareTests
    {
        [Fact]
        public void Constructor_WithValidSquare_ShouldCreateSquare()
        {
            var p1 = new Point(1, -1, 1);
            var p2 = new Point(2, 1, 1);
            var p3 = new Point(3, 1, -1);
            var p4 = new Point(4, -1, -1);

            var square = new Square(p1, p2, p3, p4);

            Assert.Equal(4, square.Points.Count);
            Assert.Contains(p1, square.Points);
            Assert.Contains(p2, square.Points);
            Assert.Contains(p3, square.Points);
            Assert.Contains(p4, square.Points);
        }

        [Fact]
        public void Constructor_WithThreePoints_ShouldThrowArgumentException()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 1, 0);
            var p3 = new Point(3, 0, 1);

            Assert.Throws<ArgumentException>(() => new Square(new[] { p1, p2, p3 }));
        }

        [Fact]
        public void Constructor_WithFivePoints_ShouldThrowArgumentException()
        {
            var points = new[]
            {
                new Point(1, 0, 0),
                new Point(2, 1, 0),
                new Point(3, 0, 1),
                new Point(4, 1, 1),
                new Point(5, 2, 2)
            };

            Assert.Throws<ArgumentException>(() => new Square(points));
        }

        [Fact]
        public void Constructor_WithNullPoints_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Square((IEnumerable<Point>)null!));
        }

        [Fact]
        public void Constructor_WithDuplicatePoints_ShouldThrowArgumentException()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 1, 0);
            var p3 = new Point(3, 0, 1);
            var p4 = new Point(4, 0, 0);

            Assert.Throws<ArgumentException>(() => new Square(p1, p2, p3, p4));
        }

        [Fact]
        public void IsValidSquare_WithValidSquare_ShouldReturnTrue()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 1, 0);
            var p3 = new Point(3, 1, 1);
            var p4 = new Point(4, 0, 1);

            var isValid = Square.IsValidSquare(p1, p2, p3, p4);

            Assert.True(isValid);
        }

        [Fact]
        public void IsValidSquare_WithRotatedSquare_ShouldReturnTrue()
        {
            var p1 = new Point(1, -1, 1);
            var p2 = new Point(2, 1, 1);
            var p3 = new Point(3, 1, -1);
            var p4 = new Point(4, -1, -1);

            var isValid = Square.IsValidSquare(p1, p2, p3, p4);

            Assert.True(isValid);
        }

        [Fact]
        public void IsValidSquare_WithRectangle_ShouldReturnFalse()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 2, 0);
            var p3 = new Point(3, 2, 1);
            var p4 = new Point(4, 0, 1);

            var isValid = Square.IsValidSquare(p1, p2, p3, p4);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValidSquare_WithRhombus_ShouldReturnFalse()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 2, 1);
            var p3 = new Point(3, 4, 0);
            var p4 = new Point(4, 2, -1);

            var isValid = Square.IsValidSquare(p1, p2, p3, p4);

            Assert.False(isValid);
        }

        [Fact]
        public void IsValidSquare_WithDuplicatePoints_ShouldReturnFalse()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 1, 0);
            var p3 = new Point(3, 0, 1);
            var p4 = new Point(4, 0, 0);

            var isValid = Square.IsValidSquare(p1, p2, p3, p4);

            Assert.False(isValid);
        }

        [Fact]
        public void Equals_WithSamePoints_ShouldReturnTrue()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 1, 0);
            var p3 = new Point(3, 1, 1);
            var p4 = new Point(4, 0, 1);

            var square1 = new Square(p1, p2, p3, p4);
            var square2 = new Square(p4, p3, p2, p1);

            Assert.True(square1.Equals(square2));
        }

        [Fact]
        public void Equals_WithDifferentPoints_ShouldReturnFalse()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 1, 0);
            var p3 = new Point(3, 1, 1);
            var p4 = new Point(4, 0, 1);
            var p5 = new Point(5, 2, 2);

            var square1 = new Square(p1, p2, p3, p4);
            var square2 = new Square(p1, p2, p3, p5);

            Assert.False(square1.Equals(square2));
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 1, 0);
            var p3 = new Point(3, 1, 1);
            var p4 = new Point(4, 0, 1);
            var square = new Square(p1, p2, p3, p4);

            var result = square.ToString();

            Assert.Contains("Square:", result);
            Assert.Contains("(0, 0)", result);
            Assert.Contains("(1, 0)", result);
        }
    }
}

