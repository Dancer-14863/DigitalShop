using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using User.API.Dto;
using User.API.Models;
using User.API.Repositories;

namespace User.API.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : Controller
    {
        private readonly AccountRepository _accountRepository;
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserController(AccountRepository accountRepository, RoleRepository roleRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            var accounts = await _accountRepository.GetAll();
            // get the roles of each account
            foreach (var account in accounts)
            {
                account.Role = await _roleRepository.Get(account.RoleId);
            }
            
            var accountDtos = _mapper.Map<IEnumerable<GetAccountDto>>(accounts);
            return Ok(accountDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _accountRepository.Get(id);
            if (account == null) return NotFound();
            account.Role = await _roleRepository.Get(account.RoleId);
            var accountDto = _mapper.Map<GetAccountDto>(account);
            return Ok(accountDto);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Account>> UpdateAccount(int id, UpdateAccountDto updateAccount)
        {
            var account = await _accountRepository.Get(id);
            if (account == null) return NotFound();
            _mapper.Map(updateAccount, account);
            await _accountRepository.Update(account);
            return Ok(_mapper.Map<GetAccountDto>(account));
        }
        
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Account>> PartialUpdateAccount(int id, [FromBody] JsonPatchDocument<UpdateAccountDto> patchDoc)
        {
            var account = await _accountRepository.Get(id);
            if (account == null) return NotFound();
            
            var updateAccount = _mapper.Map<UpdateAccountDto>(account);
            patchDoc.ApplyTo(updateAccount, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _mapper.Map(updateAccount, account);
            // id gets lost when mapping, so we need to set it manually
            account.Id = id;
            
            await _accountRepository.Update(account);
            return Ok(_mapper.Map<GetAccountDto>(account));
        }
        
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Account>> DeleteAccount(int id)
        {
            var account = await _accountRepository.Get(id);
            if (account == null) return NotFound();
            await _accountRepository.Delete(account);
            return Ok(_mapper.Map<GetAccountDto>(account));
        }
        
    }
}