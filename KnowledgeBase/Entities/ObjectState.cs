using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
