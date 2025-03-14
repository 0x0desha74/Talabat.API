﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{

    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;


        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }



        [HttpPost("login")] //POST : api/Accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }


        [HttpPost("register")] //POST : api/Accounts/register

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {

            if (CheckEmailExists(model.Email).Result.Value) //sync
                return BadRequest(new ApiResponse(400,"Email already exists"));


            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split("@")[0]
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });

            

            
        }



        [Authorize]
        [HttpGet] //GET : api/accounts
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager) //create new token , till we know how to store user Tokens in Database
            });
        }



        [Authorize]
        [HttpGet("address")] //GET : api/accounts/address
        public async Task<ActionResult<AddressDto>> GetUserAddressAsync()
        {

            var user = await _userManager.FindUserWithAddressAsync(User);

            var mappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(mappedAddress);
        }



        [Authorize]
        [HttpPut("Address")] // PUT: api/accounts/address
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddressDto)
        {
            var address = _mapper.Map<AddressDto, Address>(updatedAddressDto);
            var user = await _userManager.FindUserWithAddressAsync(User);

            address.Id = user.Address.Id;
            user.Address = address;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(_mapper.Map<Address, AddressDto>(user.Address));

        }


        [HttpGet("emailexists")] //GET : api/accounts/emailexists?email=mustafa.elsayed@gmail.com
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }


    }
}
