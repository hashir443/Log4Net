using System.Data;
using Entity.Entity;
using Entity.Enums;
using log4net;
using Microsoft.Data.SqlClient;

namespace DataLogger
{
    public class DatLogger
    {
        private readonly string _connectionString;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(DatLogger));
        private readonly DatabaseHelper _dbHelper;

        public DatLogger(string connectionString)
        {
            _connectionString = connectionString;
            _dbHelper = new DatabaseHelper(connectionString);
        }

        public async void SaveDataLogs(LogPayload logPayload, LogTarget logTarget)
        {
            if (logTarget.HasFlag(LogTarget.Database))
            {
                await SaveLogToDatabase(logPayload);
            }

            if (logTarget.HasFlag(LogTarget.File))
            {
                SaveLogToFile(logPayload);
            }

            // CloudWatch can be handled here in future if needed
        }

        private async Task SaveLogToDatabase(LogPayload payload)
        {
            //string connectionString = "Server=DESKTOP-A6COQ9C;Database=AppLog;Integrated Security=True;";

            try
            {
                await _dbHelper.ExecuteStoredProcedureAsync("[Log].[InsertApplicationLog]", parameters =>
                {
                    parameters.AddWithValue("@MethodName", payload.MethodName);
                    parameters.AddWithValue("@LogMessage", payload.LogMessage);
                    parameters.AddWithValue("@ExtraInformation", payload.ExtraInformation);
                    parameters.AddWithValue("@LogType", (int)payload.LogType);
                    parameters.AddWithValue("@CreatedBy", payload.CreatedBy);
                });
            }
            catch (Exception ex)
            {
                // Fallback logging if DB logging fails
                Console.Error.WriteLine($"[LOGGING FAILED] Could not log to DB: {ex.Message}");
                // You can fallback to file logging here
            }
        }

        private void SaveLogToFile(LogPayload payload)
        {
            try
            {
                // Serialize the entire object to JSON
                var jsonLog = System.Text.Json.JsonSerializer.Serialize(payload, new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = false
                });

                // Log as JSON based on severity
                switch (payload.LogType)
                {
                    case LogType.Debugging:
                        _logger.Debug(jsonLog);
                        break;
                    case LogType.Information:
                        _logger.Info(jsonLog);
                        break;
                    case LogType.Warning:
                        _logger.Warn(jsonLog);
                        break;
                    case LogType.Error:
                        _logger.Error(jsonLog);
                        break;
                    case LogType.Critical:
                        _logger.Fatal(jsonLog);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[LOGGING FAILED] Could not log to file: {ex.Message}");
            }
        }
    }
}
