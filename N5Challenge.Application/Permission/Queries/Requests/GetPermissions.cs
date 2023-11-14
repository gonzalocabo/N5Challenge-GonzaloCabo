using MediatR;

namespace N5Challenge.Application.Permission.Queries.Requests;

public class GetPermissions : IRequest<IEnumerable<Domain.Entities.Permissions.Permission>> { }
