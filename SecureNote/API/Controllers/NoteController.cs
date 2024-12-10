using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
public class NoteController : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> Get() {
        return Ok("Hello World");
    }
}
