using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Sql
{
    private readonly string connectionString;

    public Sql(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters)
    {
        DataTable dataTable = new DataTable();

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Hata yönetimi
            throw new Exception("An error occurred while executing the query: " + ex.Message);
        }

        return dataTable;
    }

    public int ExecuteNonQuery(string query, Dictionary<string, object> parameters)
    {
        int affectedRows = 0;

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    affectedRows = command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            // Hata yönetimi
            throw new Exception("An error occurred while executing the query: " + ex.Message);
        }

        return affectedRows;
    }

    public int Delete(string query, Dictionary<string, object> parameters)
    {
        return ExecuteNonQuery(query, parameters);
    }

    public int Insert(string query, Dictionary<string, object> parameters)
    {
        return ExecuteNonQuery(query, parameters);
    }

    public void ReplaceRecords(string deleteQuery, Dictionary<string, object> deleteParameters, string insertQuery, List<Dictionary<string, object>> insertParametersList)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection, transaction))
                {
                    if (deleteParameters != null)
                    {
                        foreach (var param in deleteParameters)
                        {
                            deleteCommand.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    deleteCommand.ExecuteNonQuery();
                }

                foreach (var insertParameters in insertParametersList)
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction))
                    {
                        foreach (var param in insertParameters)
                        {
                            insertCommand.Parameters.AddWithValue(param.Key, param.Value);
                        }

                        insertCommand.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                throw new Exception("An error occurred while replacing records: " + ex.Message);
            }
        }
    }
}
