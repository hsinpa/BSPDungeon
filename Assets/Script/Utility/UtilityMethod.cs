using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Utility {
	public class UtilityMethod {
		

	    /// <summary>
        ///  Load single sprite from multiple mode
        /// </summary>
        /// <param name="spriteArray"></param>
        /// <param name="spriteName"></param>
        /// <returns></returns>
		public static Sprite LoadSpriteFromMulti(Sprite[] spriteArray, string spriteName) {
			foreach (Sprite s in spriteArray) {
				
				if (s.name == spriteName) return s;
			}
			return null;
		}

		/// <summary>
        /// Clear every child gameobject
        /// </summary>
        /// <param name="parent"></param>
        public static void ClearChildObject(Transform parent) {
            foreach (Transform t in parent) {
				if (Application.isEditor) {
					Debug.Log("HEOOL");
					Object.DestroyImmediate(t);
				} else {
					GameObject.Destroy(t.gameObject);
				}
            }
        }

        /// <summary>
        ///  Insert gameobject to parent
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static GameObject CreateObjectToParent(Transform parent, GameObject prefab) {
            GameObject item = GameObject.Instantiate(prefab);
            item.transform.SetParent(parent);
            item.transform.localScale = Vector3.one;
			item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, 1);
			item.transform.localPosition = new Vector3( 0, 0, 1);
            return item;
        }

		public static GameObject FindObject(GameObject parent, string name) {
		     Transform[] trs= parent.GetComponentsInChildren<Transform>(true);
		     foreach(Transform t in trs){
		         if(t.name == name){
		              return t.gameObject;
		         }
		     }
		     return null;
		 }


		/// <summary>
		/// Rolls the dice, only return 1 or 0.
		/// </summary>
		/// <returns>The dice.</returns>
		public static int RollDice() {
			return Random.Range(0,2);
		}
		
		/// <summary>
		/// Possibilities the match.
		/// </summary>
		/// <returns><c>true</c>, if match was possibilityed, <c>false</c> otherwise.</returns>
		public static bool PercentageGame(float rate) {
			float testValue =Random.Range(0f ,1f);
			return ( rate >= testValue ) ? true : false;
		}

		public static T PercentageTurntable<T>(T[] p_group, float[] percent_array) {
			float percent = Random.Range(0f, 100f);
			float max = 100;

			for (int i = 0 ; i < percent_array.Length; i++) {
				float newMax = max - percent_array[i];
				if (max >= percent && newMax <= percent ) return p_group[i];

				max = newMax;
			}
			return default (T);
		}

		public static T PercentageTurntable<T>(T[] p_group, int[] percent_array) {
			float[] convertFloat = System.Array.ConvertAll(percent_array, s => (float)s);
			return PercentageTurntable<T>(p_group, convertFloat);
		}
	}
}