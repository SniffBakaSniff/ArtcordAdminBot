using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace ArtcordAdminBot.Database
{
    public static class GuildSettingsRepository
    {
        private static readonly string connectionString = "Data Source=Database/ArtcordBot.db";

        /// <summary>
        /// Inserts or updates a guild setting in the database.
        /// </summary>
        public static async Task UpsertGuildSettingAsync(string guildId, string settingName, string settingValue)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO GuildSettings (Guild_Id, SettingName, SettingValue)
                VALUES ($guildId, $settingName, $settingValue)
                ON CONFLICT(Guild_Id, SettingName) 
                DO UPDATE SET SettingValue = excluded.SettingValue;
            ";

            command.Parameters.AddWithValue("$guildId", guildId);
            command.Parameters.AddWithValue("$settingName", settingName);
            command.Parameters.AddWithValue("$settingValue", settingValue);

            await command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Retrieves the value of a specific setting for a guild.
        /// </summary>
        public static async Task<string?> GetGuildSettingAsync(string guildId, string settingName)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT SettingValue
                FROM GuildSettings
                WHERE Guild_Id = $guildId AND SettingName = $settingName;
            ";

            command.Parameters.AddWithValue("$guildId", guildId);
            command.Parameters.AddWithValue("$settingName", settingName);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return reader.GetString(0);
            }

            return null; // Return null if the setting is not found
        }
    }
}
