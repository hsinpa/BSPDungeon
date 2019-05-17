using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructure;
using Utility;

namespace PG {
	public class BSPRoom : BSPMapComponent {

        public List<Vector2Int> doorPosition;

		public BSPRoom(Rect p_holderRect) {
			//Calculate roomRect
			spaceRect = CalculateRoomRect(p_holderRect);
			display_order = 1;

            doorPosition = new List<Vector2Int>();
            new GridHolder(spaceRect, 10);

        }

        public void FindDoorIntersection(List<BSPMapComponent> corridors, int corridorSize) {
            foreach (BSPCorridor corridor in corridors) {

                if (corridor.spaceRect.xMin > this.spaceRect.xMin && corridor.spaceRect.xMax < this.spaceRect.xMax &&
                    corridor.spaceRect.yMin > this.spaceRect.yMin && corridor.spaceRect.yMax < this.spaceRect.yMax
                    ) {

                    continue;

                }

                if (!UtilityMethod.DoBoxesIntersect(this.spaceRect, corridor.spaceRect)) {
                    continue;
                }


                //Horizontal
                if (corridor.spaceRect.width != corridorSize) {
                    int y = (int)corridor.spaceRect.y;

                    if (corridor.spaceRect.xMin < this.spaceRect.xMin) {
                        doorPosition.Add(new Vector2Int((int)this.spaceRect.xMax, y));
                        doorPosition.Add(new Vector2Int((int)this.spaceRect.xMin - 1, y));

                        continue;
                    }

                    //Door at xMax side (right)
                    if (corridor.spaceRect.xMin < this.spaceRect.xMax && corridor.spaceRect.xMin > this.spaceRect.xMin)
                    {
                        doorPosition.Add(new Vector2Int((int)this.spaceRect.xMax, y));
                    }
                    else {
                        //Door at xMin side (left)
                        doorPosition.Add(new Vector2Int((int)this.spaceRect.xMin - 1, y));
                    }

                } else {
                    //Door at yMax side (up)
                    int x = (int)corridor.spaceRect.x;

                    if (corridor.spaceRect.yMin < this.spaceRect.yMin)
                    {
                        doorPosition.Add(new Vector2Int(x, (int)this.spaceRect.yMax));
                        doorPosition.Add(new Vector2Int(x, (int)this.spaceRect.yMin - 1));

                        continue;
                    }

                    if (corridor.spaceRect.yMin < this.spaceRect.yMax && corridor.spaceRect.yMin > this.spaceRect.yMin)
                    {
                        doorPosition.Add(new Vector2Int(x, (int)this.spaceRect.yMax));
                    }
                    else
                    {
                        //Door at yMin side (bottom)
                        doorPosition.Add(new Vector2Int(x, (int)this.spaceRect.yMin - 1));
                    }
                }
            }
        }

		public Rect CalculateRoomRect(Rect p_holderRect) {
			Vector2 roomPosition = new Vector2(
				UtilityMethod.RandRangeToInt(p_holderRect.x + (p_holderRect.width * 0.1f), p_holderRect.x + (p_holderRect.width * 0.3f)),
                UtilityMethod.RandRangeToInt(p_holderRect.y + (p_holderRect.height * 0.1f), p_holderRect.y + (p_holderRect.height* 0.3f))		
			);

			float maxWidth =  p_holderRect.width - (roomPosition.x - p_holderRect.x);
			float maxheight =  p_holderRect.height + (p_holderRect.y - roomPosition.y );


			Vector2 size = new Vector2(
                UtilityMethod.RandRangeToInt(maxWidth * 0.8f, maxWidth * 0.9f),
                UtilityMethod.RandRangeToInt(maxheight * 0.8f, maxheight * 0.9f)
			);
			return new Rect(roomPosition, size);
		}

        public class GridHolder {
            DataStructure.Grid[][] grids;

            public GridHolder(Rect roomSize, int p_grid_size)
            {
                float xLine = roomSize.width / p_grid_size;
                float yLine = roomSize.height / p_grid_size;
                //Debug.Log("roomSize.width " + roomSize.width + ", roomSize.height " + roomSize.height);

                //Debug.Log("Xline "+ xLine + ", yLine " + yLine);

            }
        }
		
	}
}