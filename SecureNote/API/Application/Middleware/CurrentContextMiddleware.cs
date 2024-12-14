using API.Core.Domain.Context;

namespace API.Application.Middleware;

/// <summary>
/// A middleware that wraps the current <see cref="HttpContext"/> in a <see cref="CurrentContext"/> object.
/// </summary>
public class CurrentContextMiddleware {
    private readonly RequestDelegate _next;

    public CurrentContextMiddleware(RequestDelegate next) {
        _next = next;
    }

    /// <summary>
    /// Invokes the current context middleware.
    /// </summary>
    /// <param name="httpContext">The HTTP context received from the Http Request.</param>
    /// <param name="currentContext">The current context to build.</param>
    public async Task Invoke(HttpContext httpContext, CurrentContext currentContext) {
        currentContext.Build(httpContext);
        await _next.Invoke(httpContext);
    }
}
