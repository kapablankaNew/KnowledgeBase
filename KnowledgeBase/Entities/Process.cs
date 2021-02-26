using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace KnowledgeBase
{
    public class Process
    {
        public string label { get; private set; }

        public UriNode ID { get; private set; }

        public string comment { get; private set; }

        public HashSet<Mechanism> mechanisms { get; private set; }

        public HashSet<Control> controls { get; private set; }

        public HashSet<Material> inputs { get; private set; }

        public HashSet<Material> outputs { get; private set; }

        public Process(UriNode ID, string label, string comment)
        {
            this.ID = ID;
            this.label = label;
            this.comment = comment;
            mechanisms = new HashSet<Mechanism>();
            controls = new HashSet<Control>();
            inputs = new HashSet<Material>();
            outputs = new HashSet<Material>();
        }

        public void addControl(Control control)
        {
            if (controls.Contains(control))
            {
                throw new ArgumentException("This process already has such a control!");
            }
            controls.Add(control);
        }

        public void addControls(List<Control> controls)
        {
            foreach (Control control in controls)
            {
                addControl(control);
            }
        }

        public void addMechanism(Mechanism mechanism)
        {
            if (mechanisms.Contains(mechanism))
            {
                throw new ArgumentException("This process already has such a mechanism!");
            }
            mechanisms.Add(mechanism);
        }

        public void addMechanisms(List<Mechanism> mechanisms)
        {
            foreach (Mechanism mechanism in mechanisms)
            {
                addMechanism(mechanism);
            }
        }

        public void addInput(Material input) 
        {
            if (inputs.Contains(input))
            {
                throw new ArgumentException("This process already has such a input!");
            }
            inputs.Add(input);
        }

        public void addInputs(List<Material> inputs)
        {
            foreach (Material input in inputs)
            {
                addInput(input);
            }
        }

        public void addOutput(Material output)
        {
            if (outputs.Contains(output))
            {
                throw new ArgumentException("This process already has such a output!");
            }
            outputs.Add(output);
        }

        public void addOutputs(List<Material> outputs)
        {
            foreach (Material output in outputs)
            {
                addOutput(output);
            }
        }

        public void clearData()
        {
            mechanisms.Clear();
            controls.Clear();
            inputs.Clear();
            outputs.Clear();
        }

        public string getData()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Process: " + label + "\n");
            builder.Append("Inputs: ");
            foreach (Material input in inputs)
            {
                builder.Append(input.ToString() + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append("\n");
            builder.Append("Outputs: ");
            foreach (Material output in outputs)
            {
                builder.Append(output.ToString() + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append("\n");
            builder.Append("Controls: ");
            foreach (Control control in controls)
            {
                builder.Append(control.ToString() + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append("\n");
            builder.Append("Mechanisms: ");
            foreach (Mechanism mechanism in mechanisms)
            {
                builder.Append(mechanism.ToString() + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Process process &&
                   label == process.label &&
                   EqualityComparer<UriNode>.Default.Equals(ID, process.ID) &&
                   comment == process.comment &&
                   EqualityComparer<HashSet<Mechanism>>.Default.Equals(mechanisms, process.mechanisms) &&
                   EqualityComparer<HashSet<Control>>.Default.Equals(controls, process.controls) &&
                   EqualityComparer<HashSet<Material>>.Default.Equals(inputs, process.inputs) &&
                   EqualityComparer<HashSet<Material>>.Default.Equals(outputs, process.outputs);
        }

        public override int GetHashCode()
        {
            int hashCode = 2115018310;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(label);
            hashCode = hashCode * -1521134295 + EqualityComparer<UriNode>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comment);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<Mechanism>>.Default.GetHashCode(mechanisms);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<Control>>.Default.GetHashCode(controls);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<Material>>.Default.GetHashCode(inputs);
            hashCode = hashCode * -1521134295 + EqualityComparer<HashSet<Material>>.Default.GetHashCode(outputs);
            return hashCode;
        }

        public override string ToString()
        {
            return label;
        }
    }
}
