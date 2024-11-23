using System.Data;
using Common.DTOs.Notification;
using Infrastructure.Repositories.Providers;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repositories;

public static class SmsRepository
{
    public static void Save(SmsDto sms)
    {
        var client = new SqlServerClient("sua-connection-string");
        
        try
        {
            var parameters = new SqlParameter[]
            {
                new ("@PhoneNumber", SqlDbType.VarChar, 100) { Value = sms.PhoneNumber },
                new("@Body", SqlDbType.VarChar, 100) { Value = sms.Body },
                new("@Identifier", SqlDbType.VarChar, 35) { Value = sms.Identifier },
            };
        
            client.ExecuteNonQuery("sp_InsertSms", parameters);
        }
        catch (Exception)
        {
            client.Rollback();
            throw;
        }
    }
    
    public static SmsDto? GetByIdentifier(string identifier)
    {
        var client = new SqlServerClient("sua-connection-string");

        try
        {
            var parameters = new SqlParameter[]
            {
                new("@Identifier", SqlDbType.VarChar, 35) { Value = identifier }
            };

            return client.ExecuteReaderFirst(
                "sp_GetSmsByIdentifier",
                reader => new SmsDto(
                    reader["PhoneNumber"].ToString(),
                    reader["Body"].ToString(),
                    reader["Identifier"].ToString()
                ),
                parameters);
        }
        catch (Exception)
        {
            throw;
        }
    }
   
}