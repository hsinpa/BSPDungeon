using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STP {
    [CreateAssetMenu(fileName = "[STP]Tile", menuName = "STP/Tile", order = 1)]
    public class TileSTPObject : ScriptableObject
    {
        public string _tag;
        public Sprite[] sprites;

        public Sprite GetSprite() {
            if (sprites == null || sprites.Length <= 0) return null;

            return sprites[Random.Range(0, sprites.Length)];
        }
    }
}
