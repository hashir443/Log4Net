using static LogKeeper.Enums.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace LogKeeper
{
    public class Class1
    {
        private readonly string _connectionString;
        private readonly string _filePath;
        private readonly IAmazonCloudWatchLogs _cloudWatchClient;
        private readonly string _logGroup;
        private readonly string _logStream;

        public CustomLogger(
            string connectionString,
            string filePath,
            IAmazonCloudWatchLogs cloudWatchClient,
            string logGroup,
            string logStream)
        {
            _connectionString = connectionString;
            _filePath = filePath;
            _cloudWatchClient = cloudWatchClient;
            _logGroup = logGroup;
            _logStream = logStream;
        }

        public async Task SaveDataLogs(LogEntry entry, LogTarget targets)
        {
            switch (targets)
            {
                case LogTarget.Database:
                    await SaveToDatabase(entry);
                    break;

                case LogTarget.File:
                    SaveToFile(entry);
                    break;

                case LogTarget.CloudWatch:
                    await SaveToCloudWatch(entry);
                    break;

                case LogTarget.All:
                    await SaveToDatabase(entry);
                    SaveToFile(entry);
                    await SaveToCloudWatch(entry);
                    break;

                default:
                    // Handle combined bit flags like Database | File
                    if (targets.HasFlag(LogTarget.Database)) await SaveToDatabase(entry);
                    if (targets.HasFlag(LogTarget.File)) SaveToFile(entry);
                    if (targets.HasFlag(LogTarget.CloudWatch)) await SaveToCloudWatch(entry);
                    break;
            }
        }

        private async Task SaveToDatabase(LogEntry entry)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("usp_SaveLog", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Message", entry.Message);
                cmd.Parameters.AddWithValue("@Level", entry.Level);
                cmd.Parameters.AddWithValue("@Timestamp", entry.Timestamp);
                cmd.Parameters.AddWithValue("@Source", entry.Source);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                File.AppendAllText("fallback_db_log.txt", $"[{DateTime.UtcNow}] Error logging to DB: {ex.Message}\n");
            }
        }

        private void SaveToFile(LogEntry entry)
        {
            var logLine = $"{entry.Timestamp:o} [{entry.Level}] {entry.Source}: {entry.Message}";
            File.AppendAllText(_filePath, logLine + Environment.NewLine);
        }

        private async Task SaveToCloudWatch(LogEntry entry)
        {
            try
            {
                var logEvent = new InputLogEvent
                {
                    Message = $"{entry.Timestamp:o} [{entry.Level}] {entry.Source}: {entry.Message}",
                    Timestamp = (long)(entry.Timestamp - DateTime.UnixEpoch).TotalMilliseconds
                };

                var putRequest = new PutLogEventsRequest
                {
                    LogGroupName = _logGroup,
                    LogStreamName = _logStream,
                    LogEvents = new List<InputLogEvent> { logEvent }
                };

                await _cloudWatchClient.PutLogEventsAsync(putRequest);
            }
            catch (Exception ex)
            {
                File.AppendAllText("fallback_cloud_log.txt", $"[{DateTime.UtcNow}] Error logging to CloudWatch: {ex.Message}\n");
            }
        }
    }
}
