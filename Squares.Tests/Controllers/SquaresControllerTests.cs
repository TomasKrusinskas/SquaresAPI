using Microsoft.AspNetCore.Mvc;
using Moq;
using Squares.Api.Controllers;
using Squares.Core.Models;
using Squares.Core.Services;

namespace Squares.Tests.Controllers
{
    public class SquaresControllerTests
    {
        private readonly Mock<ISquareService> _mockService;
        private readonly SquaresController _controller;

        public SquaresControllerTests()
        {
            _mockService = new Mock<ISquareService>();
            _controller = new SquaresController(_mockService.Object);
        }

        [Fact]
        public void GetSquares_ShouldReturnAllSquares()
        {

            var squares = new List<IEnumerable<Point>>
            {
                new List<Point>
                {
                    new Point(1, 0, 0),
                    new Point(2, 1, 0),
                    new Point(3, 1, 1),
                    new Point(4, 0, 1)
                }
            };
            _mockService.Setup(s => s.GetSquares()).Returns(squares);


            var result = _controller.GetSquares();


            var okResult = Assert.IsType<OkObjectResult>(result);
            var squareDtos = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Single(squareDtos);
        }

        [Fact]
        public void GetSquares_WithEmptyList_ShouldReturnEmptyList()
        {

            _mockService.Setup(s => s.GetSquares()).Returns(Enumerable.Empty<IEnumerable<Point>>());


            var result = _controller.GetSquares();


            var okResult = Assert.IsType<OkObjectResult>(result);
            var squareDtos = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Empty(squareDtos);
        }

        [Fact]
        public void Count_ShouldReturnSquareCount()
        {

            _mockService.Setup(s => s.CountSquares()).Returns(5);


            var result = _controller.Count();


            var okResult = Assert.IsType<OkObjectResult>(result);
            var countValue = okResult.Value;
            var countProperty = countValue!.GetType().GetProperty("count");
            Assert.NotNull(countProperty);
            var count = countProperty.GetValue(countValue);
            Assert.Equal(5, count);
        }

        [Fact]
        public void Count_WithZeroSquares_ShouldReturnZero()
        {

            _mockService.Setup(s => s.CountSquares()).Returns(0);


            var result = _controller.Count();


            var okResult = Assert.IsType<OkObjectResult>(result);
            var countValue = okResult.Value;
            var countProperty = countValue!.GetType().GetProperty("count");
            var count = countProperty!.GetValue(countValue);
            Assert.Equal(0, count);
        }
    }
}

