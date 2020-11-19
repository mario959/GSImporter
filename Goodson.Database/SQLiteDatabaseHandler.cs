using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Goodson.Database
{
    public class ArticleDBRepository : IDisposable
    {
        private bool _disposed = false;

        private SqliteConnection _connection;
        private SqliteTransaction _transaction;

        public ArticleDBRepository(string connectionString)
        {
            _connection = new SqliteConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        #region Database interaction

        // Wichtig! Damit auch die Daten gespeichert werden muss Commit am Ende eines erfolgreichen Imports (also nach InsertArticle) ausgefuehrt werden.
        public void Commit()
        {
            _transaction.Commit();
        }

        public void InsertArticle(string articleNumber, string name, string description, int? deliveryStateId = null)
        {
            var insertCommand = _connection.CreateCommand();
            insertCommand.Transaction = _transaction;
            insertCommand.CommandText = $"INSERT INTO Articles (ArticleNumber, Name, Description {(deliveryStateId.HasValue ? ", DeliveryStateId" : "")}) VALUES ($ArticleNumber, $Name, $Description {(deliveryStateId.HasValue ? ", $DeliveryStateId" : "")})";
            insertCommand.Parameters.AddWithValue("$ArticleNumber", articleNumber);
            insertCommand.Parameters.AddWithValue("$Name", name);
            insertCommand.Parameters.AddWithValue("$Description", description);
            if (deliveryStateId.HasValue) insertCommand.Parameters.AddWithValue("$DeliveryStateId", deliveryStateId);
            insertCommand.ExecuteNonQuery();
            
        }
        #endregion


        #region Dispose Implentation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _connection.Dispose();
            }
            _disposed = true;
        }
        #endregion
    }

    public class SQLiteDatabaseHandler
    {
        private string _databaseFilePath = "litlepim.db";

        #region Public Functions
        public bool InitDatabase()
        {
            if (CleanDatabaseFile())
            {
                using (var connection = new SqliteConnection(GetConnectionString()))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        // main table with article data
                        ExecuteNonQuery(connection, transaction, @"CREATE TABLE Articles (
                          ArticleId      integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                          ArticleNumber  varchar(50),
                          Name           text,
                          Description    text,
                          DeliveryStateId  integer  DEFAULT NULL
                        );");

                        // second table with delivery states
                        ExecuteNonQuery(connection, transaction, @"CREATE TABLE DeliveryStates (
                          DeliveryStateId      integer NOT NULL PRIMARY KEY AUTOINCREMENT,
                          Name           text
                        );");

                        AddDeliveryState(connection, transaction, "in stock");
                        AddDeliveryState(connection, transaction, "out of stock");
                        AddDeliveryState(connection, transaction, "soon in stock");
                        transaction.Commit();
                    }
                }
                return true;
            }
            return false;
        }

        public ArticleDBRepository CreateArticleRepository()
        {
            return new ArticleDBRepository(GetConnectionString());
        }

        public List<List<Tuple<string, string>>> QueryData(string selectQuery)
        {
            var tableData = new List<List<Tuple<string, string>>>();
            using (var connection = new SqliteConnection(GetConnectionString()))
            {
                connection.Open();
                var queryCommand = connection.CreateCommand();
                queryCommand.CommandText = selectQuery;

                var sqliteReader = queryCommand.ExecuteReader();
                while (sqliteReader.Read())
                {
                    var row = new List<Tuple<string, string>>();
                    for (int columnIndex = 0; columnIndex < sqliteReader.FieldCount; columnIndex++)
                    {
                        var fieldValue = sqliteReader[columnIndex];
                        row.Add(Tuple.Create(sqliteReader.GetName(columnIndex), fieldValue.ToString()));
                    }
                    tableData.Add(row);
                }
            }
            return tableData;
        }
        #endregion


        #region Private Helper Functions
        private static void AddDeliveryState(SqliteConnection connection, SqliteTransaction transaction, string state)
        {
            var insertCommand = connection.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = "INSERT INTO DeliveryStates (Name) VALUES ($name)";
            insertCommand.Parameters.AddWithValue("$name", state);
            insertCommand.ExecuteNonQuery();
        }

        private string GetConnectionString()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = _databaseFilePath };
            return connectionStringBuilder.ToString();
        }

        private bool CleanDatabaseFile()
        {
            try
            {
                if (System.IO.File.Exists(_databaseFilePath))
                {
                    System.IO.File.Delete(_databaseFilePath);
                }
            } catch {
                return false;
            }
            return true;
        }

        private static void ExecuteNonQuery(SqliteConnection connection, SqliteTransaction transaction, string createTable)
        {
            var insertCommand = connection.CreateCommand();
            
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = createTable;
            insertCommand.ExecuteNonQuery();
        }
        #endregion
    }
}
