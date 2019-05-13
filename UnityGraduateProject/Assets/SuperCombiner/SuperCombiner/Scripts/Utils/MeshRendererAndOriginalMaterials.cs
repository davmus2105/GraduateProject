using UnityEngine;
using System.Collections.Generic;

namespace LunarCatsStudio.SuperCombiner
{
    /// <summary>
    /// Data class used to store information about a mesh rendere (or skinned mesh renderer) 
    /// and all it's original materials, and eventually the list of splitted game objects
    /// </summary>
    class MeshRendererAndOriginalMaterials
    {
        /// <summary>
        /// The list of MeshRenderer
        /// </summary>
        public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        /// <summary>
        /// List of skinnedMeshRenderer
        /// </summary>
        public List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
        /// <summary>
        /// The list of original materials for each meshRenderers
        /// </summary>
        public List<Material[]> originalMaterials = new List<Material[]>();
        /// <summary>
        /// The list of original materials for each skinnedMeshRenderers
        /// </summary>
        public List<Material[]> originalskinnedMeshMaterials = new List<Material[]>();

        /// <summary>
        /// List of splitted GameObject created
        /// </summary>
        public List<GameObject> splittedGameObject = new List<GameObject>();
    }
}
