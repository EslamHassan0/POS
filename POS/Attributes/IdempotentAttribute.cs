using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using POS.Application.Contract.Invoices;
using POS.Domain.Entity.Invoices;
using POS.Domain.Shared.Results;

namespace POS.API.Attributes
{
    internal sealed class IdempotentAttribute : Attribute, IAsyncActionFilter
    {
        private const int DefaultCacheTimeInMinutes = 60;
        private readonly TimeSpan _cacheDuration;
        public IdempotentAttribute(int cacheTimeInMinutes = DefaultCacheTimeInMinutes)
        {
            _cacheDuration = TimeSpan.FromMinutes(cacheTimeInMinutes);
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Parse the Idempotent-Key header from the request
            if (!context.HttpContext.Request.Headers.TryGetValue("idempotence-Key", out StringValues idempotenceKeyValues) ||
                !Guid.TryParse(idempotenceKeyValues, out Guid idempotenceKey))
            {
                context.Result = new BadRequestObjectResult("Invalid or missing Idempotence-Key header");
                return;
            }
            IDistributedCache cache =  context.HttpContext.RequestServices.GetService<IDistributedCache>();
            //check if we already processed this request and return a cached response (if it exists)

            string cacheKey = $"Idempotent_{idempotenceKey}";
            string? cacheResult = await cache.GetStringAsync(cacheKey);

            if (cacheResult is not null)
            {
                IdempotentResponse response = JsonSerializer.Deserialize<IdempotentResponse>(cacheResult);
                var result = new ObjectResult(response.Value) { StatusCode = response.StatusCode };
                context.Result = result;

                return;
            }
            //Execute the request and cache the response for the specified duratio

            ActionExecutedContext executedContext = await next();
            if (executedContext.Result is ObjectResult objectResult)
            {
                //bool isHttpSuccess = objectResult.StatusCode is >= 200 and < 300;

                if(objectResult.Value is Result<InvoiceDto> result)
                {
                    if(result.IsSuccess)
                    {
                        int statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
                        IdempotentResponse response = new(statusCode, objectResult.Value);

                        await cache.SetStringAsync(
                            cacheKey,
                            JsonSerializer.Serialize(response),
                            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheDuration }
                        );
                    }
                }
            }

        }

        internal sealed class IdempotentResponse
        {
            [JsonConstructor]
            public IdempotentResponse(int statusCode, object? value)
            {
                StatusCode = statusCode;
                Value = value;
            }

            public int StatusCode { get; }
            public object? Value { get; }
        }
    }
}
