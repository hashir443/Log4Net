using Entity.Entity.Home;
using KodeNest.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace KodeNest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeApiController : ControllerBase
    {
        private readonly IHomeService _service;

        public HomeApiController(IHomeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var homes = await _service.GetAllAsync();
            return Ok(homes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var home = await _service.GetByIdAsync(id);
            if (home == null) return NotFound();
            return Ok(home);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HomeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _service.CreateAsync(request);
            return Ok(new { message = "Created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HomeRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _service.UpdateAsync(id, request);
            return Ok(new { message = "Updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }
    }
}
