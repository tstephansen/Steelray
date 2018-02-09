using System.Collections.Generic;
using System.Linq;
using CodingAssessment.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodingAssessment.Tests
{
    [TestClass]
    public class IdTreeSerializerTests
    {
        [TestMethod]
        public void Sample_Bytes_Length_Should_Be_Equal_To_Encoded_Bytes()
        {
            var bytes = CreateBytes();
            var encodedBytes = System.Text.Encoding.ASCII.GetBytes("1[2[3,4[8[13[14]]],5[6[12],7[9[10,11]]]]]");
            Assert.AreEqual(encodedBytes.Length, bytes.Length);
        }

        [TestMethod]
        public void Serialize_Should_Return_A_Byte_Array_Representing_The_IdTree()
        {
            var correctBytes = CreateBytes();
            var tree = CreateTree();
            var serializer = new IdTreeSerializer();
            var bytes = serializer.Serialize(tree);
            Assert.AreEqual(correctBytes.Length, bytes.Length);
            for (var i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(correctBytes[i], bytes[i]);
            }
        }

        [TestMethod]
        public void Byte_Array_Should_Be_Deserialized_Into_An_IdTree_Equivalent_To_The_Original()
        {
            var origTree = CreateTree();
            var bytes = CreateBytes();
            var serializer = new IdTreeSerializer();
            var tree = serializer.Deserialize(bytes);
            var rootNode = tree.RootNode;
            var secondNode = rootNode.Children.First(c => c.Id == 2);
            var thirdNode = secondNode.Children.First(c => c.Id == 3);
            var fourthNode = secondNode.Children.First(c => c.Id == 4);
            var fifthNode = secondNode.Children.First(c => c.Id == 5);
            var sixthNode = fifthNode.Children.First(c => c.Id == 6);
            var seventhNode = fifthNode.Children.First(c => c.Id == 7);
            var eighthNode = fourthNode.Children.First(c => c.Id == 8);
            var ninthNode = seventhNode.Children.First(c => c.Id == 9);
            var tenthNode = ninthNode.Children.First(c => c.Id == 10);
            var eleventhNode = ninthNode.Children.First(c => c.Id == 11);
            var twelfthNode = sixthNode.Children.First(c => c.Id == 12);
            var thirteenthNode = eighthNode.Children.First(c => c.Id == 13);
            var origCount = origTree.TotalNodes();
            var newCount = tree.TotalNodes();
            Assert.AreEqual(origCount, newCount);
            Assert.AreEqual(1, tree.RootNode.Children.Count);
            Assert.AreEqual(3, secondNode.Children.Count);
            Assert.AreEqual(0, thirdNode.Children.Count);
            Assert.AreEqual(1, fourthNode.Children.Count);
            Assert.AreEqual(2, fifthNode.Children.Count);
            Assert.AreEqual(1, sixthNode.Children.Count);
            Assert.AreEqual(1, seventhNode.Children.Count);
            Assert.AreEqual(1, eighthNode.Children.Count);
            Assert.AreEqual(2, ninthNode.Children.Count);
            Assert.AreEqual(0, tenthNode.Children.Count);
            Assert.AreEqual(0, eleventhNode.Children.Count);
            Assert.AreEqual(0, twelfthNode.Children.Count);
            Assert.AreEqual(1, thirteenthNode.Children.Count);
        }

        [TestMethod]
        public void Serialize_Should_Return_Default_Byte_Array_When_Passed_A_Null_Value()
        {
            const IdTree tree = null;
            var serializer = new IdTreeSerializer();
            var result = serializer.Serialize(tree);
            Assert.AreEqual(default(byte[]), result);
        }

        [TestMethod]
        public void Deserialize_Should_Return_Empty_IdTree_When_Passed_A_Null_Value()
        {
            const byte[] bytes = null;
            var serializer = new IdTreeSerializer();
            var result = serializer.Deserialize(bytes);
            Assert.IsNull(result.RootNode);
        }

        [TestMethod]
        public void Deserialize_Should_Return_Empty_Tree_When_Passed_An_Invalid_Byte_Array()
        {
            var bytes = new byte[] {0, 0, 0};
            var serializer = new IdTreeSerializer();
            var result = serializer.Deserialize(bytes);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Serialize_Should_Still_Work_When_RootNode_Has_No_Children()
        {
            var tree = new IdTree {RootNode = new IdNode {Id = 1, Children = null}};
            var serializer = new IdTreeSerializer();
            var result = serializer.Serialize(tree);
            Assert.AreNotEqual(default(byte[]), result);
        }

        [TestMethod]
        public void CountNodes_Extension_Method_Should_Return_0_When_There_Are_No_Children()
        {
            var node = new IdNode {Id = 1, Children = null};
            var nodeCount = node.CountChildren();
            Assert.AreEqual(0, nodeCount);
        }

        [TestMethod]
        public void TotalNodes_Extension_Method_Should_Return_0_When_There_Are_No_Nodes()
        {
            var tree = new IdTree();
            var nodeCount = tree.TotalNodes();
            Assert.AreEqual(0, nodeCount);
        }

        #region Sample Data
        /// <summary>
        /// Creates an IdTree with parent nodes and children for testing.
        /// </summary>
        /// <returns>IdTree.</returns>
        private static IdTree CreateTree()
        {
            var tree = new IdTree();
            var n1 = new IdNode { Id = 1, Children = new List<IdNode>() };
            var n2 = new IdNode { Id = 2, Parent = n1, Children = new List<IdNode>() };
            var n3 = new IdNode { Id = 3, Parent = n2, Children = new List<IdNode>() };
            var n4 = new IdNode { Id = 4, Parent = n2, Children = new List<IdNode>() };
            var n5 = new IdNode { Id = 5, Parent = n2, Children = new List<IdNode>() };
            var n6 = new IdNode { Id = 6, Parent = n5, Children = new List<IdNode>() };
            var n7 = new IdNode { Id = 7, Parent = n5, Children = new List<IdNode>() };
            var n8 = new IdNode { Id = 8, Parent = n4, Children = new List<IdNode>() };
            var n9 = new IdNode { Id = 9, Parent = n7, Children = new List<IdNode>() };
            var n10 = new IdNode { Id = 10, Parent = n9, Children = new List<IdNode>() };
            var n11 = new IdNode { Id = 11, Parent = n9, Children = new List<IdNode>() };
            var n12 = new IdNode { Id = 12, Parent = n6, Children = new List<IdNode>() };
            var n13 = new IdNode { Id = 13, Parent = n8, Children = new List<IdNode>() };
            var n14 = new IdNode { Id = 14, Parent = n13, Children = new List<IdNode>() };
            n1.Children.Add(n2);
            n2.Children.AddRange(new[] { n3, n4, n5 });
            n4.Children.Add(n8);
            n5.Children.AddRange(new[] { n6, n7 });
            n6.Children.Add(n12);
            n7.Children.Add(n9);
            n8.Children.Add(n13);
            n9.Children.AddRange(new[] { n10, n11 });
            n13.Children.Add(n14);
            tree.RootNode = n1;
            return tree;
        }

        /// <summary>
        /// Creates a byte array for the string 1[2[3,4[8[13[14]]],5[6[12],7[9[10,11]]]]].
        /// </summary>
        /// <returns>System.Byte[].</returns>
        private static byte[] CreateBytes() => new byte[]
        {
            49,91,50,91,51,44,52,91,56,91,49,51,91,49,52,93,93,93,44,53,91,54,91,49,50,93,44,55,91,57,91,49,48,44,49,49,93,93,93,93,93
        };
        #endregion
    }
}
