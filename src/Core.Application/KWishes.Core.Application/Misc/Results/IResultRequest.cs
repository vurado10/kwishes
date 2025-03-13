using MediatR;

namespace KWishes.Core.Application.Misc.Results;

public interface IResultRequest<TResultValue> : IRequest<Result<TResultValue>>
{
    
}