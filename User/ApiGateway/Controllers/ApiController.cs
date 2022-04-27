using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ;
using System.Net;
using User.BusinessLogic.Services.Interfaces;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IUserService _UserService;


        public ApiController(IUserService UserService, ILogger<ApiController> logger)
        {
            _logger = logger;
            _UserService = UserService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserModel>> Get(Guid userId)
        {

            var user = await _UserService.GetByUserId(userId);
            return user != null ? new UserModel
            {
                UserId = userId,
                Address = user.Address,
                Education = user.Education,
                LastName = user.LastName,
                FirstName = user.FirstName,
            } : null;
        }
    }
}