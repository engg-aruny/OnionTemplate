using Microsoft.AspNetCore.Mvc;
using OnionDemo.Domain.Services.Abstractions;

namespace OnionDemo.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingPongController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PingPongController(IServiceManager serviceManager) => _serviceManager = serviceManager;

        [HttpGet]
        public async Task<IActionResult> GetPingPongs(CancellationToken cancellationToken)
        {
            var pingPongDto = await _serviceManager.PingPongService.GetAllPingPongAsync(cancellationToken);

            return Ok(pingPongDto);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(CancellationToken cancellationToken)
        {
            await _serviceManager.PingPongService.Insert(cancellationToken);

            return Ok();
        }
    }
}
