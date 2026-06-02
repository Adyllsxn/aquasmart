
var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// endpoints reais do teu sistema
app.MapEndpoints();

// teste simples
app.MapGet("/", () => "AquaSmart OK");

app.Run();