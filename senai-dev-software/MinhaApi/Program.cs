using MinhaApi.Repositories;
using MinhaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os Controllers
builder.Services.AddControllers();

// Adiciona suporte ao Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===============================
// REPOSITORY - PRODUTO
// ===============================

builder.Services.AddScoped<
    IProdutoRepository,
    ProdutoRepository>();

// ===============================
// SERVICE - PRODUTO
// ===============================

builder.Services.AddScoped<
    IProdutoService,
    ProdutoService>();

// ===============================
// REPOSITORY - CLIENTE
// ===============================

builder.Services.AddScoped<
    IClienteRepository,
    ClienteRepository>();

// ===============================
// SERVICE - CLIENTE
// ===============================

builder.Services.AddScoped<
    IClienteService,
    ClienteService>();

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
