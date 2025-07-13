using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> ExecuteStoredProcedureAsync(string storedProcName, Action<SqlParameterCollection> paramBuilder)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(storedProcName, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            paramBuilder?.Invoke(command.Parameters);

            await connection.OpenAsync(); // Handles connection pooling internally
            return await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            // Log the error or rethrow depending on your error policy
            throw new Exception($"Error executing stored procedure: {storedProcName}", ex);
        }
    }
}
