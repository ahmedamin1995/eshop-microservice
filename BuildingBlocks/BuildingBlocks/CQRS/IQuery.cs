using MediatR;

namespace BuildingBlocks.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}


public interface IQueryHandler<in IQuery, TResponse> :IRequestHandler<IQuery, TResponse>
    where IQuery : IQuery<TResponse>
    where TResponse : notnull
{
    
}
