using Microsoft.AspNetCore.Mvc;
using Moq;
using Squares.Api.Controllers;
using Squares.Api.DTOs;
using Squares.Core.Models;
using Squares.Core.Services;

namespace Squares.Tests.Controllers
{
    public class PointsControllerTests
    {
        private readonly Mock<ISquareService> _mockService;
        private readonly PointsController _controller;

        public PointsControllerTests()
        {
            _mockService = new Mock<ISquareService>();
            _controller = new PointsController(_mockService.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllPoints()
        {

            var points = new List<Point>
            {
                new Point(1, 0, 0),
                new Point(2, 1, 1)
            };
            _mockService.Setup(s => s.GetPoints()).Returns(points);


            var result = _controller.GetAll();


            var okResult = Assert.IsType<OkObjectResult>(result);
            var pointDtos = Assert.IsAssignableFrom<IEnumerable<PointDto>>(okResult.Value);
            Assert.Equal(2, pointDtos.Count());
        }

        [Fact]
        public void GetAll_WithEmptyList_ShouldReturnEmptyList()
        {

            _mockService.Setup(s => s.GetPoints()).Returns(Enumerable.Empty<Point>());


            var result = _controller.GetAll();


            var okResult = Assert.IsType<OkObjectResult>(result);
            var pointDtos = Assert.IsAssignableFrom<IEnumerable<PointDto>>(okResult.Value);
            Assert.Empty(pointDtos);
        }

        [Fact]
        public void Import_WithValidRequest_ShouldCallService()
        {

            var request = new ImportPointsRequest
            {
                Points = new List<AddPointRequest>
                {
                    new AddPointRequest { X = 0, Y = 0 },
                    new AddPointRequest { X = 1, Y = 1 }
                }
            };


            var result = _controller.Import(request);


            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.ImportPoints(It.IsAny<IEnumerable<(int, int)>>()), Times.Once);
        }

        [Fact]
        public void Import_WithNullRequest_ShouldReturnBadRequest()
        {

            var result = _controller.Import(null);


            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Import_WithNullPoints_ShouldReturnBadRequest()
        {

            var request = new ImportPointsRequest { Points = null! };


            var result = _controller.Import(request);


            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Add_WithValidRequest_ShouldReturnCreated()
        {

            var point = new Point(1, 5, 10);
            _mockService.Setup(s => s.AddPoint(5, 10)).Returns(point);


            var result = _controller.Add(new AddPointRequest { X = 5, Y = 10 });


            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var pointDto = Assert.IsType<PointDto>(createdResult.Value);
            Assert.Equal(5, pointDto.X);
            Assert.Equal(10, pointDto.Y);
        }

        [Fact]
        public void Add_WithNullRequest_ShouldReturnBadRequest()
        {

            var result = _controller.Add(null);


            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Delete_WithExistingId_ShouldReturnNoContent()
        {

            _mockService.Setup(s => s.DeletePoint(1)).Returns(true);


            var result = _controller.Delete(1);


            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_WithNonExistentId_ShouldReturnNotFound()
        {

            _mockService.Setup(s => s.DeletePoint(999)).Returns(false);


            var result = _controller.Delete(999);


            Assert.IsType<NotFoundResult>(result);
        }
    }
}

