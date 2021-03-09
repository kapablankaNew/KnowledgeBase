using KnowledgeBase.DTO;
using KnowledgeBase.Entities;
using KnowledgeBase.Executor;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading;

namespace KnowledgeBase.DAO
{
    class DataBaseDAO
    {
        public delegate void DataBaseHandler(object sender, NpgsqlNotificationEventArgs e);

        public event DataBaseHandler dataBaseUpdate;

        private DataBaseExecutor executor;

        public DataBaseDAO(string URL)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(URL);
                executor = new DataBaseExecutor(connection);

                NpgsqlConnection connectionListen = new NpgsqlConnection(URL);
                connectionListen.Open();
                connectionListen.Notification += listenNotification;
                var listen = new NpgsqlCommand("Listen virtual", connectionListen);
                listen.ExecuteNonQuery();
                Thread threadListener = new Thread(() => {
                    while (true)
                    {
                        connectionListen.Wait();
                    }
                });
                threadListener.Start();
            }
            catch (ArgumentException)
            {
                throw new NpgsqlException("Error when connecting to the database!");
            }
        }

        private void listenNotification(object sender, NpgsqlNotificationEventArgs e)
        {
            dataBaseUpdate.Invoke(sender, e);
        }

        public List<ObjectState> getDataForTimeInterval(DateTime from, DateTime to, string table = "sensors")
        {
            List<ObjectState> result = new List<ObjectState>();
            var create = new NpgsqlCommand("SELECT * FROM " + table + " " +
            "WHERE \"measureTime\" > @from AND \"measureTime\" < @to " +
            "ORDER BY \"measureTime\" DESC");
            create.Parameters.Add("from", NpgsqlTypes.NpgsqlDbType.Timestamp);
            create.Parameters.Add("to", NpgsqlTypes.NpgsqlDbType.Timestamp);
            create.Parameters[0].Value = from;
            create.Parameters[1].Value = to;


            result = executor.execQuery(create,
                entry => {
                    ObjectState state = new ObjectState((DateTime)entry["measureTime"],
                        double.Parse(entry["Tnv"].ToString()),
                        double.Parse(entry["T1"].ToString()),
                        double.Parse(entry["P1"].ToString()),
                        double.Parse(entry["T11"].ToString()),
                        double.Parse(entry["T21"].ToString()),
                        double.Parse(entry["P2"].ToString()));
                    return state;
                });

            return result;
        }

        public List<ObjectStateDTO> getParameterForTimeInterval(DateTime from, DateTime to, Parameter parameterName, string table = "sensors")
        {
            List<ObjectStateDTO> result = new List<ObjectStateDTO>();

            List<ObjectState> states = getDataForTimeInterval(from, to, table);
            foreach (var state in states)
            {
                result.Add(state.convertToDTO(parameterName));
            }
            return result;
        }

        public List<ObjectState> getLastStates(int number = 1, string table = "sensors") 
        {
            var getting = new NpgsqlCommand("SELECT * FROM " + table + " " +
                "ORDER BY \"measureTime\" DESC LIMIT " + number);

            List<ObjectState> result = executor.execQuery(getting,
                entry => {
                    ObjectState state = new ObjectState((DateTime)entry["measureTime"],
                        double.Parse(entry["Tnv"].ToString()),
                        double.Parse(entry["T1"].ToString()),
                        double.Parse(entry["P1"].ToString()),
                        double.Parse(entry["T11"].ToString()),
                        double.Parse(entry["T21"].ToString()),
                        double.Parse(entry["P2"].ToString()));
                    return state;
                });

            return result;
        }

        public void writeData(ObjectState state, string table = "sensors") 
        {
            var adding = new NpgsqlCommand("INSERT INTO " + table + " VALUES (@measureTime, " +
                "@Tnv, @T1, @P1, @T11, @T21, @P2)");
            adding.Parameters.Add("measureTime", NpgsqlTypes.NpgsqlDbType.Timestamp);
            adding.Parameters.Add("Tnv", NpgsqlTypes.NpgsqlDbType.Real);
            adding.Parameters.Add("T1", NpgsqlTypes.NpgsqlDbType.Real);
            adding.Parameters.Add("P1", NpgsqlTypes.NpgsqlDbType.Real);
            adding.Parameters.Add("T11", NpgsqlTypes.NpgsqlDbType.Real);
            adding.Parameters.Add("T21", NpgsqlTypes.NpgsqlDbType.Real);
            adding.Parameters.Add("P2", NpgsqlTypes.NpgsqlDbType.Real);

            adding.Parameters[0].Value = state.measureTime;
            adding.Parameters[1].Value = state.Tnv;
            adding.Parameters[2].Value = state.T1;
            adding.Parameters[3].Value = state.P1;
            adding.Parameters[4].Value = state.T11;
            adding.Parameters[5].Value = state.T21;
            adding.Parameters[6].Value = state.P2;

            executor.execUpdate(adding);
        }
    }
}
