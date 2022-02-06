using System;
using HaloBiz.MyServices;
using Microsoft.AspNetCore.Mvc;

namespace HaloBiz.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactController:ControllerBase
    {
        private readonly IContactServiceImpl _contactSerceImpl;

        public ContactController(IContactServiceImpl contactSerceImpl)
        {
            this._contactSerceImpl = contactSerceImpl;
        }
    }
}
