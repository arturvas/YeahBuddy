using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YeahBuddy.Communication.Responses;
using YeahBuddy.Exceptions;
using YeahBuddy.Exceptions.ExceptionsBase;

namespace YeahBuddy.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is YeahBuddyException)
            HandleProjectException(context);
        else
            ThrowUnknownException(context);
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        if (context.Exception is not ErrorOnValidationException exception) return;

        context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMessages));
    }

    private static void ThrowUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourcesMessagesException.UNKNOWN_ERROR));
    }
}