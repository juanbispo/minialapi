using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TarefasDB"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World");

app.MapGet("/tarefas", async (AppDbContext _context) =>
    {
        return await _context.Tarefas.ToListAsync();
    });

app.MapGet("/tarefas/{id}", async (AppDbContext _context, int id) =>
{
    return await _context.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound();
});

app.MapGet("/tarefas/concluidas", async (AppDbContext _context) =>
{
    return await _context.Tarefas.Where(t => t.IsConcluida == true).ToListAsync();
});

app.MapPost("/tarefas", async (AppDbContext _context, Tarefa tarefa) =>
{
    _context.Add(tarefa);
    await _context.SaveChangesAsync();

    return Results.Created($"/tarefas/{tarefa.Id}",tarefa);

});

app.MapPut("tarefas/{id}", async(AppDbContext _context, Tarefa inputTarefa, int id) =>
{
    var tarefa = await _context.Tarefas.FindAsync(id);

    if (tarefa is null)
    {
        return Results.NotFound();
    }

    tarefa.Name = inputTarefa.Name;
    tarefa.IsConcluida = inputTarefa.IsConcluida;

    await _context.SaveChangesAsync();

    return Results.NoContent();

});

app.MapDelete("tarefas/{id}", async (AppDbContext _context, int id) =>
{
    var tarefa = await _context.Tarefas.FindAsync(id);

    if(tarefa is null)
    {
       return Results.NotFound();
    }

    _context.Tarefas.Remove(tarefa);

    await _context.SaveChangesAsync();

    return Results.Ok(tarefa);
});


app.Run();

