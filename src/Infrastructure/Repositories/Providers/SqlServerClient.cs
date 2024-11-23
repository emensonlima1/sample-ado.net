using System.Data;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repositories.Providers;

public class SqlServerClient(string connectionString) : IDisposable
{
    private IDbConnection _connection;
    private IDbTransaction _transaction;

    public void ExecuteNonQuery(string storedProcedure, params SqlParameter[] parameters)
    {
        OpenConnection();

        BeginTransaction();

        using var command = _connection.CreateCommand();
        command.CommandText = storedProcedure;
        command.CommandType = CommandType.StoredProcedure;
        command.Transaction = _transaction;

        foreach (var parameter in parameters)
        {
            command.Parameters.Add(parameter);
        }

        command.ExecuteNonQuery();
    }

    public List<T> ExecuteReaderList<T>(string storedProcedure, Func<IDataReader, T> mapToEntity,
        params SqlParameter[] parameters)
    {
        OpenConnection();

        var result = new List<T>();

        using var command = _connection.CreateCommand();
        command.CommandText = storedProcedure;
        command.CommandType = CommandType.StoredProcedure;

        foreach (var parameter in parameters)
        {
            command.Parameters.Add(parameter);
        }

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            result.Add(mapToEntity(reader));
        }

        return result;
    }

    public T? ExecuteReaderFirst<T>(string storedProcedure, Func<IDataReader, T> mapToEntity, params SqlParameter[] parameters)
    {
        try
        {
            OpenConnection();

            using var command = _connection.CreateCommand();
            command.CommandText = storedProcedure;
            command.CommandType = CommandType.StoredProcedure;

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            using var reader = command.ExecuteReader();

            return reader.Read() ? mapToEntity(reader) : default;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void OpenConnection()
    {
        try
        {
            if (_connection == null)
                _connection = new SqlConnection(connectionString);

            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void CloseConnection()
    {
        try
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region Dispose
    public void Dispose()
    {
        _transaction.Dispose();
        _connection.Dispose();
    }

    #endregion

    #region  Transaction
    private void BeginTransaction()
    {
        try
        {
            OpenConnection();

            if (_connection.State != ConnectionState.Open)
                throw new Exception("Connection is not open");
            
            if (_transaction != null)
                _transaction = _connection.BeginTransaction();
        }
        catch (Exception)
        {
            throw;
        }
        
    }

    public void Commit()
    {
        try
        {
            if (_connection.State != ConnectionState.Open)
                throw new Exception("Connection is not open");
            
            if (_transaction != null)
                throw new Exception("Transaction is not open");
            
            _transaction.Commit();
            
            CloseConnection();
            Dispose();
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public void Rollback()
    {
        try
        {
            if (_connection.State != ConnectionState.Open)
                throw new Exception("Connection is not open");
            
            if (_transaction != null)
                throw new Exception("Transaction is not open");
            
            _transaction.Rollback();

            CloseConnection();
            Dispose();
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}