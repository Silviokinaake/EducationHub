using EducationHub.Conteudo.Application.Services;
using EducationHub.Conteudo.Data;
using EducationHub.Conteudo.Data.Repository;
using EducationHub.Conteudo.Domain.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//  Configurar DbContext (EF Core)
builder.Services.AddDbContext<ConteudoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependências (Repositories e Services)
builder.Services.AddScoped<ICursoRepositorio, CursoRepository>();
builder.Services.AddScoped<ICursoAppService, CursoAppService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//  Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//  Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
