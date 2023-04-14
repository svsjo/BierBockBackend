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

    [HttpPost("newChallenge", Name = "AddNewChallenge")]
    public RequestStatus<object> AddNewChallenge(Challenge challenge)
    {
        _dbAppDatabaseContext.AddChallenge(challenge);

        var status = Status.Successful;

        return new RequestStatus<object>
        {
            Status = status
        };
    }

    /*
     * Sperre User XY
     * ...?
     */
}