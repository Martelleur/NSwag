using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Patterns;
using server.Constants;
using server.DTO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace server.Controllers;

/// <summary>
/// Controller class used for endpoint <see cref="RoutePath.Test"/>.
/// </summary>
[ApiController]
[ApiVersion(Api.MAJOR_VERSION_ONE)]
[Route(RoutePath.Test)]
[Produces("application/json")]
public class TestControllerVersionOne : ControllerBase
{

    /// <summary>
    /// Used to ping a valid user name.
    /// </summary>
    /// <param name="userName">A <see cref="string"/></param>
    /// <returns>A <see cref="UserNameDTO"/></returns>
    /// <response code="200">With the user name</response>
    [HttpGet("{userName}/ping", Name = "PingUserNameV1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<UserNameDTO>> PingUserName(
        [FromRoute]
        string userName
    )
    {
        var result = new UserNameDTO
        {
            UserName = userName
        };

        return Ok(result);
    }
}
