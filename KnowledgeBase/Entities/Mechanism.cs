using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace KnowledgeBase.Entities
{
    public class Mechanism
    {
        public string label { get; private set; }

        public UriNode ID { get; private set; }

        public Mechanism(UriNode ID, string label)
        {
            this.ID = ID;
            this.label = label;
        }

        public override bool Equals(object obj)
        {
            return obj is Mechanism mechanism &&
                   EqualityComparer<UriNode>.Default.Equals(ID, mechanism.ID);
        }

        public override int GetHashCode()
        {
            return 1213502048 + EqualityComparer<UriNode>.Default.GetHashCode(ID);
        }

        public override string ToString()
        {
            return label;
        }
    }
}
