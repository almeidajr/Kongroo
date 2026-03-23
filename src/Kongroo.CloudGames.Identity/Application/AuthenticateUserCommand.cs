namespace Kongroo.CloudGames.Identity.Application;

public sealed record AuthenticateUserCommand(string Username, string Password);
