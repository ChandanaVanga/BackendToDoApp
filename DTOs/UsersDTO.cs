using System.Text.Json.Serialization;
using ToDoApp.Models;

namespace ToDoApp.DTOs;

public record UsersDTO
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [JsonPropertyName("passwor_d")]
    public string Password { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    

}

public record UsersCreateDTO
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [JsonPropertyName("passwor_d")]
    public string Password { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }


}

public record UsersUpdateDTO
{
    [JsonPropertyName("user_name")]
    public long UserName { get; set; }

    [JsonPropertyName("passwor_d")]
    public string Password { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

}