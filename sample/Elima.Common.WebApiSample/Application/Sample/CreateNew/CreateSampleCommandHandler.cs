using Elima.Common.Application.MediatR.Commands;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Results;
using Elima.Common.WebApiSample.Domain.Entity;
using Elima.Common.WebApiSample.Infrastructure.Data;

namespace Elima.Common.WebApiSample.Application.Sample.CreateNew;

public class CreateSampleCommandHandler : ICommandHandler<CreateSampleCommand, SampleModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SampleDataContext _sampleDataContext;

    public CreateSampleCommandHandler(IUnitOfWork unitOfWork, SampleDataContext sampleDataContext)
    {
        _unitOfWork = unitOfWork;
        _sampleDataContext = sampleDataContext;
    }

    public async Task<Result<SampleModel>> Handle(CreateSampleCommand request, CancellationToken cancellationToken)
    {
        var sample = new SampleModel() { Titile = request.Title };

        await _sampleDataContext.Samples.AddAsync(sample);

        if (request.Title.Equals("Test", StringComparison.InvariantCultureIgnoreCase))
            //await _unitOfWork.RollbackAsync(cancellationToken);
            return Result<SampleModel>.Conflict();
        else
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        return sample;
    }
}
