#region

using BierBockBackend.BackgroundServices;
using BierBockBackend.Data;
using BierBockBackend.Identity;
using DataStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#endregion

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


    [Authorize(Policy = IdentityData.AdminUserPolicy)]
    [HttpPost("lockUser", Name = "LockUser")]
    public RequestStatus<object> LockUser(string username)
    {
        var user = _dbAppDatabaseContext.GetUsers()
            .FirstOrDefault(x => x.UserName == username)!;

        user.AccountLocked = true;
        _dbAppDatabaseContext.Update(user);
        _dbAppDatabaseContext.SaveChanges();

        return new RequestStatus<object>
        {
            Status = Status.Successful
        };
    }


    [Authorize(Policy = IdentityData.AdminUserPolicy)]
    [HttpPost("newChallenge", Name = "AddNewChallenge")]
    public RequestStatus<object> AddNewChallenge(Challenge challenge)
    {
        _dbAppDatabaseContext.AddChallenge(challenge);

        return new RequestStatus<object>
        {
            Status = Status.Successful
        };
    }

    [Authorize(Policy = IdentityData.AdminUserPolicy)]
    [HttpGet("getUsers", Name = "GetUsers")]
    public RequestStatus<IEnumerable<object>> GetUsers()
    {
        var users = _dbAppDatabaseContext.GetUsers()
            .Select(x => new
            {
                x.UserName,
                x.AccountLocked
            })
            .ToList();

        return new RequestStatus<IEnumerable<object>>()
        {
            Status = Status.Successful,
            Result = users
        };
    }

    [Authorize(Policy = IdentityData.AdminUserPolicy)]
    [HttpGet("newTestData", Name = "InsertNewTestData")]
    public RequestStatus<object> InsertNewTestData()
    {
        var testData = new TestDataHolder(_dbAppDatabaseContext);

        /* Folgende Methode kann nach belieben ausgetauscht werden -> sollte auch mit Hot Reload funktionieren */
        testData.InitHorberDrinkActions(30);

        return new RequestStatus<object>()
        {
            Status = Status.Successful,
        };
    }
}