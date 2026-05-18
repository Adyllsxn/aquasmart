var builder = DistributedApplication.CreateBuilder(args);

// Adiciona o Server (backend)
var server = builder.AddProject<Projects.Aquasmart_Server>("server")
    .WithExternalHttpEndpoints();

// Adiciona o Client Blazor WASM
builder.AddProject<Projects.Aquasmart_Web>("web")
    .WithReference(server)
    .WithExternalHttpEndpoints();

builder.Build().Run();