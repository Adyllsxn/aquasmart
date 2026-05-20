namespace Aquasmart.Server.Source.Presentation.Enpoints;
public interface IEndpoint
{
    static abstract void MapEndpoint(IEndpointRouteBuilder app);
}

