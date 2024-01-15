using Elima.Common.Results.AspNetCore;
using Elima.Common.WebApiSample.Application.Sample.CreateNew;
using Elima.Common.WebApiSample.Application.Sample.Delete;
using Elima.Common.WebApiSample.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elima.Common.WebApiSample.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly ISender _sender;

    public SampleController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost()]
    public async Task<ActionResult<SampleModel>> PostAsync(CreateSampleCommand createSampleCommand,CancellationToken cancellationToken)
    {
        var result = await _sender.Send(createSampleCommand, cancellationToken);
        //return Ok(result);
        return result.ToActionResult(this);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteCommand(id), cancellationToken);
        return result.ToActionResult(this);
    }
}
