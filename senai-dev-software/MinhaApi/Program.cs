using MinhaApi.Repositories;
using MinhaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONTROLLERS
// ========================================

builder.Services.AddControllers();

// ========================================
// CORS - PERMITE O REACT ACESSAR A API
// ========================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ========================================
// SWAGGER
// ========================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========================================
// REPOSITORY - PRODUTO
// ========================================

builder.Services.AddScoped<
    IProdutoRepository,
    ProdutoRepository>();

// ========================================
// SERVICE - PRODUTO
// ========================================

builder.Services.AddScoped<
    IProdutoService,
    ProdutoService>();

// ========================================
// REPOSITORY - CLIENTE
// ========================================

builder.Services.AddScoped<
    IClienteRepository,
    ClienteRepository>();

// ========================================
// SERVICE - CLIENTE
// ========================================

builder.Services.AddScoped<
    IClienteService,
    ClienteService>();

// ========================================
// REPOSITORY - VENDA
// ========================================

builder.Services.AddScoped<
    IVendaRepository,
    VendaRepository>();

// ========================================
// SERVICE - VENDA
// ========================================

builder.Services.AddScoped<
    IVendaService,
    VendaService>();

// ========================================
// CONSTRÓI A APLICAÇÃO
// ========================================

var app = builder.Build();

// ========================================
// SWAGGER
// ========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ========================================
// CORS
// ========================================

app.UseCors("ReactPolicy");

// ========================================
// CONTROLLERS
// ========================================

app.MapControllers();

// ========================================
// INICIA A API
// ========================================

app.Run();