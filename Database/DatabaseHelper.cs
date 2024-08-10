using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtcordAdminBot.Database
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = "Data Source=Database/ArtcordAdminBot.db";

        // Initializes the database and creates the necessary tables if they don't exist
        public static async Task InitializeDatabaseAsync()
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS CommandLogs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId TEXT NOT NULL,
                    UserName TEXT NOT NULL,
                    CommandName TEXT NOT NULL,
                    CommandArgs TEXT,
                    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                );
            ";

            await command.ExecuteNonQueryAsync();
        }

        // Logs a command execution to the database
        public static async Task LogCommandAsync(string userId, string userName, string commandName, string commandArgs)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO CommandLogs (UserId, UserName, CommandName, CommandArgs)
                VALUES ($userId, $userName, $commandName, $commandArgs);
            ";

            command.Parameters.AddWithValue("$userId", userId);
            command.Parameters.AddWithValue("$userName", userName);
            command.Parameters.AddWithValue("$commandName", commandName);
            command.Parameters.AddWithValue("$commandArgs", commandArgs ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        // Retrieves all command logs from the database
        public static async Task<List<CommandLog>> GetCommandLogsAsync()
        {
            var logs = new List<CommandLog>();

            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, UserId, UserName, CommandName, CommandArgs, Timestamp
                FROM CommandLogs
                ORDER BY Timestamp DESC;
            ";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new CommandLog
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetString(1),
                    UserName = reader.GetString(2),
                    CommandName = reader.GetString(3),
                    CommandArgs = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Timestamp = reader.GetDateTime(5)
                });
            }

            return logs;
        }
    }

    // Represents a log entry for a command execution
    public class CommandLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string CommandName { get; set; } = string.Empty;
        public string? CommandArgs { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
