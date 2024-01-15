using System;
using System.Net;
using System.Text;
using Elima.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Elima.Common.Results.AspNetCore
{
    /// <summary>
    /// Extensions to support converting Result to an ActionResult
    /// </summary>
    public static partial class ResultExtensions
    {
        /// <summary>
        /// Convert a <see cref="Result{T}"/> to a <see cref="ActionResult"/>
        /// </summary>
        /// <typeparam name="T">The value being returned</typeparam>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result"/> to a <see cref="ActionResult"/>
        /// </summary>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult ToActionResult(this Result result, ControllerBase controller)
        {
            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result{T}"/> to a <see cref="ActionResult"/>
        /// </summary>
        /// <typeparam name="T">The value being returned</typeparam>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller,
            Result<T> result)
        {
            return controller.ToActionResult((IResult)result);
        }

        /// <summary>
        /// Convert a <see cref="Result"/> to a <see cref="ActionResult"/>
        /// </summary>
        /// <param name="controller">The controller this is called from</param>
        /// <param name="result">The Result to convert to an ActionResult</param>
        /// <returns></returns>
        public static ActionResult ToActionResult(this ControllerBase controller,
            Result result)
        {
            return controller.ToActionResult((IResult)result);
        }

        internal static ActionResult ToActionResult(this ControllerBase controller, IResult result)
        {
            var actionProps = controller.ControllerContext.ActionDescriptor.Properties;

            var (statusCode, responseBody) = ResultResponseMap.Map(controller, result);

            switch (result.Status)
            {
                case ResultStatus.Succeeded:
                    if (result is IResultWithValue resultWithValue)
                    {
                        return controller.StatusCode((int)result.Status, resultWithValue.GetValue());
                    }
                    return controller.StatusCode((int)result.Status);
                default:
                    return responseBody == null
                        ? controller.StatusCode(statusCode)
                        : controller.StatusCode(statusCode, responseBody);
            }
        }
    }
}
