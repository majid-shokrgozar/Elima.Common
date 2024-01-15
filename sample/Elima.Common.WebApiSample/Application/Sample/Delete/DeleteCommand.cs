using Elima.Common.Application.MediatR.Commands;

namespace Elima.Common.WebApiSample.Application.Sample.Delete;

public record DeleteCommand(Guid Id):ICommand;
