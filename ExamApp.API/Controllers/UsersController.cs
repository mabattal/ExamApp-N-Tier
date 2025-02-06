using ExamApp.Repositories.Enums;
using ExamApp.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.API.Controllers
{
    public class UsersController(IUserService _userService) : CustomBaseController
    {
        //kısa yazımı
        //[HttpGet]
        //public async Task<IActionResult> GetAll() => CreateActionResult(await _userService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            return CreateActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequestDto createUserRequest)
        {
            var result = await _userService.AddAsync(createUserRequest);
            return CreateActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserRequestDto updateUserRequest)
        {
            var result = await _userService.UpdateAsync(id, updateUserRequest);
            return CreateActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _userService.GetByEmailAsync(email);
            return CreateActionResult(result);
        }

        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetByRole(UserRole role)
        {
            var result = await _userService.GetByRole(role);
            return CreateActionResult(result);
        }
    }
}
