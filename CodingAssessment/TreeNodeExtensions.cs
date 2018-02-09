using CodingAssessment.Models;

namespace CodingAssessment
{
    /// <summary>
    /// Extension methods for the IdTree and IdNode classes.
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// Returns the total number of nodes that an IdTree contains.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns>System.Int32.</returns>
        public static int TotalNodes(this IdTree tree)
        {
            if (tree.RootNode == null) return 0;
            return tree.RootNode.CountChildren() + 1;
        }

        /// <summary>
        /// Counts the children of the IdNode.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>System.Int32.</returns>
        private static int CountChildren(this IdNode node)
        {
            if (node == null) return 0;
            var count = node.Children.Count;
            foreach (var child in node.Children)
            {
                if (child?.Children.Count > 0)
                    count += CountChildren(child);
            }
            return count;
        }
    }
}
