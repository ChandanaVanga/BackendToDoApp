using ToDoApp.Models;
using Dapper;
using ToDoApp.Utilities;
using ToDoApp.Repositories;


namespace ToDoApp.Repositories;

public interface IToDoListRepository
{
    Task<ToDoList> Create(ToDoList Item);
    Task<bool> Update(ToDoList Item);
    Task<bool> Delete(long ToDoId);
    Task<ToDoList> GetById(long ToDoId);
    Task<List<ToDoList>> GetList();

}
public class ToDoListRepository : BaseRepository, IToDoListRepository
{
    public ToDoListRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<ToDoList> Create(ToDoList Item)
    {
        var query = $@"INSERT INTO ""{TableNames.to_do_list}"" 
        (title, description, created_at, updated_at, is_completed, is_deleted, user_id) 
        VALUES (@Title, @Description, @CreatedAt, @UpdatedAt, @IsCompleted, @IsDeleted, @UserId) 
        RETURNING *";

        using (var con = NewConnection)
          return await con.QuerySingleAsync<ToDoList>(query,Item);
        
            // var res = await con.QuerySingleOrDefaultAsync<ToDoList>(query, Item);
            // return res;
        
    }

    public async Task<bool> Delete(long ToDoId)
    {
       var query = $@"DELETE FROM ""{TableNames.to_do_list}"" 
         WHERE to_do_id = @ToDoId";

         using (var con = NewConnection)
         {
             var res = await con.ExecuteAsync(query, new { ToDoId });
             return res > 0;
         }
    }


    public async Task<bool> Update(ToDoList Item)
    {
        var query = $@"UPDATE ""{TableNames.to_do_list}"" SET 
        title = @Title, description = @Description WHERE to_do_id = @ToDoId";

        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);
            return rowCount == 1;
        }
    }

    public async Task<ToDoList> GetById(long ToDoId)
    {
        var query = $@"SELECT * FROM ""{TableNames.to_do_list}"" 
        WHERE to_do_id = @ToDoId";
        // SQL-Injection

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<ToDoList>(query, new { ToDoId });
    }

    public async Task<List<ToDoList>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.to_do_list}""";

        List<ToDoList> res;
        using (var con = NewConnection) // Open connection
            res = (await con.QueryAsync<ToDoList>(query)).AsList(); // Execute the query
        // Close the connection

        // Return the result
        return res;
    }
}