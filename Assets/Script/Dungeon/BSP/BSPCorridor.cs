using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG {
	public class BSPCorridor : BSPMapComponent {
		public BSPCorridor(Rect p_holderRect) {
			//Calculate roomRect
			spaceRect = p_holderRect;
			display_order = 0;
		}
	}
}