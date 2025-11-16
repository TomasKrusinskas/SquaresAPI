using Squares.Core.Models;
using Squares.Core.Repositories;

namespace Squares.Tests.Repositories
{
    public class PointRepositoryTests
    {
        private readonly PointRepository _repository;

        public PointRepositoryTests()
        {
            _repository = new PointRepository();
        }

        [Fact]
        public void Add_ShouldAddNewPoint()
        {

            var point = _repository.Add(5, 10);


            Assert.NotNull(point);
            Assert.Equal(5, point.X);
            Assert.Equal(10, point.Y);
            Assert.True(point.Id > 0);
        }

        [Fact]
        public void Add_WithDuplicateCoordinates_ShouldReturnExistingPoint()
        {

            var firstPoint = _repository.Add(5, 10);


            var secondPoint = _repository.Add(5, 10);


            Assert.Equal(firstPoint.Id, secondPoint.Id);
            Assert.Single(_repository.GetAll().Where(p => p.X == 5 && p.Y == 10));
        }

        [Fact]
        public void GetAll_ShouldReturnAllPoints()
        {

            _repository.Add(1, 1);
            _repository.Add(2, 2);
            _repository.Add(3, 3);


            var points = _repository.GetAll().ToList();


            Assert.Equal(3, points.Count);
        }

        [Fact]
        public void GetAll_WithEmptyRepository_ShouldReturnEmpty()
        {

            var points = _repository.GetAll().ToList();


            Assert.Empty(points);
        }

        [Fact]
        public void GetById_WithExistingId_ShouldReturnPoint()
        {

            var addedPoint = _repository.Add(5, 10);


            var retrievedPoint = _repository.GetById(addedPoint.Id);


            Assert.NotNull(retrievedPoint);
            Assert.Equal(addedPoint.Id, retrievedPoint!.Id);
            Assert.Equal(5, retrievedPoint.X);
            Assert.Equal(10, retrievedPoint.Y);
        }

        [Fact]
        public void GetById_WithNonExistentId_ShouldReturnNull()
        {

            var point = _repository.GetById(999);


            Assert.Null(point);
        }

        [Fact]
        public void GetByCoordinates_WithExistingCoordinates_ShouldReturnPoint()
        {

            var addedPoint = _repository.Add(5, 10);


            var retrievedPoint = _repository.GetByCoordinates(5, 10);


            Assert.NotNull(retrievedPoint);
            Assert.Equal(addedPoint.Id, retrievedPoint!.Id);
        }

        [Fact]
        public void GetByCoordinates_WithNonExistentCoordinates_ShouldReturnNull()
        {

            var point = _repository.GetByCoordinates(999, 999);


            Assert.Null(point);
        }

        [Fact]
        public void Delete_WithExistingId_ShouldReturnTrue()
        {

            var point = _repository.Add(5, 10);


            var result = _repository.Delete(point.Id);


            Assert.True(result);
            Assert.Null(_repository.GetById(point.Id));
        }

        [Fact]
        public void Delete_WithNonExistentId_ShouldReturnFalse()
        {

            var result = _repository.Delete(999);


            Assert.False(result);
        }

        [Fact]
        public void Clear_ShouldRemoveAllPoints()
        {

            _repository.Add(1, 1);
            _repository.Add(2, 2);
            _repository.Add(3, 3);


            _repository.Clear();


            Assert.Empty(_repository.GetAll());
        }

        [Fact]
        public void Add_ShouldGenerateUniqueIds()
        {

            var p1 = _repository.Add(1, 1);
            var p2 = _repository.Add(2, 2);
            var p3 = _repository.Add(3, 3);


            Assert.NotEqual(p1.Id, p2.Id);
            Assert.NotEqual(p2.Id, p3.Id);
            Assert.NotEqual(p1.Id, p3.Id);
        }

        [Fact]
        public void Add_WithNegativeCoordinates_ShouldWork()
        {

            var point = _repository.Add(-5, -10);


            Assert.Equal(-5, point.X);
            Assert.Equal(-10, point.Y);
        }

        [Fact]
        public void Add_WithZeroCoordinates_ShouldWork()
        {

            var point = _repository.Add(0, 0);


            Assert.Equal(0, point.X);
            Assert.Equal(0, point.Y);
        }
    }
}

