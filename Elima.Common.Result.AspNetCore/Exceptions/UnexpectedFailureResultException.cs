﻿using Elima.Common.Results;
using System;
using System.Collections.Generic;

namespace Elima.Common.Results.AspNetCore.Exceptions
{
    internal class UnexpectedFailureResultsException : Exception
    {
        public UnexpectedFailureResultsException(IEnumerable<ResultStatus> statuses)
        {
            UnexpectedStatuses = statuses;
        }

        public IEnumerable<ResultStatus> UnexpectedStatuses { get; }

        public override string ToString()
        {
            return $"ActionModel has [{nameof(ExpectedFailuresAttribute)}] with result statuses which are not configured in ResultConvention.";
        }
    }
}
