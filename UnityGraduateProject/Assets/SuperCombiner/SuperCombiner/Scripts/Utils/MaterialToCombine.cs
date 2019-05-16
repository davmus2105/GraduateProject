using UnityEngine;
using System.Collections;

namespace LunarCatsStudio.SuperCombiner
{
	/// <summary>
	/// Class storing data of a single material to combine
	/// </summary>
    public class MaterialToCombine
    {
        /// <summary>
		///  The material to combine
        /// </summary>
        public Material material;
        /// <summary>
		/// The maximum uv bounds for this material
		/// </summary>
        public Rect uvBounds;
		/// <summary>
		/// The index of the combined material
		/// </summary>
		public int combinedIndex;
        /// <summary>
        /// Index of this element in it's list. This index is equal to the combinedIdex
        /// </summary>
        public int index;

        /// <summary>
        /// Get the scaled and offseted by material parameters uv bounds
        /// </summary>
        /// <returns></returns>
        public Rect GetScaledAndOffsetedUVBounds()
        {
            Rect rect = uvBounds;
            if (material.HasProperty("_MainTex"))
            {
                rect.size = Vector2.Scale(rect.size, material.mainTextureScale);
                rect.position += material.mainTextureOffset;
            }
            return rect;
        }
    }

}
