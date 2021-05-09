using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Tragate.Common.Library.Aspects
{
    public class ExceptionHandler : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context){
            if (context.Exception is AggregateException exception){
                foreach (var item in exception.InnerExceptions){
                    Log.Error($"{item}");
                }
            }
            else{
                Log.Error($"{context.Exception}");
            }
        }
    }
}