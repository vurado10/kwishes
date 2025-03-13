using MediatR;

namespace KWishes.Core.Application.Misc.Results;

public interface IResultRequestHandler<in TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IResultRequest<TResponse>
{
}