 using ToDoApp.DTOs;

namespace ToDoApp.Models;


public record Users
{
    /// <summary>
    /// Primary Key - NOT NULL, Unique, Index is Available
    /// </summary>
    public long UserId { get; set; }
    public string UserName { get; set; }

    // public string Gender { get; set; }
     public string Password { get; set; }
    // public string Email { get; set; }
    
    

    public UsersDTO asDto => new UsersDTO
    {
        
        UserName = UserName,
        Password = Password,
       // Email = Email,
        
    };
}