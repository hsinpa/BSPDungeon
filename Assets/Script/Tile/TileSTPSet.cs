using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP {
    [CreateAssetMenu(fileName = "[STP]TileSet", menuName = "STP/TileSet", order = 2)]
    public class TileSTPSet : ScriptableObject
    {
        public TileSTPObject[] tiles;

        public TileSTPObject GetTile(string p_tag) {
            if (tiles == null || tiles.Length <= 0) return null;

            for (int i = 0; i < tiles.Length; i++) {
                if (tiles[i]._tag == p_tag)
                    return tiles[i];
            }

            return null;
        }
    }
}
