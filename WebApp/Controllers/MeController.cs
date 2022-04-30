namespace WebApp.Controllers;



[Route("api/[controller]")]
[ApiController]
public class MeController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Me()
    {
        return Ok();
    }
}
