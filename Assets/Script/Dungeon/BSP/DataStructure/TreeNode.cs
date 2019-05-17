using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using PG;
using Utility;

namespace DataStructure {
	public class TreeNode {

		// public enum Type {
		// 	Room,
		// 	Corridor
		// }

		// public Type nodeType;
		public TreeNode parent;
		public List<TreeNode> leafs = new List<TreeNode>();
		public Rect rect;

		public BSPRoom room;
		public List<BSPCorridor> corridors = new List<BSPCorridor>();
        public List<BSPCorridor> tempCorridors = new List<BSPCorridor>();

        private float AllowRatioDifference = 1.4f;

		public TreeNode(Rect p_rect, TreeNode p_parent) {
			rect = p_rect;
			parent = p_parent;

			//Add Room
			if (p_parent != null)
				room = new BSPRoom(rect);
		}

		public bool IsRoom() {
			return (room != null);
		}

		public BSPRoom GetRandomRoom() {
			int randomIndex = Random.Range(0, leafs.Count);
			if (room != null) return room;

			if (leafs.Count > 0 && leafs[0].room != null) {
				return leafs[randomIndex].room;
			}

			return leafs[randomIndex].GetRandomRoom();
		}

		public void Divide() {
			room = null;
			//If is alraedy divide
			if (leafs.Count > 0) {
				foreach (TreeNode node in leafs)
				{
					node.Divide();
				}
			} else {
				Rect[] rooms = GetDivideRoomSize();
				leafs.Add(new TreeNode(rooms[0], this));
				leafs.Add(new TreeNode(rooms[1], this));
			}
		}

		/// <summary>
		/// Always return a Rect[] of 2 length
		/// </summary>
		/// <returns></returns>
		private Rect[] GetDivideRoomSize() {
			int throwDice = UtilityMethod.RollDice();

			Rect[] rooms = new Rect[2];

			//Horizontal 
			if (throwDice == 0) {
				if (rect.width  / rect.height  > AllowRatioDifference ) return GetDivideRoomSize();

				float randomY = UtilityMethod.RandRangeToInt( rect.height * 0.4f, rect.height * 0.6f );

				rooms[0] = new Rect(rect.x, rect.y, rect.width, randomY);
				rooms[1] = new Rect(rect.x, rect.y + randomY, rect.width, rect.height - randomY);
			//Vertical
			} else {
				if (rect.height  / rect.width  > AllowRatioDifference ) return GetDivideRoomSize();

				float randomX = UtilityMethod.RandRangeToInt( rect.width * 0.4f, rect.width*0.6f);

				rooms[0] = new Rect(rect.x, rect.y, randomX, rect.height);
				rooms[1] = new Rect(rect.x + randomX, rect.y, rect.width - randomX, rect.height);
			}

			return rooms;
		}

		public void GenerateCorridor() {
			if (leafs.Count <= 0) return;

			//Is the bottom treenode, or is its turn to generate corridor
			if ((leafs[0].room != null && leafs[1].room != null)
				|| (leafs[0].corridors.Count > 0 && leafs[1].corridors.Count > 0)
			 ) {
				LinkCorridorBetween(leafs[0].GetRandomRoom(), leafs[1].GetRandomRoom());

				//Ready to move backward
				if (corridors.Count > 0 && parent != null) {
					parent.GenerateCorridor();	
				}

			}
		}

		private void LinkCorridorBetween(BSPRoom left, BSPRoom right) {
            tempCorridors.Clear();

            Rect lroom = left.spaceRect;
			Rect rroom = right.spaceRect;

			// Debug.Log("Creating corridor(s) between " + left.debugId + "(" + lroom + ") and " + right.debugId + " (" + rroom + ")");

			// attach the corridor to a random point in each room
			Vector2 lpoint = new Vector2 (UtilityMethod.RandRangeToInt(lroom.x + 1, lroom.xMax - 1),
                                            UtilityMethod.RandRangeToInt(lroom.y + 1, lroom.yMax - 1));
			Vector2 rpoint = new Vector2 (UtilityMethod.RandRangeToInt(rroom.x + 1, rroom.xMax - 1),
                                             UtilityMethod.RandRangeToInt(rroom.y + 1, rroom.yMax - 1));




            //Debug.Log("lPoint " + lpoint +", rPoint " + rpoint);
			// always be sure that left point is on the left to simplify the code
			if (lpoint.x > rpoint.x) {
				Vector2 temp = lpoint;
				lpoint = rpoint;
				rpoint = temp;
			}

			int w = (int)(lpoint.x - rpoint.x);
			int h = (int)(lpoint.y - rpoint.y);
			int corridorSize = 1;

			// if the points are not aligned horizontally
			if (w != 0) {
				// choose at random to go horizontal then vertical or the opposite
				if (Random.Range (0, 1) > 2) {
                    // add a corridor to the right
                    tempCorridors.Add (new BSPCorridor( new Rect (lpoint.x, lpoint.y, Mathf.Abs (w) + 1, corridorSize)));

					// if left point is below right point go up
					// otherwise go down
					if (h < 0) {
                        tempCorridors.Add (new BSPCorridor(new Rect (rpoint.x, lpoint.y, corridorSize, Mathf.Abs (h))));
					} else {
                        tempCorridors.Add (new BSPCorridor(new Rect (rpoint.x, lpoint.y, corridorSize, -Mathf.Abs (h))));
					}

				} else {
					// go up or down
					if (h < 0) {
                        tempCorridors.Add (new BSPCorridor(new Rect (lpoint.x, lpoint.y, corridorSize, Mathf.Abs (h))));
					} else {
                        tempCorridors.Add (new BSPCorridor(new Rect (lpoint.x, rpoint.y, corridorSize, Mathf.Abs (h))));
					}

                    // then go right
                    tempCorridors.Add (new BSPCorridor(new Rect (lpoint.x, rpoint.y, Mathf.Abs (w) + 1, corridorSize)));
				}
			} else {

				//假設 房間的x軸對齊的話
				//根據y軸的差距 決定往下 或 往上走
				if (h < 0) {
                    tempCorridors.Add (new BSPCorridor(new Rect ((int)lpoint.x, (int)lpoint.y, corridorSize, Mathf.Abs (h))));
				} else {
                    tempCorridors.Add (new BSPCorridor(new Rect ((int)rpoint.x, (int)rpoint.y, corridorSize, Mathf.Abs (h))));
				}
			}

            corridors.AddRange(tempCorridors);

            //left.FindDoorIntersection(tempCorridors, corridorSize);
            //right.FindDoorIntersection(tempCorridors, corridorSize);

            foreach (BSPCorridor corridor in corridors) {
				//Debug.Log ("corridor: " + corridor.spaceRect);
			}
		}

	}
}