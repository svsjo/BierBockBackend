using BierBockBackend.Data;
using BierBockBackend.Identity;
using DataStorage;
using Microsoft.AspNetCore.Authorization;
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

<<<<<<< HEAD
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

=======

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
>>>>>>> 497ddb2ec8c135bfc8e666ae982783bf1c74424d
    [HttpPost("newChallenge", Name = "AddNewChallenge")]
    public RequestStatus<object> AddNewChallenge(Challenge challenge)
    {
        // TODO

        return new RequestStatus<object>
        {
        };
    }

    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPost("blockUser", Name = "BlockUser")]
    public RequestStatus<object> BlockUser(string userName)
    {
        // TODO

        return new RequestStatus<object>
        {
        };
    }
}