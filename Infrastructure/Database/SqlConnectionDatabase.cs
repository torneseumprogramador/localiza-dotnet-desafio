using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace Infrastructure.Database
{
    public class SqlConnectionDatabase<T> : IDatabase<T> where T : class
    {
        public SqlConnectionDatabase()
        {
            string cnn = Environment.GetEnvironmentVariable("CONNECTION_STRING", EnvironmentVariableTarget.Process);
            this.connectionString = cnn;
        }

        private string connectionString;

        public void Save(T obj)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = MapTable.BuilderInsert(obj);
                SqlCommand command = new SqlCommand(queryString, connection);
                var parameters = MapTable.BuilderParameters(obj);
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void Update(T obj)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = MapTable.BuilderUpdate(obj);
                SqlCommand command = new SqlCommand(queryString, connection);
                var parameters = MapTable.BuilderParameters(obj, true);
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void SqlCommand(string queryString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<T> All(string SqlWhere = null)
        {
            var list = new List<T>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = MapTable.BuilderSelect<T>(SqlWhere);
                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Connection.Open();

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var instance = Activator.CreateInstance(typeof(T));
                        this.fill(instance, dr);
                        list.Add((T)instance);
                    }
                }
            }

            return list;
        }

        public List<T> AllByPrecedure(string queryString, List<DbParameter> parameters = null)
        {
            var list = new List<T>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Connection.Open();
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                        command.Parameters.Add(parameter);
                }

                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var instance = Activator.CreateInstance(typeof(T));
                        this.fill(instance, dr);
                        list.Add((T)instance);
                    }
                }
            }

            return list;
        }

        private void fill(object modelo, SqlDataReader dr)
        {
            foreach (var p in modelo.GetType().GetProperties())
            {
                try
                {
                    if (dr[p.Name] == DBNull.Value) continue;
                    p.SetValue(modelo, dr[p.Name]);
                }
                catch { }
            }
        }
        public void Remove(T obj)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var queryString = MapTable.BuilderDelete(obj);
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}