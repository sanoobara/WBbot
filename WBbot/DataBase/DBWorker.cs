using Microsoft.Data.Sqlite;

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

        public async Task AddUser(string? userName, long userId, string action, string message)
        {
            try
            {
                await using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "INSERT INTO Users (Name, Id, Date_insert, Action, Message) VALUES (@Name, @Id, @Date_insert, @Action, @Message)";
                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                    // создаем параметр для сообщения
                    command.Parameters.Add(new SqliteParameter("@Name", userName == null ? "NULL" : userName));
                    command.Parameters.Add(new SqliteParameter("@Id", userId));
                    command.Parameters.Add(new SqliteParameter("@Date_insert", DateTime.Now.ToString("g")));
                    command.Parameters.Add(new SqliteParameter("@Action", action));
                    command.Parameters.Add(new SqliteParameter("@Message", message));
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


        public async Task<int> AddMarketOrder(int id, DateTime createdAt, string name, int price, int convertedPrice, int article)
        {
            int q;
            try
            {
                await using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string sqlExpression = "INSERT OR IGNORE INTO MarketOrders (Name, Id, Date_insert, Date_create, price,article, convertPrice) VALUES (@Name, @Id, @Date_insert, @Date_create, @price, @article, @convertPrice)";
                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                    // создаем параметр для сообщения
                    command.Parameters.Add(new SqliteParameter("@Name", name));
                    command.Parameters.Add(new SqliteParameter("@Id", id));
                    command.Parameters.Add(new SqliteParameter("@Date_insert", DateTime.Now.ToString("g")));
                    command.Parameters.Add(new SqliteParameter("@Date_create", createdAt));
                    command.Parameters.Add(new SqliteParameter("@article", article));
                    command.Parameters.Add(new SqliteParameter("@price", price));
                    command.Parameters.Add(new SqliteParameter("@convertPrice", convertedPrice));
                    q = command.ExecuteNonQuery();
                }
                return q;
            }

            catch (Exception exception)
            {

                var ErrorMessage = exception switch
                {
                    SqliteException sqliteException => $"SQLexxeption:\n[{sqliteException.ErrorCode}]\n{sqliteException.Message}",
                    _ => exception.ToString()

                };
                Console.WriteLine(ErrorMessage);
                return -1;

            }

        }

    }
}
