namespace Kongroo.CloudGames.Identity.Application;

public sealed record CreateUserCommand(string Username, string Email, string Password, string Name);
