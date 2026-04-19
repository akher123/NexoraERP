using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NexoraERP.Domain.Accounting.Exceptions;

namespace NexoraERP.Infrastructure.Exceptions;

public sealed class AccountingRuleExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not AccountingRuleViolationException ex)
            return false;

        Trace.TraceWarning("Accounting rule violation: {0}", ex.Message);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(
            new
            {
                type = "https://httpstatuses.io/400",
                title = ex.Message,
                status = 400
            },
            cancellationToken);

        return true;
    }
}
