using System.Net;
using System.Text.Json;
using SmartInventorySystem.Responses;

namespace SmartInventorySystem.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            var response =
                new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string>
                    {
                        ex.InnerException?.Message
                        ?? "No inner exception"
                    }
                };

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}