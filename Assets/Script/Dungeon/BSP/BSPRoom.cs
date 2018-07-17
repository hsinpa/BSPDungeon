using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG {
	public class BSPRoom : BSPMapComponent {

		public BSPRoom(Rect p_holderRect) {
			//Calculate roomRect
			spaceRect = CalculateRoomRect(p_holderRect);
			display_order = 1;
		}

		public Rect CalculateRoomRect(Rect p_holderRect) {
			Vector2 roomPosition = new Vector2(
				Random.Range(p_holderRect.x + (p_holderRect.width * 0.1f), p_holderRect.x + (p_holderRect.width * 0.3f)),
				Random.Range(p_holderRect.y + (p_holderRect.height * 0.1f), p_holderRect.y + (p_holderRect.height* 0.3f))		
			);

			float maxWidth =  p_holderRect.width - (roomPosition.x - p_holderRect.x);
			float maxheight =  p_holderRect.height + (p_holderRect.y - roomPosition.y );


			Vector2 size = new Vector2(
				Random.Range(maxWidth * 0.8f, maxWidth * 0.9f),
				Random.Range(maxheight * 0.8f, maxheight * 0.9f)
			);
			return new Rect(roomPosition, size);
		}
		
		
	}
}