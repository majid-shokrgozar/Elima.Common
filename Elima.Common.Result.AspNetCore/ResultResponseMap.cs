using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Elima.Common.Results.AspNetCore
{
    public static class ResultResponseMap
    {
        public static (int statusCode, object? responseBody) Map(ControllerBase controller, IResult result)
        {
            return result.Status switch
            {
                ResultStatus.BadRequest => GenerateResult(result, BadRequest(controller, result)),
                ResultStatus.Unauthorized => GenerateResult(result, Unauthorized(controller, result)),
                ResultStatus.Forbidden => GenerateResult(result, Forbidden(controller, result)),
                ResultStatus.NotFound => GenerateResult(result, NotFoundEntity(controller, result)),
                ResultStatus.Conflict => GenerateResult(result, ConflictEntity(controller, result)),
                ResultStatus.Failed => GenerateResult(result, CriticalEntity(controller, result)),
                ResultStatus.NotImplemented => GenerateResult(result, NotImplemented(controller, result)),
                ResultStatus.Unprocessable => GenerateResult(result, UnprocessableEntity(controller, result)),
                _ => ((int)HttpStatusCode.NoContent, null),
            };
        }

        private static (int statusCode, object responseBody) GenerateResult(IResult result, object mapper)
        {
            return ((int)result.Status, mapper);
        }

        private static ValidationProblemDetails BadRequest(ControllerBase controller, IResult result)
        {
            foreach (var error in result.ValidationError)
            {
                controller.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return new ValidationProblemDetails(controller.ModelState);
        }

        private static ExtendedProblemDetails UnprocessableEntity(ControllerBase controller, IResult result)
        {
            return ConvertResultToProblemDetails(result, "Something went wrong.", "", "");
        }

        private static ExtendedProblemDetails NotFoundEntity(ControllerBase controller, IResult result)
        {
            return ConvertResultToProblemDetails(result, "Resource not found.", "", "");
        }

        private static ExtendedProblemDetails ConflictEntity(ControllerBase controller, IResult result)
        {
            return ConvertResultToProblemDetails(result, "There was a conflict.", "", "");
        }

        private static ExtendedProblemDetails CriticalEntity(ControllerBase controller, IResult result)
        {
            return ConvertResultToProblemDetails(result, "Something went wrong.", "", "");
        }

        private static ExtendedProblemDetails Unauthorized(ControllerBase controller, IResult result)
        {
            return ConvertResultToProblemDetails(result, "full authentication is required to access this resource.", "", "");
        }

        private static ExtendedProblemDetails Forbidden(ControllerBase controller, IResult result)
        {
            return ConvertResultToProblemDetails(result, "you don't have permission to access this resource.", "", "");
        }

        private static ExtendedProblemDetails NotImplemented(ControllerBase controller, IResult result)
        {
            return ConvertResultToProblemDetails(result, "Service is not implemented yet.", "", "");
        }

        private static ExtendedProblemDetails ConvertResultToProblemDetails(
            IResult result,
            string defaultMessage,
            string instanceUrl,
            string typeUrl)
        {
            return new ExtendedProblemDetails
            {
                Title = result.Errors.Count > 1 || result.Error == null ? defaultMessage : result.Error?.Message,
                Instance = instanceUrl,
                Type = typeUrl,
                Status = (int)result.Status,
                Detail = result.Errors
                               .Select(error => new ExtendedProblemDetail()
                               {
                                   Code = error.Code,
                                   Data = error.Data,
                                   Description = error.Message,
                                   Message = error.Message
                               })
                               .ToList()
            };
        }
    }
}
