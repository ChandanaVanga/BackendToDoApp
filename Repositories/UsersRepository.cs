using ToDoApp.Models;
using Dapper;
using ToDoApp.Utilities;
using ToDoApp.Repositories;


namespace ToDoApp.Repositories;

public interface IUsersRepository
{
    Task<Users> Create(Users Item);
    Task<bool> Update(Users Item);
    Task<bool> Delete(long UserId);
    Task<Users> GetById(long UserId);
    Task<List<Users>> GetList();

    Task<Users> GetByUserName(string UserName);

}
public class UsersRepository : BaseRepository, IUsersRepository
{
    public UsersRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Users> Create(Users Item)
    {
        var query = $@"INSERT INTO ""{TableNames.users}"" 
        (user_id, user_name, passwor_d, email) 
        VALUES (@UserId, @UserName, @Password, @Email) 
        RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Users>(query, Item);
            return res;
        }
    }

    public async Task<bool> Delete(long UserId)
    {
       var query = $@"DELETE FROM ""{TableNames.users}"" 
         WHERE user_id = @UserId";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { UserId });
             return res > 0;
        }
    }



    public async Task<bool> Update(Users Item)
    {
       var query = $@"UPDATE ""{TableNames.users}"" SET user_id = @UserId, 
        user_name = @UserName, gender = @Gender, passwor_d = @Password, 
        email = @Email WHERE user_id = @UserId ";

        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);
            return rowCount == 1;
        }
    }

    public async Task<Users> GetById(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" 
        WHERE user_id = @UserId";
        // SQL-Injection

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { UserId });
    }

    public async Task<List<Users>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""";

        List<Users> res;
        using (var con = NewConnection) // Open connection
            res = (await con.QueryAsync<Users>(query)).AsList(); // Execute the query
        // Close the connection

        // Return the result
        return res;throw new NotImplementedException();
    }

    public async Task<Users> GetByUserName(string UserName)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" 
        WHERE user_name = @UserName";
        // SQL-Injection

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { UserName });
    }
}