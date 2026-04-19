using MediatR;

namespace NexoraERP.Application.Identity.Commands.Login;

public sealed record LoginCommand(string UserName, string Password) : IRequest<LoginResponse?>;
