using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Registration.Data;
using Registration.IRepository;
using Registration.Model;
using Registration.Model.Request;
using Registration.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Registration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AccountController(UserManager<User> userManager, IUnitOfWork unitOfWork,
            ILogger<AccountController> logger,
            IMapper mapper
            , IAuthManager authManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("login")]
        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {    
            var usern =  _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"Login Attempt for {userDTO.UserName} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = await _userManager.FindByNameAsync(userDTO.UserName);
            if (!await _authManager.ValidateUser(userDTO))
            {
                return Unauthorized();
            }

            //Create and return Access Token and Refresh Token
            var token = new { Token = await _authManager.CreateToken(user) };
            var refreshToken = new { Token = _authManager.CreateRefreshToken() };
            RefreshToken refreshTokenDTO = new RefreshToken
            {
                Token = refreshToken.Token,
                UserId = user.Id
            };
            await _unitOfWork.RefreshTokens.Insert(refreshTokenDTO);
            await _unitOfWork.Save();

            return Accepted(new object[] { token.Token, refreshToken.Token });
        }


        [HttpPost]
        [Authorize]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] ApiUser model)
        {
           

            _logger.LogInformation($"Registration Attempt for {model.UserName} ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }         
           
            var user = new User
            {
                Code = model.Code,
                FatherName = model.FatherName,
                FirstName = model.FirstName,
                Image = model.Image != null ? Convert.FromBase64String(model.Image) : null,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                CreatorUserName = GetUserNameFromToken()
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRoleAsync(user, model.Role);

            return Accepted();
        }

        private string GetUserNameFromToken()
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (accessToken.ToString().Length<8)
            {
                return null;
            }
            var jwt = accessToken.ToString().Remove(0, 7);
            var handler = new JwtSecurityTokenHandler();
            var btoken = handler.ReadJwtToken(jwt);
            var username = btoken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            return username;
        }

        [HttpPost]
        [Route("refresh")]
        [ServiceFilter(typeof(ClientIpCheckActionFilter))]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshReqest)
        {
            _logger.LogInformation($"Refresh Attempt for refreshReqest ");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isValidTokenValidator = RefreshTokenValidator.Validate(refreshReqest.RefreshToken);
            if (!isValidTokenValidator)
            {
                return BadRequest("Invalid Refresh Token");
            }

            RefreshToken refreshTokenDTO = await _unitOfWork.RefreshTokens.Get(x => x.Token == refreshReqest.RefreshToken);

            if (refreshTokenDTO == null)
            {
                return NotFound("Invalid Refresh Token");
            }

            await _unitOfWork.RefreshTokens.Delete(refreshTokenDTO.Id);

            User user = await _userManager.FindByIdAsync(refreshTokenDTO.UserId);
            if (user == null)
            {
                return NotFound("user not found");
            }

            var token = new { Token = await _authManager.CreateToken(user) };
            var refreshToken = new { Token = _authManager.CreateRefreshToken() };
            RefreshToken newrefreshTokenDTO = new RefreshToken
            {
                Token = refreshToken.Token,
                UserId = user.Id
            };

            await _unitOfWork.RefreshTokens.Insert(newrefreshTokenDTO);
            await _unitOfWork.Save();

            return Accepted(new object[] { token.Token, refreshToken.Token });
        }

        [HttpGet("checklastname")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> checkLastName(string lastName)
        {
            _logger.LogInformation($"InvalidateToken Attempt for invalidate  {lastName} token ");

            var user = await _unitOfWork.Users.Get(x => x.LastName == lastName);
            if (user != null)
            {
                return Accepted();
            }
            return NotFound();
          
        }


        [HttpGet("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> LogOut(string userName)
        {
            _logger.LogInformation($"InvalidateToken Attempt for invalidate  {userName} token ");

            var user = await _unitOfWork.Users.Get(x => x.UserName == userName);
            var refreshTokens = await _unitOfWork.RefreshTokens.GetAll(x => x.UserId == user.Id);
            _unitOfWork.RefreshTokens.DeleteRange(refreshTokens);
            await _unitOfWork.Save();
            return NoContent();
        }


    }
}
