using Microsoft.Data.Sqlite;

namespace fima_client_maui7.LocalStorage
{
    public class LocalStore
    {
        /// <summary>
        /// Creates the schema if it's not already there.
        /// </summary>
        /// <param name="forceRecreate">Removes existing schema before recreating it.</param>
        public async Task CreateSchema(bool forceRecreate)
        {
            if (forceRecreate && File.Exists(GetDbFilePath()))
                File.Delete(GetDbFilePath());

            await using var connection = CreateConnection();
            await connection.OpenAsync();
            try
            {
                var command = connection.CreateCommand();
                command.CommandText =
                    """
                    CREATE TABLE IF NOT EXISTS nosql (
                        id TEXT PRIMARY KEY,
                        data TEXT NULL
                    )
                    """;
                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                await connection.CloseAsync();
            }
            
        }

        private static SqliteConnection CreateConnection()
        {
            var dbFile = GetDbFilePath();
            return new SqliteConnection($"Data Source={dbFile}");
        }

        private static string GetDbFilePath()
        {
            var dbFile = Path.Combine(FileSystem.AppDataDirectory, "fima.db");
            return dbFile;
        }

        public void CreateIndex()
        {
            // CREATE INDEX idx_nosql_name ON nosql (json_extract(data, '$.name'));
        }

        /// <summary>
        /// Adds or updates a document.
        /// </summary>
        public async Task Set(string id, string data)
        {
            await using var connection = CreateConnection();
            await connection.OpenAsync();
            try
            {
                const string statement = """
                                         INSERT INTO nosql (id, data)  VALUES (:id, :data)
                                         ON CONFLICT(id) DO UPDATE SET data = excluded.data;
                                         """;

                await using var command = new SqliteCommand(statement, connection);
                command.Parameters.AddWithValue(":id", id);
                command.Parameters.AddWithValue(":data", data);
                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                await connection.CloseAsync();
            }
            
        }

        public async Task<string?> Get(string id)
        {
            await using var connection = CreateConnection();
            await connection.OpenAsync();

            try
            {
                const string statement = "SELECT data FROM nosql WHERE id = :id";

                await using var command = new SqliteCommand(statement, connection);
                command.Parameters.AddWithValue(":id", id);
                var result = await command.ExecuteScalarAsync();
                return result?.ToString();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
