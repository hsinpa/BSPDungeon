using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructure
{
    public class TreeMap {
        public TreeNode rootNode;

        public List<TreeNode> all_room_parent = new List<TreeNode>();

        public TreeMap(Vector2 p_center, float p_radius_x, float p_radius_y) {
            Vector2 position = new Vector2( p_center.x - p_radius_x,  p_center.y - p_radius_y );
            Rect rect = new Rect(position, new Vector2(p_radius_x * 2, p_radius_y * 2) );

            rootNode = new TreeNode(rect, null);
        }

        public void GenerateCorridor() {
            foreach (var node in all_room_parent)
            {
                node.GenerateCorridor();
            }
        }

        public void UpdateMapDatasetFromTree() {
            Stack<TreeNode> nodes = new Stack<TreeNode>();
            nodes.Push(rootNode);

            while (nodes.Count > 0) {
                TreeNode node = nodes.Pop();
                if (node == null)
                    continue;

                if (node.leafs.Count > 0) {
                    nodes.Push(node.leafs[0]);
                    nodes.Push(node.leafs[1]);
                }

                if (node.leafs.Count > 0 && node.leafs[0].room != null && node.leafs[1].room != null) {
                    all_room_parent.Add(node);
                }
            }

        }

    }
}