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
[ApiVersion(Api.MAJOR_VERSION_TWO)]
[Route(RoutePath.Test)]
[Produces("application/json")]
public class TestControllerVersionTwo : ControllerBase
{

    /// <summary>
    /// Used to ping a valid user name.
    /// </summary>
    /// <param name="userName">A <see cref="string"/></param>
    /// <returns>A <see cref="UserNameDTO"/></returns>
    /// <response code="200">With the user name</response>
    [HttpGet("{userName}/ping", Name = "PingUserNameV2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<UserNameDTO> PingUserName(
        [FromRoute]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "The user name format is invalid.")]
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
