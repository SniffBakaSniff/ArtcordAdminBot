using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace ArtcordAdminBot.Database
{
    public static class DatabaseInitializer
    {
        private static readonly string connectionString = "Data Source=Database/ArtcordBot.db";

        public static async Task InitializeDatabaseAsync()
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            await CreateGuildSettingsTableAsync(connection);
        }


        private static async Task CreateGuildSettingsTableAsync(SqliteConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS GuildSettings (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Guild_Id TEXT NOT NULL,
                    SettingName TEXT NOT NULL,
                    SettingValue TEXT NOT NULL
                );
            ";
            await command.ExecuteNonQueryAsync();
        }
    }
}
