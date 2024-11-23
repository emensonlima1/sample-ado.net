namespace Common.DTOs.Notification;

public record SmsDto(string? PhoneNumber, string? Body, string? Identifier);