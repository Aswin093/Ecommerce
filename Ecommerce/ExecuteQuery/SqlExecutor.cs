using Microsoft.Data.SqlClient;
using System.Data;

namespace Ecommerce.ExecuteQuery
{
    public class SqlExecutor
    {
        private readonly string _connectionString;

        public SqlExecutor(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void ExecuteNonQuery(string storedProcedureName, params SqlParameter[] parameters)
        {
            using SqlConnection conn = new(_connectionString);
            conn.Open();

            using SqlCommand cmd = new(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            cmd.ExecuteNonQuery();
        }

        public T ExecuteReader<T>(string storedProcedureName, Func<SqlDataReader, T> readerFunc, params SqlParameter[] parameters)
        {
            using SqlConnection conn = new(_connectionString);
            conn.Open();

            using SqlCommand cmd = new(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            using SqlDataReader reader = cmd.ExecuteReader();
            return readerFunc(reader);
        }

        public bool ExecuteRowsAffected(string storedProcedure, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int rowsAffected = reader.GetInt32("RowsAffected");
                            return rowsAffected > 0;
                        }
                        return false;
                    }
                }
            }
        }

        public DataTable ExecuteDataTable(string storedProcedureName, params SqlParameter[] parameters)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new(storedProcedureName, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            using SqlDataAdapter adapter = new(cmd);
            DataTable dataTable = new();
            adapter.Fill(dataTable);
            return dataTable;
        }

        public object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using SqlConnection conn = new(_connectionString);
            conn.Open();

            using SqlCommand cmd = new(query, conn)
            {
                CommandType = CommandType.Text 
            };
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd.ExecuteScalar();
        }

        public void ExecuteTransaction(Action<SqlConnection, SqlTransaction> transactionAction)
        {
            using SqlConnection conn = new(_connectionString);
            conn.Open();

            using SqlTransaction transaction = conn.BeginTransaction();
            try
            {
                transactionAction(conn, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}