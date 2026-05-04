namespace Zephyrus.Identity.Application.Models;

public record NotificationMessage(
    string To,
    string? Subject,
    string Body,
    string? From
    );