using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Registration.Data;
using Registration.IRepository;
using Registration.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserManagerController> _logger;
        private readonly UserManager<User> _userManager;
        public UserManagerController(IUnitOfWork unitOfWork, ILogger<UserManagerController> logger, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("getusers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAll();
            var apiUsers = new List<ApiUser>();
            users.ToList().ForEach(x =>
           {
               apiUsers.Add(new ApiUser
               {
                   Id = x.Id,
                   Code = x.Code,
                   FatherName = x.FatherName,
                   FirstName = x.FirstName,
                   Image = x.Image != null ? Convert.ToBase64String(x.Image) : null,
                   LastName = x.LastName,
                   PhoneNumber = x.PhoneNumber,
                   UserName = x.UserName,
                   CreatorUserName=x.CreatorUserName
               });
           });

            return Ok(apiUsers);
        }

        [HttpGet("getuser")]       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            var apiUser = new ApiUser
            {
                Id=user.Id,
                Code = user.Code,
                FatherName = user.FatherName,
                FirstName = user.FirstName,
                Image = user.Image != null ? Convert.ToBase64String(user.Image) : null,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                CreatorUserName = user.CreatorUserName
            };

            return Ok(apiUser);
        }

        [Authorize]
        [HttpPost]
        [Route("update")]        
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] ApiUser model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
                return BadRequest(ModelState);
            }

            User user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
                return BadRequest("Submitted data is invalid");
            }

            user.Code = model.Code;
            user.FatherName = model.FatherName;
            user.FirstName = model.FirstName;
            user.Image = model.Image != null ? Convert.FromBase64String(model.Image) : null;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;            

            IdentityResult result = await _userManager.UpdateAsync(user);           
            return NoContent();
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        //[Route("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(Delete)}");
                return BadRequest();
            }

            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(Delete)}");
                return BadRequest("Submitted data is invalid");
            }
            IdentityResult result = await _userManager.DeleteAsync(user);
            return NoContent();

        }
    }
}
