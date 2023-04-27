using BierBockBackend.Data;
using DataStorage;
using Microsoft.AspNetCore.Mvc;

namespace BierBockBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController
{
    private readonly AppDatabaseContext _dbAppDatabaseContext;

    public AdminController(AppDatabaseContext dbAppDatabaseContext)
    {
        _dbAppDatabaseContext = dbAppDatabaseContext;
    }

    [HttpPost("lockUser",Name = "LockUser")]
    public RequestStatus<object> LockUser(string username)
    {
        var user =  _dbAppDatabaseContext.GetUsers()
            .FirstOrDefault(x => x.UserName == username)!;
        user.AccountLocked = true;
        _dbAppDatabaseContext.Update(user);
        _dbAppDatabaseContext.SaveChanges();
        return new RequestStatus<object>()
        {

        };
    }

    [HttpPost("newChallenge", Name = "AddNewChallenge")]
    public RequestStatus<object> AddNewChallenge(Challenge challenge)
    {
        return new RequestStatus<object>
        {
        };
    }

    /*
     * Sperre User XY
     * ...?
     */
}