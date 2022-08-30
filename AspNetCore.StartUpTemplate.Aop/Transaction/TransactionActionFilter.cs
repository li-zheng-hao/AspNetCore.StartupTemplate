using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCore.StartUpTemplate.AOP;

public class TransactionActionFilter:IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}