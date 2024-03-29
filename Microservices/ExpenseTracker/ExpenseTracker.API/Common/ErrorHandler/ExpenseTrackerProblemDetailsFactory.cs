﻿using ExpenseTracker.API.Common.Http;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace ExpenseTracker.API.Common.ErrorHandler
{
    public sealed class ExpenseTrackerProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options;

        public ExpenseTrackerProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public override ProblemDetails CreateProblemDetails
          (HttpContext httpContext,
          int? statusCode = null,
          string? title = null,
          string? type = null,
          string? detail = null,
          string? instance = null)
        {
            statusCode ??= 500;

            var problemDetails = new ProblemDetails()
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            if (title != null)
            {
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status ??= statusCode;

            if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                if (string.IsNullOrEmpty(problemDetails.Title)) 
                {
                    problemDetails.Title = clientErrorData.Title;
                }
                problemDetails.Type ??= clientErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
            if (traceId != null)
            {
                problemDetails.Extensions[HttpContextItemKeys.TraceId] = traceId;
            }

            var error = httpContext?.Items[HttpContextItemKeys.Errors] as Error;
            if (error is null)
                return;

            if (error.Metadata.Any())
            {
                var errorDict = new Dictionary<string, object>();
                foreach ( var item in error.Metadata )
                {
                    errorDict.Add(item.Key, new List<object> { item.Value });
                }
                problemDetails.Extensions.Add(
                    HttpContextItemKeys.Errors,
                    errorDict);
            }
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
                HttpContext httpContext,
                ModelStateDictionary modelStateDictionary,
                int? statusCode = null,
                string? title = null,
                string? type = null,
                string? detail = null,
                string? instance = null)
        {
            ArgumentNullException.ThrowIfNull(modelStateDictionary);

            statusCode ??= 400;

            var problemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            if (title != null)
            {
                // For validation problem details, don't overwrite the default title with null.
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }
    }
}
