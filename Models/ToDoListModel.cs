 using ToDoApp.DTOs;

namespace ToDoApp.Models;

public record ToDoList
{
    /// <summary>
    /// Primary Key - NOT NULL, Unique, Index is Available
    /// </summary>
    public long ToDoId { get; set; }
    public string Title { get; set; }

    public string Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsCompleted { get; set; }

    
    public bool IsUpdated { get; set; }
     public bool IsDeleted { get; set; }


    public long UserId { get; set; }

    

    public ToDoListDTO asDto => new ToDoListDTO
    {
       ToDoId = ToDoId,
        Title = Title,
        Description = Description,
        UserId = UserId,
        CreatedAt = CreatedAt,
        IsCompleted = IsCompleted, 
        IsDeleted = IsDeleted, 
        UpdatedAt = UpdatedAt,
    };
}