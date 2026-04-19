using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NexoraERP.Application.Abstractions;
using NexoraERP.Domain.Enums;
using NexoraERP.Domain.Master;
using NexoraERP.Infrastructure.Configuration;
using NexoraERP.Infrastructure.Services;
using NexoraERP.Shared.Http;
using NexoraERP.Shared.MultiTenancy;

namespace NexoraERP.Infrastructure.Middleware;

public sealed class TenantResolutionMiddleware(
    RequestDelegate next,
    IOptions<MultiTenancyOptions> options)
{
    private readonly MultiTenancyOptions _options = options.Value;

    public async Task InvokeAsync(
        HttpContext context,
        TenantContext tenantContext,
        ITenantRegistryReader registryReader)
    {
        if (ShouldSkip(context.Request.Path))
        {
            await next(context);
            return;
        }

        tenantContext.Clear();

        TenantRegistration? tenant = null;

        if (context.Request.Headers.TryGetValue(TenantHttpHeaders.TenantId, out var headerValues))
        {
            var raw = headerValues.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(raw) && Guid.TryParse(raw, out var tenantId))
                tenant = await registryReader.GetByIdAsync(tenantId, context.RequestAborted);
        }

        if (tenant is null && _options.EnableSubdomainResolution)
        {
            var host = context.Request.Host.Host;
            var key = SubdomainParser.TryGetSubdomainKey(host, _options.BaseDomain);
            if (!string.IsNullOrEmpty(key))
                tenant = await registryReader.GetBySubdomainKeyAsync(key, context.RequestAborted);
        }

        if (tenant is null)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Tenant could not be resolved.");
            return;
        }

        if (tenant.Status != TenantStatus.Active)
        {
            await WriteProblemAsync(context, StatusCodes.Status403Forbidden, "Tenant is not active.");
            return;
        }

        tenantContext.SetTenant(tenant.Id, tenant.Name, tenant.ConnectionString);
        await next(context);
    }

    private bool ShouldSkip(PathString path)
    {
        foreach (var prefix in _options.ExcludedPathPrefixes)
        {
            if (path.StartsWithSegments(prefix, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(new
        {
            type = "https://httpstatuses.io/" + statusCode,
            title,
            status = statusCode
        });
    }
}
