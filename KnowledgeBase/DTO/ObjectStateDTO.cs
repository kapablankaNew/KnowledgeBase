using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.DTO
{
    class ObjectStateDTO
    {
        public DateTime measureTime { get; }

        public double parameterValue { get; }

        public Parameter parameterName { get; }

        public ObjectStateDTO(DateTime measureTime, double parameterValue, Parameter parameterName)
        {
            this.measureTime = measureTime;
            this.parameterValue = parameterValue;
            this.parameterName = parameterName;
        }
    }
}
