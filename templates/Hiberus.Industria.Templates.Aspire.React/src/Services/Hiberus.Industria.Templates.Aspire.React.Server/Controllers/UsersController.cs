using FluentResults;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Identity;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.CreateUser;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.DeleteUser;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.ResetUserPassword;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Commands.UpdateUser;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Dtos;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Queries.GetUserById;
using Hiberus.Industria.Templates.Aspire.React.Server.Application.Users.Queries.GetUsers;
using Hiberus.Industria.Templates.Aspire.React.Server.Domain.Common.Dtos;
using Hiberus.Industria.Templates.Aspire.React.Server.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hiberus.Industria.Templates.Aspire.React.Server.Controllers;

/// <summary>
/// Handles HTTP requests for user-related operations.
/// </summary>
[Route("users")]
[ApiController]
[Authorize(PolicyNames.HasAdministratorRole)]
public class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR mediator.</param>
    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of users.
    /// </summary>
    /// <param name="page">The page number (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="group">An optional list of group filters to apply when retrieving users.</param>
    /// <param name="username">An optional username filter to apply when retrieving users.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of users.</returns>
    /// <response code="200">Returns the paginated list of users.</response>
    /// <response code="400">Invalid pagination parameters.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUsersAsync(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromQuery] IEnumerable<string> group,
        [FromQuery] string username = "",
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetUsersQuery(page, pageSize, group, username);
        Result<PagedResultDto<UserDto>> result = await this
            .mediator.Send(query, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return this.Ok(result.Value);
        }

        return result.ToActionResult(this);
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user details.</returns>
    /// <response code="200">Returns the user details.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserByIdAsync(
        [FromRoute] long id,
        CancellationToken cancellationToken = default
    )
    {
        var query = new GetUserByIdQuery(id);
        Result<UserDto> result = await this
            .mediator.Send(query, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return this.Ok(result.Value);
        }

        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userRequest">The user information to create.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created user details.</returns>
    /// <response code="201">Returns the created user details.</response>
    /// <response code="400">Invalid user information provided.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUserAsync(
        [FromBody] CreateUserRequestDto userRequest,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userRequest);

        var command = new CreateUserCommand(userRequest);
        Result<UserDto> result = await this
            .mediator.Send(command, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            UserDto user = result.Value;
            Uri locationUri = new($"users/{user.Id}", UriKind.Relative);
            return this.Created(locationUri, user);
        }

        return result.ToActionResult(this);
    }

    /// <summary>
    /// Updates a user's information.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="userRequest">The user information to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated user details.</returns>
    /// <response code="200">Returns the updated user details.</response>
    /// <response code="400">Invalid user information provided.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUserAsync(
        [FromRoute] long id,
        [FromBody] UpdateUserRequestDto userRequest,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userRequest);

        var command = new UpdateUserCommand(id, userRequest);
        Result<UserDto> result = await this
            .mediator.Send(command, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return this.Ok(result.Value);
        }

        return result.ToActionResult(this);
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if the deletion was successful.</returns>
    /// <response code="204">User deleted successfully.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUserAsync(
        [FromRoute] long id,
        CancellationToken cancellationToken = default
    )
    {
        var command = new DeleteUserCommand(id);
        Result<Unit> result = await this
            .mediator.Send(command, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return this.NoContent();
        }

        return result.ToActionResult(this);
    }

    /// <summary>
    /// Resets the password of a user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>No content if the password reset was successful.</returns>
    /// <response code="204">Password reset successfully.</response>
    /// <response code="404">User not found.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost("{id}/reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetUserPasswordAsync(
        [FromRoute] long id,
        CancellationToken cancellationToken = default
    )
    {
        var command = new ResetUserPasswordCommand(id);
        Result<Unit> result = await this
            .mediator.Send(command, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            return this.NoContent();
        }

        return result.ToActionResult(this);
    }
}
