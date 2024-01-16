using Microsoft.Data.Sqlite;
using System;

namespace WBbot.DataBase
{
    internal class DBWorker
    {
        private string connectionString;

        public DBWorker(string connectionString)
        {
            this.connectionString = connectionString;
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

        }

        public async Task AddUser(string? userName, long userId, string action)
        {
            try
            {
                await using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "INSERT INTO Users (Name, Id, Date_insert, Action) VALUES (@Name, @Id, @Date_insert, @Action)";
                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                    // создаем параметр для сообщения
                    command.Parameters.Add(new SqliteParameter("@Name", userName == null ? "NULL" : userName));
                    command.Parameters.Add(new SqliteParameter("@Id", userId));
                    command.Parameters.Add(new SqliteParameter("@Date_insert", DateTime.Now.ToString("g")));
                    command.Parameters.Add(new SqliteParameter("@Action", action));
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {

                var ErrorMessage = exception switch
                {
                    SqliteException sqliteException => $"SQLexxeption:\n[{sqliteException.ErrorCode}]\n{sqliteException.Message}",
                    _ => exception.ToString()

                };
                Console.WriteLine(ErrorMessage);
                
            }

        }
        
    }
}
