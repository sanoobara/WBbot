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

        }

        async Task AddUser(string userName, string userId)
        {
            await using (var connection = new SqliteConnection(this.connectionString))
            {
                connection.Open();
                string sqlExpression = "INSERT INTO Users (Name, First_name, Second_name, Id, Date_insert, Active) VALUES (@Name, @First_name, @Second_name, @Id, @Date_insert, @Active)";
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                // создаем параметр для сообщения
                SqliteParameter messageParam = new SqliteParameter("@Name", messageText);
                command.Parameters.Add(messageParam);
                // создаем параметр для возраста
                SqliteParameter dateParam = new SqliteParameter("@First_name", message.Date.ToString("g"));
                command.Parameters.Add(dateParam);
                SqliteParameter id_userParam = new SqliteParameter("@id_user", message.From.Id);
                command.Parameters.Add(id_userParam);
                SqliteParameter nik_nameParam = new SqliteParameter("@nik_name", message.From.Username);
                command.Parameters.Add(nik_nameParam);

            }
        }


    }
}
