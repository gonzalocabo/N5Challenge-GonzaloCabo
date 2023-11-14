using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using N5Challenge.API.Controllers;
using N5Challenge.API.DTO;
using N5Challenge.Application.Permission.Commands.Requests;
using N5Challenge.Application.Permission.Exceptions;
using N5Challenge.Application.Permission.Queries.Requests;
using N5Challenge.Domain.Entities.Permissions;
using System.Net;

namespace N5Challenge.Tests;

[TestClass]
public class UnitTests
{
    private Mock<IMediator> _mediatorMock;
    private PermissionsController _permissionsController;
    
    [TestInitialize]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _permissionsController = new PermissionsController(_mediatorMock.Object, new Mock<Serilog.ILogger>().Object, new Mock<IHttpContextAccessor>().Object);
    }

    [TestMethod]
    public async Task GetPermissions_ReturnsPermissionsList()
    {
        // Arrange
        var permissions = new List<Permission>
        {
            new Permission 
            { 
                Id = 1,
                EmployeeForename = "Gonzalo",
                EmployeeSurname = "Cabo",
                PermissionDate = DateTime.Now,
                PermissionTypeId = 1,
                PermissionType = new PermissionType
                {
                    Id = 1,
                    Description = "Administrator"
                }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPermissions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(permissions);

        // Act
        var result = await _permissionsController.GetPermissions(CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        
        Assert.IsInstanceOfType(result, typeof(IEnumerable<Permission>));

        Assert.AreEqual(permissions.Count, result!.Count());
    }

    [TestMethod]
    public async Task ModifyPermission_ReturnsNoContent()
    {
        // Arrange
        var permissionDto = new UpdatePermissionDTO() { EmployeeForename = "Gonzalo" };

        // Act
        var result = await _permissionsController.ModifyPermission(1, permissionDto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        
        Assert.IsInstanceOfType(result, typeof(StatusCodeResult));

        var statusCodeResult = result as StatusCodeResult;
        Assert.AreEqual((int)HttpStatusCode.NoContent, statusCodeResult?.StatusCode);
    }

    [TestMethod]
    public async Task ModifyPermission_NotFoundPermission_ReturnsNotFound()
    {
        // Arrange
        var permissionDto = new UpdatePermissionDTO() { EmployeeForename = "Gonzalo" };

        _mediatorMock.Setup(m => m.Send(It.IsAny<ModifyPermission>(), It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                throw new StatusCodeException((int)HttpStatusCode.NotFound, string.Empty);
            });

        // Act
        var result = await _permissionsController.ModifyPermission(1, permissionDto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(ObjectResult));

        var statusCodeResult = result as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.NotFound, statusCodeResult?.StatusCode);
    }

    [TestMethod]
    public async Task ModifyPermission_BadParameters_ReturnsBadRequest()
    {
        // Arrange
        var permissionDto = new UpdatePermissionDTO() { };

        // Act
        var result = await _permissionsController.ModifyPermission(1, permissionDto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(ObjectResult));

        var statusCodeResult = result as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult?.StatusCode);
    }

    [TestMethod]
    public async Task RequestPermission_ReturnsCreated()
    {
        // Arrange
        var permissionDto = new CreatePermissionDTO() { 
            EmployeeForename = "Gonzalo",
            EmployeeSurname = "Cabo",
            PermissionType = 1
        };

        // Act
        var result = await _permissionsController.RequestPermission(permissionDto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(StatusCodeResult));

        var statusCodeResult = result as StatusCodeResult;
        Assert.AreEqual((int)HttpStatusCode.Created, statusCodeResult!.StatusCode);
    }

    [TestMethod]
    public async Task RequestPermission_NotFoundPermissionType_ReturnsNotFound()
    {
        // Arrange
        var permissionDto = new CreatePermissionDTO()
        {
            EmployeeForename = "Gonzalo",
            EmployeeSurname = "Cabo",
            PermissionType = 1
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreatePermission>(), It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                throw new StatusCodeException((int)HttpStatusCode.NotFound, string.Empty);
            });

        // Act
        var result = await _permissionsController.RequestPermission(permissionDto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(ObjectResult));

        var statusCodeResult = result as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.NotFound, statusCodeResult!.StatusCode);
    }

    [TestMethod]
    public async Task RequestPermission_BadParameters_ReturnsBadRequest()
    {
        // Arrange
        var permissionDto = new CreatePermissionDTO() { };

        // Act
        var result = await _permissionsController.RequestPermission(permissionDto, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);

        Assert.IsInstanceOfType(result, typeof(ObjectResult));

        var statusCodeResult = result as ObjectResult;
        Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult?.StatusCode);
    }

}