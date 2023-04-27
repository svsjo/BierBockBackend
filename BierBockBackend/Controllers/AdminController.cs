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


    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
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
            Status = Status.Successful
        };
    }


    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [HttpPost("newChallenge", Name = "AddNewChallenge")]
    public RequestStatus<object> AddNewChallenge(Challenge challenge)
    {
        _dbAppDatabaseContext.AddChallenge(challenge);

        return new RequestStatus<object>
        {
            Status = Status.Successful,
        };
    }
}