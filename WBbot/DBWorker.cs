using Microsoft.Data.Sqlite;
using Telegram.Bot.Types;

namespace WBbot
{
    internal class DBWorker
    {
        private string connectionString;

        public DBWorker(string connectionString)
        {
            this.connectionString = connectionString;
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

        }

        public async Task AddUser(string userName, long userId, string userFirstName, string userSecondName)
        {
            await using (var connection = new SqliteConnection(this.connectionString))
            {

                connection.Open();
                
                string sqlExpression = "INSERT INTO Users (Name, First_name, Second_name, Id, Date_insert, Active) VALUES (@Name, @First_name, @Second_name, @Id, @Date_insert, @Active)";
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                // создаем параметр для сообщения
                
                command.Parameters.Add(new SqliteParameter("@Name", userName));
                command.Parameters.Add(new SqliteParameter("@First_name", userFirstName));
                command.Parameters.Add(new SqliteParameter("@Second_name", userSecondName.ToString()));
                command.Parameters.Add(new SqliteParameter("@Id", userId));
                command.Parameters.Add(new SqliteParameter("@Date_Insert", DateTime.Now.ToString("g")));
                command.Parameters.Add(new SqliteParameter("@Active", 1));
                int i = command.ExecuteNonQuery();
                await Console.Out.WriteLineAsync("Chel dobavlen");

            }
        }


    }
}
