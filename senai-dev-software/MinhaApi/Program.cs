using MinhaApi.Repositories;
using MinhaApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

// ===============================
// REPOSITORY - VENDA
// ===============================

builder.Services.AddScoped<
    IVendaRepository,
    VendaRepository>();

// ===============================
// SERVICE - VENDA
// ===============================

builder.Services.AddScoped<
    IVendaService,
    VendaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
