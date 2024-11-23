using Common.DTOs.Notification;
using Common.Interfaces;
using Infrastructure.Repositories;

namespace Domain.BusinessRules;

public class SmsRule : ISmsRule
{
    public void Send(SmsDto dto)
    {
        SmsRepository.Save(dto);
    }
    
    public SmsDto? GetByIdentifier(string identifier)
    {
        return SmsRepository.GetByIdentifier(identifier);
    }
}