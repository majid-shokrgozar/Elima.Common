using Elima.Common.Application.MediatR.Commands;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Results;
using Elima.Common.WebApiSample.Domain.Entity;
using Elima.Common.WebApiSample.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Elima.Common.WebApiSample.Application.Sample.Delete;

public class DeleteCommandHandler : ICommandHandler<DeleteCommand>
{
    private readonly SampleDataContext _sampleDataContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCommandHandler(SampleDataContext sampleDataContext, IUnitOfWork unitOfWork)
    {
        _sampleDataContext = sampleDataContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        SampleModel? entity = await _sampleDataContext.Samples.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Result.NotFound(request.Id);

        _sampleDataContext.Samples.Remove(entity);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
