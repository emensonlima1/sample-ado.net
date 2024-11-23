using Common.DTOs.Notification;

namespace Common.Interfaces;

public interface ISmsRule
{
    void Send(SmsDto dto);
    SmsDto? GetByIdentifier(string identifier);
}