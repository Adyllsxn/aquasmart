var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Aquasmart_Server>("server");
builder.AddProject<Projects.Aquasmart_Web>("web");

builder.Build().Run();