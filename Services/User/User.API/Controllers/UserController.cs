using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using User.API.Dto;
using User.API.Filters;
using User.API.Models;
using User.API.Repositories;

namespace User.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/users")]
    public class UserController : Controller
    {
        private readonly AccountRepository _accountRepository;
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public UserController(AccountRepository accountRepository, RoleRepository roleRepository, IMapper mapper,
            IAuthorizationService authorizationService)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts([FromQuery] PaginationFilter paginationFilter,
            [FromQuery] AccountColumnFilter accountColumnFilter)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var accounts =
                await _accountRepository.GetAllFilteredPaginated(paginationFilter.PageNumber, paginationFilter.PageSize,
                    accountColumnFilter);
            // get the roles of each account
            foreach (var account in accounts)
            {
                if (account != null) account.Role = await _roleRepository.Get(account.RoleId);
            }

            var accountDtos = _mapper.Map<IEnumerable<GetAccountDto>>(accounts);
            var totalCount = await _accountRepository.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / filter.PageSize);
            return Ok(new PaginatedResponse<IEnumerable<GetAccountDto>>(paginationFilter.PageNumber,
                paginationFilter.PageSize, totalPages, totalCount, accountDtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _accountRepository.Get(id);
            if (account == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, account, "AccountOwner");
            if (!result.Succeeded) return Forbid();

            account.Role = await _roleRepository.Get(account.RoleId);
            var accountDto = _mapper.Map<GetAccountDto>(account);
            return Ok(accountDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Account>> UpdateAccount(int id, UpdateAccountDto updateAccount)
        {
            var account = await _accountRepository.Get(id);
            if (account == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, account, "AccountOwner");
            if (!result.Succeeded) return Forbid();

            if (!ModelState.IsValid) return BadRequest(ModelState);
            _mapper.Map(updateAccount, account);
            account.Role = await _roleRepository.Get(account.RoleId);
            account.UpdatedAt = DateTime.Now;
            await _accountRepository.Update(account);
            return Ok(_mapper.Map<GetAccountDto>(account));
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Account>> PartialUpdateAccount(int id,
            [FromBody] JsonPatchDocument<UpdateAccountDto> patchDoc)
        {
            var account = await _accountRepository.Get(id);
            if (account == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, account, "AccountOwner");
            if (!result.Succeeded) return Forbid();

            var updateAccount = _mapper.Map<UpdateAccountDto>(account);
            patchDoc.ApplyTo(updateAccount, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _mapper.Map(updateAccount, account);
            // id gets lost when mapping, so we need to set it manually
            account.Id = id;

            account.Role = await _roleRepository.Get(account.RoleId);
            account.UpdatedAt = DateTime.Now;
            await _accountRepository.Update(account);
            return Ok(_mapper.Map<GetAccountDto>(account));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Account>> DeleteAccount(int id)
        {
            var account = await _accountRepository.Get(id);
            if (account == null) return NotFound();

            var result = await _authorizationService.AuthorizeAsync(User, account, "AccountOwner");
            if (!result.Succeeded) return Forbid();

            await _accountRepository.Delete(account);
            return Ok(_mapper.Map<GetAccountDto>(account));
        }
    }
}