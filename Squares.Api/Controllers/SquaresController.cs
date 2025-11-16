using Microsoft.AspNetCore.Mvc;
using Squares.Api.DTOs;
using Squares.Core.Services;
using System.Linq;

namespace Squares.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SquaresController : ControllerBase
    {
        private readonly ISquareService _service;

        public SquaresController(ISquareService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetSquares()
        {
            var squares = _service.GetSquares()
                .Select(s => new SquareDto
                {
                    Points = s.Select(p => new PointDto { Id = p.Id, X = p.X, Y = p.Y }).ToList()
                });

            return Ok(squares);
        }

        [HttpGet("count")]
        public IActionResult Count()
        {
            return Ok(new { count = _service.CountSquares() });
        }
    }
}
