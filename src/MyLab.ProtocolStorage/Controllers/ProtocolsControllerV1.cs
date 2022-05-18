using Microsoft.AspNetCore.Mvc;

namespace MyLab.ProtocolStorage.Controllers
{
    [Route("v1/protocols")]
    [ApiController]
    public class ProtocolsControllerV1 : ControllerBase
    {
        [HttpPost]
        public IActionResult PushEntity()
        {

        }
    }
}
