using Microsoft.AspNetCore.Mvc;
using SunfeadApi.DTOs;
using SunfeadApi.Services.Interfaces;

namespace SunfeadApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUser(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse<UserDto>(false, null, "User not found"));

            return Ok(new ApiResponse<UserDto>(true, user));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiResponse<UserDto>(false, null, "User not found"));

            return Ok(new ApiResponse<UserDto>(true, user));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new ApiResponse<UserDto>(true, user, "User registered successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<UserDto>(false, null, ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Login([FromBody] LoginDto dto)
    {
        try
        {
            var user = await _userService.AuthenticateUserAsync(dto);
            if (user == null)
                return Unauthorized(new ApiResponse<UserDto>(false, null, "Invalid email or password"));

            return Ok(new ApiResponse<UserDto>(true, user, "Login successful"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<UserDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpGet("{userId:guid}/addresses")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AddressDto>>>> GetAddresses(Guid userId)
    {
        try
        {
            var addresses = await _userService.GetUserAddressesAsync(userId);
            return Ok(new ApiResponse<IEnumerable<AddressDto>>(true, addresses));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<IEnumerable<AddressDto>>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpPost("{userId:guid}/addresses")]
    public async Task<ActionResult<ApiResponse<AddressDto>>> AddAddress(Guid userId, [FromBody] CreateAddressDto dto)
    {
        try
        {
            var address = await _userService.AddAddressAsync(userId, dto);
            return Ok(new ApiResponse<AddressDto>(true, address, "Address added successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<AddressDto>(false, null, "An error occurred", new[] { ex.Message }));
        }
    }

    [HttpDelete("{userId:guid}/addresses/{addressId:guid}")]
    public async Task<ActionResult<ApiResponse>> DeleteAddress(Guid userId, Guid addressId)
    {
        try
        {
            var result = await _userService.DeleteAddressAsync(userId, addressId);
            if (!result)
                return NotFound(new ApiResponse(false, "Address not found"));

            return Ok(new ApiResponse(true, "Address deleted successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse(false, "An error occurred", new[] { ex.Message }));
        }
    }
}
