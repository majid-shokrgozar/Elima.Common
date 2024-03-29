﻿using System;

namespace Elima.Common.Application;

/// <summary>
/// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
/// </summary>
[Serializable]
public record BasePagedAndSortedResultRequestDto : BasePagedResultRequestDto, IPagedAndSortedResultRequest
{
    public virtual string? Sorting { get; set; }
}
