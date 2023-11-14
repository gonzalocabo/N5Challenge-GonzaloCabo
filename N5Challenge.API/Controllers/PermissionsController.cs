using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5Challenge.API.DTO;
using N5Challenge.Application.Permission.Commands.Requests;
using N5Challenge.Application.Permission.Exceptions;
using N5Challenge.Application.Permission.Queries.Requests;
using N5Challenge.Domain.Entities.Permissions;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace N5Challenge.API.Controllers
{
    [Route("[controller]"), ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public PermissionsController(IMediator mediator, Serilog.ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(logger);

            _mediator = mediator;
            _logger = logger;

            if(httpContextAccessor?.HttpContext is not null)
                _logger.Information($"{httpContextAccessor.HttpContext!.Request.Method}: {httpContextAccessor.HttpContext.Request.RouteValues["action"]}");
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns all permissions", typeof(Permission))]
        public async Task<IEnumerable<Permission>> GetPermissions(CancellationToken cancellationToken)
        {
            try
            {
                return await _mediator.Send(new GetPermissions(), cancellationToken);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error while getting permissions");
                throw;
            }
        }

        [HttpPost("Request")]
        [SwaggerResponse(StatusCodes.Status201Created, "The permission has been created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Check request parameters", typeof(ErrorMessageObject))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The permission type was not found", typeof(ErrorMessageObject))]
        public async Task<IActionResult> RequestPermission([FromBody] CreatePermissionDTO createDTO, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createDTO.EmployeeSurname) || string.IsNullOrWhiteSpace(createDTO.EmployeeForename) || createDTO.PermissionType <= 0)
                    throw new StatusCodeException((int)HttpStatusCode.BadRequest, "Bad request to perform the operation");

                await _mediator.Send(new CreatePermission()
                {
                    EmployeeForename = createDTO.EmployeeForename,
                    EmployeeSurname = createDTO.EmployeeSurname,
                    PermissionType = createDTO.PermissionType
                }, cancellationToken);

                return StatusCode((int)HttpStatusCode.Created);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Error while requesting permission");
                
                if(ex is StatusCodeException stEx)
                    return StatusCode(stEx.StatusCode, stEx.ErrorMessageObject);
                
                throw;
            }
        }

        [HttpPut("Modify/{permission_id:int:min(1)}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The permission has modified")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Check request parameters", typeof(ErrorMessageObject))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "The permission or the permission type was not found", typeof(ErrorMessageObject))]
        public async Task<IActionResult> ModifyPermission(int permission_id, [FromBody] UpdatePermissionDTO updateDTO, CancellationToken cancellationToken)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(updateDTO.EmployeeSurname) && string.IsNullOrWhiteSpace(updateDTO.EmployeeForename) && !updateDTO.PermissionType.HasValue)
                    throw new StatusCodeException((int)HttpStatusCode.BadRequest, "Bad request to perform the operation");
                
                await _mediator.Send(new ModifyPermission()
                {
                    PermissionId = permission_id,
                    EmployeeForename = updateDTO.EmployeeForename,
                    EmployeeSurname = updateDTO.EmployeeSurname,
                    PermissionType = updateDTO.PermissionType
                }, cancellationToken);

                return NoContent();
            }
            catch (StatusCodeException ex)
            {
                _logger.Error(ex, "Error while modifing permission");

                if (ex is StatusCodeException stEx)
                    return StatusCode(stEx.StatusCode, stEx.ErrorMessageObject);

                throw;
            }
        }
    }
}
