using Microsoft.Data.Sqlite;

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
                command.Parameters.Add(new SqliteParameter("@Second_name", userSecondName));
                command.Parameters.Add(new SqliteParameter("@Id", userId));
                command.Parameters.Add(new SqliteParameter("@Date_insert", DateTime.Now.ToString("g")));
                command.Parameters.Add(new SqliteParameter("@Active", 1));
                command.ExecuteNonQuery();


            }
        }

        public async Task AddUser(string userName, long userId, string userFirstName)
        {
            await using (var connection = new SqliteConnection(this.connectionString))
            {

                connection.Open();

                string sqlExpression = "INSERT OR IGNORE INTO Users (Name, First_name, Id, Date_insert, Active) VALUES (@Name, @First_name,  @Id, @Date_insert, @Active)";
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                // создаем параметр для сообщения

                command.Parameters.Add(new SqliteParameter("@Name", userName));
                command.Parameters.Add(new SqliteParameter("@First_name", userFirstName));
                command.Parameters.Add(new SqliteParameter("@Id", userId));
                command.Parameters.Add(new SqliteParameter("@Date_insert", DateTime.Now.ToString("g")));
                command.Parameters.Add(new SqliteParameter("@Active", 1));
                command.ExecuteNonQuery();


            }
        }



    }
}
