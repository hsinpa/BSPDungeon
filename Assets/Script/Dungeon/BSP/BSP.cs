using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;

namespace PG {
	public class BSP : MonoBehaviour {
		private float _width, _height;
		private List<TreeNode> _treeNodes = new List<TreeNode>();
		private List<BSPMapComponent> _treeComponents = new List<BSPMapComponent>();

		private TreeMap _treeMap;

		void Start() {
			SetUp(1000, 1000);
			GenerateMap(5);
		}

		public void SetUp(float p_width, float p_height) {
			_width = p_width;
			_height = p_height;

		}

		public void GenerateMap(int p_iteration) {
			_treeMap = new TreeMap(Vector2.zero, _width, _height);

			for (int i = 0; i < p_iteration; i++) {
				_treeMap.rootNode.Divide();
			}

			_treeMap.UpdateMapDatasetFromTree();
			_treeMap.GenerateCorridor();

			_treeComponents = GetAllMapComponent();
		}

		void OnDrawGizmos()
		{
			if (_treeMap == null) return;
			for (int i = 0 ; i < _treeMap.all_room_parent.Count; i++) {
				TreeNode node = _treeMap.all_room_parent[i];
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(node.leafs[0].rect.center, node.leafs[0].rect.size);
				Gizmos.DrawWireCube(node.leafs[1].rect.center, node.leafs[1].rect.size);
			}
			
			if (_treeComponents.Count > 0) {
				for (int i = 0 ; i < _treeComponents.Count; i++) {
					BSPMapComponent node = _treeComponents[i];

					if (node.GetType() == typeof(BSPRoom)) {
						Gizmos.color = Color.red;
						Gizmos.DrawWireCube(node.spaceRect.center, node.spaceRect.size);
					}
					else if (node.GetType() == typeof(BSPCorridor)) {
						Gizmos.color = Color.green;
						// Gizmos.DrawWireCube(node.spaceRect.center, node.spaceRect.size);
					}
				}
			}
		}

		public List<BSPMapComponent> GetAllMapComponent() {
			Stack<TreeNode> nodes = new Stack<TreeNode>();
			List<BSPMapComponent> mapComps = new List<BSPMapComponent>();

            nodes.Push(_treeMap.rootNode);

            while (nodes.Count > 0) {
                TreeNode node = nodes.Pop();
                if (node == null)
                    continue;

                if (node.leafs.Count > 0) {
                    nodes.Push(node.leafs[0]);
                    nodes.Push(node.leafs[1]);
                } else {
					mapComps.Add(node.room);
				}

				if (node.corridors.Count > 0) {
					foreach(BSPCorridor c in node.corridors)
						mapComps.Add(c);
				}
            }
			return mapComps;
		}

	}
}