using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AzureWorkshop
{
    // This class is based on the Github Gist at https://gist.github.com/rocklan/8a20c52431efe083603f9f0f2a18e6f3,
    // updated to work with dotnet core.
    public class ThrottleFilter : ActionFilterAttribute
    {
        private Throttler _throttler;
        private string _throttleGroup;

        public ThrottleFilter(
            int RequestLimit = 5,
            int TimeoutInSeconds = 10,
            [CallerMemberName] string ThrottleGroup = null)
        {
            _throttleGroup = ThrottleGroup;
            _throttler = new Throttler(ThrottleGroup, RequestLimit, TimeoutInSeconds);
        }

        public override void OnActionExecuting( ActionExecutingContext actionContext ) 
        {
            setIdentityAsThrottleGroup(actionContext.HttpContext);

            if (_throttler.RequestShouldBeThrottled)
            {
            
                actionContext.HttpContext.Response.StatusCode = 429;
                
                actionContext.Result = new ContentResult
                {
                    Content = "Too many requests"
                };

                addThrottleHeaders(actionContext.HttpContext.Response);
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted( ActionExecutedContext actionContext ) 
        {
            setIdentityAsThrottleGroup(actionContext.HttpContext);
            if (actionContext.Exception == null) _throttler.IncrementRequestCount();
            addThrottleHeaders(actionContext.HttpContext.Response);
            base.OnActionExecuted(actionContext);
        }

        private void setIdentityAsThrottleGroup(HttpContext httpContext)
        {
            if (_throttleGroup == "identity")
                _throttler.ThrottleGroup = httpContext.User.Identity.Name;

            if (_throttleGroup == "ipaddress")
                _throttler.ThrottleGroup = httpContext.Connection.RemoteIpAddress.ToString();
        }

        private void addThrottleHeaders(HttpResponse response)
        {
            if (response == null) return;

            foreach (var header in _throttler.GetRateLimitHeaders())
                response.Headers.Add(header.Key, header.Value);
        }
    }
}