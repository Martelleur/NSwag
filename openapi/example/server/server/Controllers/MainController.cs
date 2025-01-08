using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Patterns;
using server.Constants;
using server.DTO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace server.Controllers;

/// <summary>
/// Controller class used for endpoint <see cref="RoutePath.Main"/>.
/// </summary>
[ApiController]
[Route(RoutePath.Main)]
[ApiVersion(Api.MAJOR_VERSION_ONE)]
[ApiVersion(Api.MAJOR_VERSION_TWO)]
[Produces("application/json")]
public class MainController : ControllerBase
{

    /// <summary>
    /// Hello world end point.
    /// </summary>
    /// <returns>A <see cref="HelloWorldDTO"/></returns>
    /// <response code="200">With the hello world message</response>
    [HttpGet("hello-world", Name = "HelloWorld")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<UserNameDTO> HelloWorld()
    {
        var result = new HelloWorldDTO
        {
            Value = "Hello world"
        };

        return Ok(result);
    }

    /// <summary>
    /// A ping end point.
    /// </summary>
    /// <returns>A <see cref="PingDTO"/></returns>
    /// <response code="200">With the hello world message</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("ping")]
    public ActionResult<PingDTO> Ping()
    {
        var result = new PingDTO
        {
            Value = "Pong"
        };

        return Ok(result);
    }
}
