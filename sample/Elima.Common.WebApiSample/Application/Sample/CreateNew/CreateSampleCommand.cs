using Elima.Common.Application.MediatR.Commands;
using Elima.Common.Results;
using Elima.Common.WebApiSample.Domain.Entity;

namespace Elima.Common.WebApiSample.Application.Sample.CreateNew;

public record CreateSampleCommand(string Title) : ICommand<SampleModel>;

