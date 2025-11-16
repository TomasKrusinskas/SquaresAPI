using Squares.Core.Models;

namespace Squares.Tests.Models
{
    public class PointTests
    {
        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            var point = new Point(1, 5, 10);

            Assert.Equal(1, point.Id);
            Assert.Equal(5, point.X);
            Assert.Equal(10, point.Y);
        }

        [Fact]
        public void DistanceSquared_ShouldCalculateCorrectly()
        {
            var p1 = new Point(1, 0, 0);
            var p2 = new Point(2, 3, 4);

            var distanceSquared = p1.DistanceSquared(p2);

            Assert.Equal(25, distanceSquared);
        }

        [Fact]
        public void DistanceSquared_WithSamePoint_ShouldReturnZero()
        {
            var point = new Point(1, 5, 10);

            var distanceSquared = point.DistanceSquared(point);

            Assert.Equal(0, distanceSquared);
        }

        [Fact]
        public void DistanceSquared_WithNegativeCoordinates_ShouldCalculateCorrectly()
        {
            var p1 = new Point(1, -1, -1);
            var p2 = new Point(2, 1, 1);

            var distanceSquared = p1.DistanceSquared(p2);

            Assert.Equal(8, distanceSquared);
        }

        [Fact]
        public void Equals_WithSameCoordinates_ShouldReturnTrue()
        {
            var p1 = new Point(1, 5, 10);
            var p2 = new Point(2, 5, 10);

            Assert.True(p1.Equals(p2));
            Assert.True(p1 == p2 || p1.Equals(p2));
        }

        [Fact]
        public void Equals_WithDifferentCoordinates_ShouldReturnFalse()
        {
            var p1 = new Point(1, 5, 10);
            var p2 = new Point(2, 5, 11);

            Assert.False(p1.Equals(p2));
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            var point = new Point(1, 5, 10);

            Assert.False(point.Equals(null));
        }

        [Fact]
        public void GetHashCode_WithSameCoordinates_ShouldReturnSameHash()
        {
            var p1 = new Point(1, 5, 10);
            var p2 = new Point(2, 5, 10);

            Assert.Equal(p1.GetHashCode(), p2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WithDifferentCoordinates_ShouldReturnDifferentHash()
        {
            var p1 = new Point(1, 5, 10);
            var p2 = new Point(2, 5, 11);

            Assert.NotEqual(p1.GetHashCode(), p2.GetHashCode());
        }
    }
}
