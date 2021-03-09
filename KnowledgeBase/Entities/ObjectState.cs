using KnowledgeBase.DTO;
using System;
using System.Text;

namespace KnowledgeBase.Entities
{
    class ObjectState
    {
        public DateTime measureTime { get; private set; }

        public double Tnv { get; private set; }

        public double T1 { get; private set; }

        public double P1 { get; private set; }

        public double T11 { get; private set; }

        public double T21 { get; private set; }

        public double P2 { get; private set; }

        public ObjectState(DateTime measureTime, double Tnv,
            double T1, double P1, double T11, double T21, double P2)
        {
            this.measureTime = measureTime;
            this.Tnv = Tnv;
            this.T1 = T1;
            this.P1 = P1;
            this.T11 = T11;
            this.T21 = T21;
            this.P2 = P2;
        }

        public double getParameterValue(Parameter param)
        {
            var property = typeof(ObjectState).GetProperty(param.ToString());
            return (double)property.GetValue(this);
        }

        public ObjectStateDTO convertToDTO(Parameter parameterName)
        {
            return new ObjectStateDTO(measureTime, getParameterValue(parameterName), parameterName);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Measure time: " + measureTime + ",\nTnv: " + Tnv + ",\n");
            builder.Append("T1: " + T1 + ",\nP1:" + P1 + ",\nT11:" + T11 + ",\n");
            builder.Append("T21: " + T21 + ",\nP2:" + P2);
            return builder.ToString();
        }
    }
}
