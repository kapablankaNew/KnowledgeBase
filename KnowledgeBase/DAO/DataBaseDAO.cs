using KnowledgeBase.DTO;
using KnowledgeBase.Entities;
using KnowledgeBase.Executor;
using Npgsql;
using System;
using System.Collections.Generic;

namespace KnowledgeBase.DAO
{
    class DataBaseDAO
    {
        private DataBaseExecutor executor;

        public DataBaseDAO(string URL)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(URL);
                executor = new DataBaseExecutor(connection);
            }
            catch (ArgumentException)
            {
                throw new NpgsqlException("Error when connecting to the database!");
            }
        }

        public List<ObjectState> getDataForTimeInterval(DateTime from, DateTime to)
        {
            List<ObjectState> result = new List<ObjectState>();
            var create = new NpgsqlCommand("SELECT * FROM sensors " +
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

        public List<ObjectStateDTO> getParameterForTimeInterval(DateTime from, DateTime to, Parameter parameterName)
        {
            List<ObjectStateDTO> result = new List<ObjectStateDTO>();

            List<ObjectState> states = getDataForTimeInterval(from, to);
            foreach (var state in states)
            {
                result.Add(state.convertToDTO(parameterName));
            }
            return result;
        }

        public void writeData(ObjectState state) {
            var adding = new NpgsqlCommand("INSERT INTO sensors VALUES(@measureTime, " +
                "@Tnv, @T1, @P1, @T11, @T21, @P2");
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
