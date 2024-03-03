using GameStore.Api.Entities;

const string GetGameEndPointName = "GetGame";
List<Game> games = new()
{
new Game()
{
    Id=1,
    Name="Street Fighter II",
    Genre="Fighting",
    Price=19.99M,
    ReleaseDate=new DateTime(1991, 2, 1),
    ImageUri="http://placeholder.co/100"
},
new Game()
{
    Id=2,
    Name="Final Fantasy XIV",
    Genre="Roleplaying",
    Price=59.99M,
    ReleaseDate=new DateTime(2021, 12, 1),
    ImageUri="http://placeholder.co/100"
},
new Game()
{
    Id=3,
    Name="FIFA 23",
    Genre="Sport",
    Price=69.99M,
    ReleaseDate=new DateTime(2023, 12, 27),
    ImageUri="http://placeholder.co/100"
},
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var group = app.MapGroup("/game");

group.MapGet("/", () => games);


group.MapGet("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);
    if (game is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(game);
})
.WithName(GetGameEndPointName);


group.MapPost("/", (Game game) =>
{
    game.Id = games.Max(game => game.Id) + 1;
    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndPointName, new { id = game.Id }, game);
});


group.MapPut("/{id}", (int id, Game updatedGame) =>
{
    Game? existingGame = games.Find(game => game.Id == id);
    if (existingGame is null)
    {
        return Results.NotFound();
    }
    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ImageUri = updatedGame.ImageUri;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;

    return Results.NoContent();
});


group.MapDelete("/{id}", (int id) =>
{
    Game? game = games.Find(game => game.Id == id);
    if (game is null)
    {
        return Results.NotFound();
    }
    else
    {
        games.Remove(game);
        return Results.NoContent();
    }
});

app.Run();
