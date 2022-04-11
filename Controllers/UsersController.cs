using ToDoApp.DTOs;
using ToDoApp.Models;
using ToDoApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ToDoApp.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersRepository _users;
    // private readonly IUserRepository _user;

    public UsersController(ILogger<UsersController> logger,
    IUsersRepository users)
    {
        _logger = logger;
        _users = users;
        
    }

     [HttpPost]
    public async Task<ActionResult<Users>> Create([FromBody] UsersDTO Data)
     {

         var toCreateUsers = new Users
        {
            
             UserName = Data.UserName,
             Password = Data.Password?.Trim(),
             //Email = Data.Email,
        };

         var createdItem = await _users.Create(toCreateUsers);

         return StatusCode(StatusCodes.Status201Created, createdItem);
     }
[HttpGet]
    public async Task<ActionResult<List<UsersDTO>>> GetList()
    {
        var usersList = await _users.GetList();

        // User -> UserDTO
        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList);
    }

     [HttpGet("{user_id}")]
    public async Task<ActionResult> GetById([FromRoute] int user_id)
    {
        var res = await _users.GetById(user_id);

        if (res == null)
            return NotFound();

        var dto = res.asDto;
        // dto.Products = (await _Product.GetListByCustomerId(id))
        //.Select(x => x.asDto).ToList();
        //  dto.Orders = (await _order.GetListByCustomerId(id)).Select(x => x.asDto).ToList();

        return Ok(dto);
    }


    [HttpPut("{user_id}")]
    public async Task<ActionResult> UpdateUsers([FromRoute] int user_id,
    [FromBody] UsersCreateDTO Data)
    {
        var existing = await _users.GetById(user_id);
        if (existing is null)
            return NotFound("No User found with given id");

        var toUpdateItem = existing with
        {
            UserName = Data.UserName,
            Password = Data.Password?.Trim(),
           // Email = Data.Email
    
        };

        await _users.Update(toUpdateItem);

        return NoContent();
    }

    [HttpDelete("{user_id}")]
    public async Task<ActionResult> DeleteUsers([FromRoute] int user_id)
    {
        var existing = await _users.GetById(user_id);
        if (existing is null)
            return NotFound("No users found with given id");

        await _users.Delete(user_id);

        return NoContent();
    }
}