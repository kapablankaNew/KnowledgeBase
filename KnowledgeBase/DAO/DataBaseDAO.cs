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

        private string URL = @"Host=localhost;Username=postgres;Password=postgres;Database=knowledgebase";

        public DataBaseDAO()
        {
            NpgsqlConnection connection = new NpgsqlConnection(URL);
            executor = new DataBaseExecutor(connection);
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
    }
}
