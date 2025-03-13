using Microsoft.EntityFrameworkCore;

namespace KWishes.Core.Application.Misc.Results;

public abstract class RepositoryRequestHandler<TRequest, TResponse> : IResultRequestHandler<TRequest, TResponse> 
    where TRequest : IResultRequest<TResponse>
{
    private readonly IDbContextFactory<RepositoryContext> dbContextFactory;

    protected RepositoryRequestHandler(IDbContextFactory<RepositoryContext> dbContextFactory)
    {
        this.dbContextFactory = dbContextFactory;
    }

    public async Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await Handle(context, request, cancellationToken);
    }

    protected abstract ValueTask<Result<TResponse>> Handle(
        RepositoryContext context,
        TRequest request,
        CancellationToken cancellationToken);
}