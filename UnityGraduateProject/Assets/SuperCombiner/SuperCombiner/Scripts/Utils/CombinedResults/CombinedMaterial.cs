using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LunarCatsStudio.SuperCombiner
{
    /// <summary>
    /// Stores information about a combined material
    /// </summary>
    [System.Serializable]
    public class CombinedMaterial
    {
        // The combined material
        public Material material;
        // The list of uvs
        public Rect[] uvs;
        public Rect[] uvs2;
        public List<float> scaleFactors = new List<float>();
        // List of meshes UV bound
        public List<Rect> meshUVBounds = new List<Rect>();
        // This will be true if there is only one material to combine, is this case we simply reuse the existing material
        public bool isOriginalMaterial = false;
        // The index at which this combined material will be displayed in the inspector. Usefull in multimaterial when some combined material are null
        public int displayedIndex;
        // Editor display parameter
        public bool showCombinedMaterial;
        public bool showUVs;
        public bool showMeshUVBounds;
    }
}
