using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;
        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("##### Executando #####");
            _logger.LogInformation("###########");
            _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
            _logger.LogInformation($"Model State: {context.ModelState.IsValid}");
            _logger.LogInformation("#########");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("##### Finalizado #####");
            _logger.LogInformation("###########");
            _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
            _logger.LogInformation("#########");

        }
    }
}
