using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace KnowledgeBase
{
    //special class for getting data from the knowledge base
    //this class is abstraction layer between the program and the knowledge base
    public class DAO
    {
        //object containing data from the knowledge base
        private Graph knowledgeBase { get; set; } 
        private Notation3Parser parser { get; set; }
        private List<Process> allProcesses { get; set; }

        public DAO()
        {
            knowledgeBase = new Graph();
            parser = new Notation3Parser();
            allProcesses = new List<Process>();
        }

        //load data to Graph-object from file
        public void loadData(string fileName)
        {
            parser.Load(knowledgeBase, fileName);
            allProcesses = getAllProcesses();
            foreach (Process process in allProcesses)
            {
                List<Control> controls = getControls(process);
                process.addControls(controls);
                List<Mechanism> mechanisms = getMechanisms(process);
                process.addMechanisms(mechanisms);
                List<Material> inputs = getInputs(process);
                process.addInputs(inputs);
                List<Material> outputs = getOutputs(process);
                process.addOutputs(outputs);
            }
        }

        public List<Control> getControls()
        {
            List<Control> result = new List<Control>();
            string queryControls = @"prefix classes:<URN:classes:>
                prefix rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                select ?id
                where
                {
                    ?id		rdf:type		classes:Control.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryControls);

            for (int i = 0; i < resultSet.Count; i++) 
            {
                string queryLabel = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?label
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:label		?label.
                }";

                SparqlResultSet resultSetLabels = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryLabel);
                Control control = new Control((UriNode)resultSet[i]["id"],
                    resultSetLabels[0]["label"].ToString());
                result.Add(control);
            }

            return result;
        }

        public List<Control> getControls(Process process)
        {
            List<Control> result = new List<Control>();
            string queryControls = @"prefix predicat:<URN:predicat>
                select ?id
                where
                {
                    <" + process.ID.ToString() + @">		predicat:hasControl		?id.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryControls);

            for (int i = 0; i < resultSet.Count; i++)
            {
                string queryLabel = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?label
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:label		?label.
                }";

                SparqlResultSet resultSetLabels = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryLabel);
                Control control = new Control((UriNode)resultSet[i]["id"],
                    resultSetLabels[0]["label"].ToString());
                result.Add(control);
            }

            return result;
        }

        public List<Mechanism> getMechanisms()
        {
            List<Mechanism> result = new List<Mechanism>();
            string queryMechanisms = @"prefix classes:<URN:classes:>
                prefix rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                select ?id
                where
                {
                    ?id		rdf:type		classes:Mechanism.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryMechanisms);

            for (int i = 0; i < resultSet.Count; i++)
            {
                string queryLabel = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?label
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:label		?label.
                }";

                SparqlResultSet resultSetLabels = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryLabel);
                Mechanism mechanism = new Mechanism((UriNode)resultSet[i]["id"],
                    resultSetLabels[0]["label"].ToString());
                result.Add(mechanism);
            }

            return result;
        }

        public List<Mechanism> getMechanisms(Process process)
        {
            List<Mechanism> result = new List<Mechanism>();
            string queryMechanisms = @"prefix predicat:<URN:predicat>
                select ?id
                where
                {
                    <" + process.ID.ToString() + @">		predicat:hasMechanism		?id.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryMechanisms);

            for (int i = 0; i < resultSet.Count; i++)
            {
                string queryLabel = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?label
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:label		?label.
                }";

                SparqlResultSet resultSetLabels = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryLabel);
                Mechanism mechanism = new Mechanism((UriNode)resultSet[i]["id"],
                    resultSetLabels[0]["label"].ToString());
                result.Add(mechanism);
            }

            return result;
        }

        public List<Material> getMaterials()
        {
            List<Material> result = new List<Material>();
            string queryMaterials = @"prefix classes:<URN:classes:>
                prefix rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                select ?id
                where
                {
                    ?id		rdf:type		classes:Mechanism.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryMaterials);

            for (int i = 0; i < resultSet.Count; i++)
            {
                string queryLabel = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?label
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:label		?label.
                }";

                SparqlResultSet resultSetLabels = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryLabel);
                Material material = new Material((UriNode)resultSet[i]["id"],
                    resultSetLabels[0]["label"].ToString());
                result.Add(material);
            }

            return result;
        }

        public List<Material> getInputs(Process process)
        {
            List<Material> result = new List<Material>();
            string queryMaterials = @"prefix predicat:<URN:predicat>
                select ?id
                where
                {
                    <" + process.ID.ToString() + @">		predicat:hasInput		?id.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryMaterials);

            for (int i = 0; i < resultSet.Count; i++)
            {
                string queryLabel = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?label
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:label		?label.
                }";

                SparqlResultSet resultSetLabels = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryLabel);
                Material material = new Material((UriNode)resultSet[i]["id"],
                    resultSetLabels[0]["label"].ToString());
                result.Add(material);
            }

            return result;
        }

        public List<Material> getOutputs(Process process)
        {
            List<Material> result = new List<Material>();
            string queryMaterials = @"prefix predicat:<URN:predicat>
                select ?id
                where
                {
                    <" + process.ID.ToString() + @">		predicat:hasOutput		?id.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryMaterials);

            for (int i = 0; i < resultSet.Count; i++)
            {
                string queryLabel = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?label
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:label		?label.
                }";

                SparqlResultSet resultSetLabels = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryLabel);
                Material material = new Material((UriNode)resultSet[i]["id"],
                    resultSetLabels[0]["label"].ToString());
                result.Add(material);
            }

            return result;
        }

        public List<Process> getProcesses() 
        {
            return allProcesses;
        }

        private List<Process> getAllProcesses()
        {
            List<Process> result = new List<Process>();

            string queryProcesses = @"prefix classes:<URN:classes:>
                prefix rdf:<http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                select ?id
                where
                {
                    ?id		rdf:type		classes:Process.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryProcesses);

            for (int i = 0; i < resultSet.Count; i++)
            {
                string queryLabel = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?label
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:label		?label.
                }";

                SparqlResultSet resultSetLabels = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryLabel);

                string queryComment = @"prefix rdfs:<http://www.w3.org/2000/01/rdf-schema#>
                select ?comment
                where
                {
                    <" + resultSet[i]["id"].ToString() + @">	rdfs:comment		?comment.
                }";

                SparqlResultSet resultSetComments = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryComment);

                Process process = new Process((UriNode)resultSet[i]["id"],
                     resultSetLabels[0]["label"].ToString(),
                     resultSetComments[0]["comment"].ToString());
                result.Add(process);
            }

            return result;
        }

        public List<Process> getHighLevel()
        {
            List<Process> result = new List<Process>();
            foreach (Process process in allProcesses)
            {
                List<Process> parents = getParents(process);
                if (parents.Count == 0)
                {
                    result.Add(process);
                }
            }
            return result;
        }

        public List<Process> getParents(Process process)
        {
            List<Process> parents = new List<Process>();

            string queryParents = @"prefix predicat:<URN:predicat>
                select ?id
                where
                {
                    ?id		predicat:include		<" + process.ID.ToString() + @">.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryParents);

            for (int i = 0; i < resultSet.Count; i++)
            {
                Process parent = getProcessByID((UriNode)resultSet[i]["id"]);
                parents.Add(parent);
            }
            return parents;
        }

        public List<Process> getChildren(Process process)
        {
            List<Process> children = new List<Process>();

            string queryChildren = @"prefix predicat:<URN:predicat>
                select ?id
                where
                {
                    <" + process.ID.ToString() + @">		predicat:include		?id.
                }";
            SparqlResultSet resultSet = (SparqlResultSet)knowledgeBase.ExecuteQuery(queryChildren);

            for (int i = 0; i < resultSet.Count; i++)
            {
                Process child = getProcessByID((UriNode)resultSet[i]["id"]);
                children.Add(child);
            }
            return children;
        }

        public Process getProcessByID(UriNode ID)
        {
            foreach (Process process in allProcesses)
            {
                if (process.ID.Equals(ID))
                {
                    return process;
                }
            }
            return null;
        }
    }
}
