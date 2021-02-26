using System;
using System.Collections.Generic;
using KnowledgeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VDS.RDF;

namespace KnowledgeBaseTests
{
    [TestClass]
    public class ProcessTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddControl()
        {
            KnowledgeBaseDAO dAO = new KnowledgeBaseDAO();
            dAO.loadData("../../../KnowledgeBase/resources/knowledgeBase.n3");
            List<Control> controlsFirst = dAO.getControls();
            List<Control> controlsSecond = dAO.getControls();
            List<Process> processes = dAO.getProcesses();
            Assert.AreEqual(14, processes.Count);
            Process process = dAO.getProcesses()[0];
            process.clearData();
            process.addControl(controlsFirst[0]);
            Assert.AreEqual(1, process.controls.Count);
            process.addControl(controlsSecond[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddMechanism()
        {
            KnowledgeBaseDAO dAO = new KnowledgeBaseDAO();
            dAO.loadData("../../../KnowledgeBase/resources/knowledgeBase.n3");
            List<Mechanism> mechanismsFirst = dAO.getMechanisms();
            List<Mechanism> mechanismsSecond = dAO.getMechanisms();
            List<Process> processes = dAO.getProcesses();
            Assert.AreEqual(14, processes.Count);
            Process process = dAO.getProcesses()[0];
            process.clearData();
            process.addMechanism(mechanismsFirst[0]);
            Assert.AreEqual(1, process.mechanisms.Count);
            process.addMechanism(mechanismsSecond[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddInput()
        {
            KnowledgeBaseDAO dAO = new KnowledgeBaseDAO();
            dAO.loadData("../../../KnowledgeBase/resources/knowledgeBase.n3");
            List<Material> materialsFirst = dAO.getMaterials();
            List<Material> materialsSecond = dAO.getMaterials();
            List<Process> processes = dAO.getProcesses();
            Assert.AreEqual(14, processes.Count);
            Process process = dAO.getProcesses()[0];
            process.clearData();
            process.addInput(materialsFirst[0]);
            Assert.AreEqual(1, process.inputs.Count);
            process.addInput(materialsSecond[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddOutput()
        {
            KnowledgeBaseDAO dAO = new KnowledgeBaseDAO();
            dAO.loadData("../../../KnowledgeBase/resources/knowledgeBase.n3");
            List<Material> materialsFirst = dAO.getMaterials();
            List<Material> materialsSecond = dAO.getMaterials();
            List<Process> processes = dAO.getProcesses();
            Assert.AreEqual(14, processes.Count);
            Process process = dAO.getProcesses()[0];
            process.clearData();
            process.addOutput(materialsFirst[0]);
            Assert.AreEqual(1, process.outputs.Count);
            process.addOutput(materialsSecond[0]);
        }

        [TestMethod]
        public void TestAddInputOutputPositive()
        {
            KnowledgeBaseDAO dAO = new KnowledgeBaseDAO();
            dAO.loadData("../../../KnowledgeBase/resources/knowledgeBase.n3");
            List<Material> materialsFirst = dAO.getMaterials();
            List<Process> processes = dAO.getProcesses();
            Assert.AreEqual(14, processes.Count);
            Process process = dAO.getProcesses()[0];
            process.clearData();
            process.addOutput(materialsFirst[0]);
            Assert.AreEqual(1, process.outputs.Count);
            process.addInput(materialsFirst[0]);
            Assert.AreEqual(1, process.inputs.Count);
            process.addInput(materialsFirst[1]);
            Assert.AreEqual(2, process.inputs.Count);
            process.addOutput(materialsFirst[1]);
            Assert.AreEqual(2, process.outputs.Count);
        }
    }
}
