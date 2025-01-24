// ----------------------------------------------------------------------------
// Developer:      Ismail Hamzah
// Email:         go2ismail@gmail.com
// ----------------------------------------------------------------------------

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Authentication;

namespace WebAPI.Common.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        this._logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext, IExceptionHandler customExceptionHandler)
    {
        try
        {
            _logger.LogInformation($"excuting url: {httpContext.Request.GetDisplayUrl()}");
            await _next(httpContext);
            _logger.LogInformation($"excuted url: {httpContext.Request.GetDisplayUrl()}");
            if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await customExceptionHandler.TryHandleAsync(httpContext, new UnauthorizedAccessException("Unauthorized - Token missing or invalid"), CancellationToken.None);
            }
            else if (httpContext.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await customExceptionHandler.TryHandleAsync(httpContext, new Exception("Forbidden - Access denied"), CancellationToken.None);
            }
        }
        catch (SecurityTokenExpiredException ex)
        {
            await customExceptionHandler.TryHandleAsync(httpContext, new SecurityTokenExpiredException("Token expired", ex), CancellationToken.None);
        }
        catch (AuthenticationException ex)
        {
            await customExceptionHandler.TryHandleAsync(httpContext, new AuthenticationException("Authentication failed", ex), CancellationToken.None);
        }
        catch (InvalidOperationException ex)
        {
            await customExceptionHandler.TryHandleAsync(httpContext, new InvalidOperationException("Invalid operation", ex), CancellationToken.None);
        }
        catch (UnauthorizedAccessException ex)
        {
            await customExceptionHandler.TryHandleAsync(httpContext, new UnauthorizedAccessException("Unauthorized access", ex), CancellationToken.None);
        }
        catch (Exception ex)
        {
            await customExceptionHandler.TryHandleAsync(httpContext, ex, CancellationToken.None);
        }
    }


}

