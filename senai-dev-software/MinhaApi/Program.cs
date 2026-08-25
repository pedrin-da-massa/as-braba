using MinhaApi.Repositories;
using MinhaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os Controllers
builder.Services.AddControllers();

// Adiciona suporte ao Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra o Repository
builder.Services.AddScoped<
    IProdutoRepository,
    ProdutoRepository>();

// Registra a Service
builder.Services.AddScoped<
    IProdutoService,
    ProdutoService>();

var app = builder.Build();

// Configuração do Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Permite o uso dos Controllers
app.MapControllers();

// Inicia a aplicação
app.Run();