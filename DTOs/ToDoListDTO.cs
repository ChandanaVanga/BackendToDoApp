
using System.Text.Json.Serialization;
using ToDoApp.Models;

namespace ToDoApp.DTOs;

public record ToDoListDTO
{
    [JsonPropertyName("to_do_id")]
    public long ToDoId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("is_completed")]
    public bool IsCompleted { get; set; }

   [JsonPropertyName("is_deleted")]
    public bool IsDeleted { get; set; }

    [JsonPropertyName("user_id")]
    public long UserId { get; set; }
}

public record ToDoListCreateDTO
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }


}

public record ToDoListUpdateDTO
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

}