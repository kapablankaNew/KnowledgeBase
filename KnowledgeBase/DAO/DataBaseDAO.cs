using KnowledgeBase.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.DAO
{
    class DataBaseDAO
    {
        private NpgsqlConnection connection { get; set; }

        private string URL = @"Host=localhost;Username=postgres;Password=postgres;Database=knowledgebase";

        public DataBaseDAO()
        {
            connection = new NpgsqlConnection(URL);
        }

        public List<ObjectState> getAllData()
        {
            List<ObjectState> result = new List<ObjectState>();
            connection.Open();
            var create = new NpgsqlCommand("SELECT * FROM sensors ORDER BY \"measureTime\" DESC", connection);
            create.Prepare();
            var reader = create.ExecuteReader();
            foreach (DbDataRecord entry in reader)
            {
                DateTime measureTime = (DateTime)entry["measureTime"];
                double Tnv = double.Parse(entry["Tnv"].ToString());
                double T1 = double.Parse(entry["T1"].ToString());
                double P1 = double.Parse(entry["P1"].ToString());
                double T11 = double.Parse(entry["T11"].ToString());
                double T21 = double.Parse(entry["T21"].ToString());
                double P2 = double.Parse(entry["P2"].ToString());
                ObjectState state = new ObjectState(measureTime, Tnv, T1, P1, T11, T21, P2);
                result.Add(state);
            }
            connection.Close();
            return result;
        }

        public List<ObjectState> getDataForTimeInterval(DateTime from, DateTime to)
        {
            List<ObjectState> result = new List<ObjectState>();
            connection.Open();
            var create = new NpgsqlCommand("SELECT * FROM sensors " +
            "WHERE \"measureTime\" > @from AND \"measureTime\" < @to " +
            "ORDER BY \"measureTime\" DESC", connection);
            create.Parameters.Add("from", NpgsqlTypes.NpgsqlDbType.Timestamp);
            create.Parameters.Add("to", NpgsqlTypes.NpgsqlDbType.Timestamp);
            create.Prepare();
            create.Parameters[0].Value = from;
            create.Parameters[1].Value = to;
            var reader = create.ExecuteReader();
            foreach (DbDataRecord entry in reader)
            {
                DateTime measureTime = (DateTime)entry["measureTime"];
                double Tnv = double.Parse(entry["Tnv"].ToString());
                double T1 = double.Parse(entry["T1"].ToString());
                double P1 = double.Parse(entry["P1"].ToString());
                double T11 = double.Parse(entry["T11"].ToString());
                double T21 = double.Parse(entry["T21"].ToString());
                double P2 = double.Parse(entry["P2"].ToString());
                ObjectState state = new ObjectState(measureTime, Tnv, T1, P1, T11, T21, P2);
                result.Add(state);
            }
            connection.Close();
            return result;
        }
    }
}
