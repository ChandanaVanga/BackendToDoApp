using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using ToDoApp.Repositories;
using ToDoApp.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ToDoApp.Controllers;

[ApiController]
[Route("api/todolist")]
[Authorize]
public class ToDoListController : ControllerBase
{
    private readonly ILogger<ToDoListController> _logger;
    private readonly IToDoListRepository _ToDoList;
    //  private readonly IProductRepository _Product;
    //private readonly IToDoListRepository _order;

    public ToDoListController(ILogger<ToDoListController> logger,
    IToDoListRepository ToDoList)
    {
        _logger = logger;
        _ToDoList = ToDoList;
        //// _Product = Product;
        // this._order = _order;
    }

    [HttpGet]
    public async Task<ActionResult<List<ToDoListDTO>>> GetList()
    {
        var res = await _ToDoList.GetList();

        return Ok(res.Select(x => x.asDto));
    }

    [HttpGet("{to_do_id}")]
    public async Task<ActionResult> GetById([FromRoute] int to_do_id)
    {
        var res = await _ToDoList.GetById(to_do_id);

        if (res == null)
            return NotFound();

        var dto = res.asDto;
        // dto.Products = (await _Product.GetListByCustomerId(id))
        //.Select(x => x.asDto).ToList();
        //  dto.Orders = (await _order.GetListByCustomerId(id)).Select(x => x.asDto).ToList();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ToDoListDTO Data)

    {

        //   var headers = Request.Headers.Authorization.ToString();
        // var jwt = headers.Replace("Bearer ", string.Empty);

        // Console.WriteLine(headers);

        // var handler = new JwtSecurityTokenHandler();
        // // Console.WriteLine("can validate token: " + handler.CanValidateToken.ToString());
        // // Console.WriteLine("can read token: " + handler.CanReadToken(jwt).ToString());

        // var jsonToken = handler.ReadJwtToken(jwt);
        // var tokenS = jsonToken as JwtSecurityToken;

        // var tokenId = tokenS.Claims.First(claim => claim.Type == ClaimTypes.SerialNumber).Value;


        var claims = User.Claims;
        var userIdClaimValue = claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;
        if (userIdClaimValue is null) return BadRequest();
        
        var toCreateToDoList = new ToDoList
        {
                Title = Data.Title,
                Description = Data.Description,
                CreatedAt = Data.CreatedAt,
                UpdatedAt = Data.UpdatedAt,
                IsCompleted = Data.IsCompleted,
                IsDeleted = Data.IsDeleted,
                UserId = long.Parse(userIdClaimValue)
          
        };

        var res = await _ToDoList.Create(toCreateToDoList);

        return StatusCode(StatusCodes.Status201Created, res.asDto);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] ToDoListCreateDTO Data)
    {
        var existingToDoList = await _ToDoList.GetById(id);

        if (existingToDoList == null)
            return NotFound();

        var toUpdateToDoList = existingToDoList with
        {
            Title = Data.Title,
            Description = Data.Description,
        };

        var didUpdate = await _ToDoList.Update(toUpdateToDoList);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();
    }

    [HttpDelete("{to_do_id}")]
    public async Task<ActionResult> DeleteToDoList([FromRoute] int to_do_id)
    {
        var existing = await _ToDoList.GetById(to_do_id);
        if (existing is null)
            return NotFound("No ToDo found with given id");

        var UserId = GetCurrentUserId();
        var Id = long.Parse(UserId);
        if (Id != existing.UserId)
            return Unauthorized("Your not authorized to Delete.");
        if (existing is null)
            return NotFound("No Todos found to Delete");    

        await _ToDoList.Delete(to_do_id);

        return NoContent();
    }

    private string GetCurrentUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userClaims = identity.Claims;
        return (userClaims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value);
    }
}