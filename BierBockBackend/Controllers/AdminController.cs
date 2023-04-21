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