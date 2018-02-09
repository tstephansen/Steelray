using CodingAssessment.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingAssessment
{
    /// <summary>
    /// Serializes and Deserializes an IdTree object.
    /// </summary>
    public class IdTreeSerializer
    {
        /// <summary>
        /// Serializes the specified tree to a byte array.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>System.Byte[].</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "The assignment specified a non static method")]
        public byte[] Serialize(IdTree tree)
        {
            var sb = new StringBuilder();
            var rootNode = tree.RootNode;
            ConvertNodesToString(rootNode, sb);
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Deserializes byte array into a tree.
        /// </summary>
        /// <param name="bytes">The tree bytes.</param>
        /// <returns>IdTree.</returns>
        public IdTree Deserialize(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes);
            _nodeDict = new Dictionary<int, IdNode>();
            var array = str.ToCharArray();
            var rootId = GetNodeId(array, 0);
            var rootNode = new IdNode
            {
                Id = rootId,
                Parent = null,
                Children = new List<IdNode>()
            };
            _nodeDict.Add(rootNode.Id, rootNode);
            DeserializeNodes(array, rootNode);
            return RecreateTreeStructure(_nodeDict);
        }

        #region Private Methods

        /// <summary>
        /// Converts the structure of the IdTree to string.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="sb">The sb.</param>
        private static void ConvertNodesToString(IdNode node, StringBuilder sb)
        {
            if (node?.Children == null) return;
            sb.Append(node.Id);
            if (node.Children.Count > 0) sb.Append('[');
            for (var i = 0; i < node.Children.Count; i++)
            {
                ConvertNodesToString(node.Children[i], sb);
                if (i + 1 != node.Children.Count)
                    sb.Append(',');
            }
            if (node.Children.Count > 0) sb.Append(']');
        }

        /// <summary>
        /// Deserializes the character array into IdNodes.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="node">The node.</param>
        private void DeserializeNodes(char[] array, IdNode node)
        {
            var nested = 0;
            var newNode = new IdNode();
            for (var i = 1; i < array.Length; i++)
            {
                if (IsNextCharNumber(array, i))
                {
                    var digit = GetNodeId(array, i);
                    newNode = new IdNode
                    {
                        Id = digit,
                        Parent = node,
                        Children = new List<IdNode>()
                    };
                    _nodeDict.Add(newNode.Id, newNode);
                    if (digit.ToString().Length > 1)
                        i += (digit.ToString().Length - 1);
                }
                if (i + 1 == array.Length) return;
                switch (array[i + 1])
                {
                    case '[':
                        nested++;
                        node = newNode;
                        break;
                    case ']':
                        node = (nested != 0 && node.Parent != null) ? node.Parent : newNode.Parent;
                        nested--;
                        break;
                    case ',':
                        break;
                }
            }
        }

        /// <summary>
        /// Recreates the IdTree structure.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns>IdTree.</returns>
        private static IdTree RecreateTreeStructure(Dictionary<int, IdNode> nodes)
        {
            Parallel.ForEach(nodes.Where(c=>c.Value.Parent != null), (node, state) =>
            {
                nodes.First(c => c.Key == node.Value.Parent.Id).Value.Children.Add(node.Value);
            });
            return new IdTree { RootNode = nodes.ElementAt(0).Value };
        }

        /// <summary>
        /// Gets the IdNode's Id.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns>System.Int32.</returns>
        private static int GetNodeId(char[] array, int index)
        {
            var sb = new StringBuilder(array[index].ToString());
            GetNextNumber(array, index, sb);
            var str = sb.ToString();
            var digit = int.Parse(str);
            return digit;
        }

        /// <summary>
        /// Gets the next number in the character array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <param name="sb">The sb.</param>
        private static void GetNextNumber(char[] array, int index, StringBuilder sb)
        {
            index++;
            var nextChar = array[index].ToString();
            while (nextChar != "[" && nextChar != "," && nextChar != "]")
            {
                sb.Append(nextChar);
                index++;
                nextChar = array[index].ToString();
            }
        }

        /// <summary>
        /// Determines if the next character is a number.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns>
        /// <c>true</c> if the next character of the array is a number; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNextCharNumber(char[] array, int index)
        {
            switch (array[index].ToString())
            {
                case "[":
                case ",":
                case "]":
                    return false;
                default:
                    return true;
            }
        }
        #endregion

        private Dictionary<int, IdNode> _nodeDict;
    }
}