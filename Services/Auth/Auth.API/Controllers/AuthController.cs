using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.API.Dto;
using Auth.API.Misc;
using Auth.API.Models;
using Auth.API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : Controller
    {
        private readonly AccountRepository _accountRepository;
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public AuthController(AccountRepository accountRepository, RoleRepository roleRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<ActionResult<object>> SignIn(SignInDto signInDto)
        {
            PasswordHelper passwordHelper = new();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _accountRepository.GetAccountByEmail(signInDto.Email);
            if (user == null) return Unauthorized();
            if (!passwordHelper.VerifyPassword(signInDto.Password, user.Password))
                return Unauthorized();
            
            user.Role = await _roleRepository.Get(user.RoleId);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };
            var token = JwtHelper.GetJwtSecurityToken(
                user.Email,
                Environment.GetEnvironmentVariable("Jwt_Secret"),
                Environment.GetEnvironmentVariable("Jwt_Issuer"),
                TimeSpan.FromMinutes(Convert.ToDouble(Environment.GetEnvironmentVariable("Jwt_Timeout"))),
                claims);
            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Account>> Register(RegisterAccountDto registerAccountDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _mapper.Map<Account>(registerAccountDto);
            user.Password = new PasswordHelper().HashPassword(registerAccountDto.Password);
            // all new users are set to "user" role
            user.RoleId = 3;
            await _accountRepository.Add(user);
            user.Role = await _roleRepository.Get(user.RoleId);
            return Ok(_mapper.Map<GetAccountDto>(user));
        }
        
        // allow admins to change user roles
        [Authorize(Roles = "Admin")]
        [HttpPost("user/{id}/role")]
        public async Task<ActionResult<Account>> ChangeRole(int id, ChangeRoleDto changeRoleDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _accountRepository.Get(id);
            if (user == null) return NotFound();
            var role = await _roleRepository.GetRoleByName(changeRoleDto.RoleName);
            if (role == null) return NotFound();
            
            user.RoleId = role.Id;
            await _accountRepository.Update(user);
            user.Role = role;
            return Ok(_mapper.Map<GetAccountDto>(user));
        }
    }
}