using KnowledgeBase.DAO;
using KnowledgeBase.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;


namespace KnowledgeBase.Tests
{
    [TestClass()]
    public class EqualsTests
    {
        [TestMethod()]
        public void EqualControlTest()
        {
            KnowledgeBaseDAO dAO = new KnowledgeBaseDAO();
            dAO.loadData("../../../KnowledgeBase/resources/knowledgeBase.n3");
            List<Control> controlsFirst = dAO.getControls();
            List<Control> controlsSecond = dAO.getControls();
            for (int i = 0; i < controlsFirst.Count; i++)
            {
                Assert.IsTrue(controlsFirst[i].Equals(controlsSecond[i]));
            }
            Assert.IsFalse(controlsFirst[0].Equals(controlsFirst[1]));
        }

        [TestMethod()]
        public void EqualMaterialTest()
        {
            KnowledgeBaseDAO dAO = new KnowledgeBaseDAO();
            dAO.loadData("../../../KnowledgeBase/resources/knowledgeBase.n3");
            List<Material> materialsFirst = dAO.getMaterials();
            List<Material> materialsSecond = dAO.getMaterials();
            for (int i = 0; i < materialsFirst.Count; i++)
            {
                Assert.IsTrue(materialsFirst[i].Equals(materialsSecond[i]));
            }
            Assert.IsFalse(materialsFirst[0].Equals(materialsFirst[1]));
        }

        [TestMethod()]
        public void EqualMechanismTest()
        {
            KnowledgeBaseDAO dAO = new KnowledgeBaseDAO();
            dAO.loadData("../../../KnowledgeBase/resources/knowledgeBase.n3");
            List<Mechanism> mechanismsFirst = dAO.getMechanisms();
            List<Mechanism> mechanismsSecond = dAO.getMechanisms();
            for (int i = 0; i < mechanismsFirst.Count; i++)
            {
                Assert.IsTrue(mechanismsFirst[i].Equals(mechanismsSecond[i]));
            }
            Assert.IsFalse(mechanismsFirst[0].Equals(mechanismsFirst[1]));
        }
    }
}