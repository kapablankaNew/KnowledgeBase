using Npgsql;
using System.Collections.Generic;
using System.Data.Common;

namespace KnowledgeBase.Executor
{
    delegate T Handler<T>(DbDataRecord entry);

    class DataBaseExecutor
    {
        private NpgsqlConnection connection { get; set; }

        public DataBaseExecutor(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public void execUpdate(NpgsqlCommand query)
        {
            connection.Open();
            query.Connection = connection;
            query.Prepare();
            query.ExecuteNonQuery();
            connection.Close();
        }

        public List<T> execQuery<T>(NpgsqlCommand query, Handler<T> handler)
        {
            connection.Open();
            query.Connection = connection;
            query.Prepare();
            var reader = query.ExecuteReader();
            List<T> result = new List<T>();
            foreach (DbDataRecord entry in reader)
            {
                result.Add(handler(entry));
            }
            connection.Close();
            return result;
        }
    }
}
