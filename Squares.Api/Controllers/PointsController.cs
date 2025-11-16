using Microsoft.AspNetCore.Mvc;
using Squares.Api.DTOs;
using Squares.Core.Services;
using System.Linq;

namespace Squares.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly ISquareService _service;

        public PointsController(ISquareService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var pts = _service.GetPoints()
                .Select(p => new PointDto { Id = p.Id, X = p.X, Y = p.Y });

            return Ok(pts);
        }

        [HttpPost("import")]
        public IActionResult Import([FromBody] ImportPointsRequest request)
        {
            if (request?.Points == null)
                return BadRequest("Points required.");

            var coords = request.Points.Select(p => (p.X, p.Y));
            _service.ImportPoints(coords);

            return NoContent();
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddPointRequest request)
        {
            if (request == null)
                return BadRequest();

            var point = _service.AddPoint(request.X, request.Y);
            var dto = new PointDto { Id = point.Id, X = point.X, Y = point.Y };
            return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var ok = _service.DeletePoint(id);
            if (!ok) 
                return NotFound();
            return NoContent();
        }
    }
}
