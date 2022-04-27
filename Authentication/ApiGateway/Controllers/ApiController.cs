using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using Authentication.BusinessLogic.Services.Interfaces;
using Shared.RabbitMQ;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {

        private readonly ILogger<ApiController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthenticationAddedSender _authenticationAddedSender;


        public ApiController(IAuthenticationService authenticationService, IAuthenticationAddedSender authenticationAddedSender, ILogger<ApiController> logger)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _authenticationAddedSender = authenticationAddedSender;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserAuthenticationModel>> Get(Guid userId)
        {
            var auth = await _authenticationService.GetByUserId(userId);
            return auth != null ? new UserAuthenticationModel
            {
                Email = auth.Email,
                MobileNumber = auth.MobileNumber,
                UserId = auth.UserId
            } : null;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<Guid>> Register(UserAuthenticationModel userAuthenticationModel)
        {
            var userId = await _authenticationService.Register(userAuthenticationModel.Email, userAuthenticationModel.MobileNumber);
            _authenticationAddedSender.SendAuthentication(userId, userAuthenticationModel.FirstName, userAuthenticationModel.LastName, userAuthenticationModel.Address, userAuthenticationModel.Education);


            return userId;
        }
    }
}