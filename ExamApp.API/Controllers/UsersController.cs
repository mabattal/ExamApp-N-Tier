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

        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize)
        {
            var result = await _userService.GetPagedAllAsync(pageNumber, pageSize);
            return CreateActionResult(result);
        }

        [HttpGet("{value}")]
        public async Task<IActionResult> GetByIdOrEmail(string value)
        {
            if (int.TryParse(value, out int id))
            {
                var result = await _userService.GetByIdOrEmailAsync(id, null);
                return CreateActionResult(result);
            }
            else
            {
                var result = await _userService.GetByIdOrEmailAsync(null, value);
                return CreateActionResult(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequestDto createUserRequest)
        {
            var result = await _userService.AddAsync(createUserRequest);
            return CreateActionResult(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateUserRequestDto updateUserRequest)
        {
            var result = await _userService.UpdateAsync(id, updateUserRequest);
            return CreateActionResult(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            return CreateActionResult(result);
        }

        [HttpGet("role/{role:int}")]
        public async Task<IActionResult> GetByRole(UserRole role)
        {
            var result = await _userService.GetByRole(role);
            return CreateActionResult(result);
        }
    }
}
