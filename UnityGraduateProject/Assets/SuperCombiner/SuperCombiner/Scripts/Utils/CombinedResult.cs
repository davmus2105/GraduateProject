using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace LunarCatsStudio.SuperCombiner 
{
	/// <summary>
	/// This class stores all the relevant data of a combined session
	/// </summary>
	public class CombinedResult : ScriptableObject
    {
        // Editor display parameters
        public bool showCombinedMaterials = false;
        public bool showCombinedMeshes = false;

        // The list of combined material
        public List<CombinedMaterial> combinedMaterials = new List<CombinedMaterial>();

		/// <summary>
		/// The list of dictionnaries of original materials to combine. There is one dicionnary by combinedIndex.
        /// Key = material's instanceID
		/// </summary>
		public List<Dictionary<int, MaterialToCombine>> originalMaterialList = new List<Dictionary<int, MaterialToCombine>>();
        /// <summary>
        /// Dictionnary of original and reference material's instanceID for each combinedIndex. The reference material is the one used and copied to create the combined material
        /// </summary>
		public Dictionary<int, int> originalReferenceMaterial = new Dictionary<int, int> ();

        /// <summary>
        /// List of generated combined GameObjects for meshes
        /// </summary>
        public List<List<GameObject>> combinedGameObjectFromMeshList = new List<List<GameObject>>();
        /// <summary>
        /// List of generated combined GameObjects for skinnedMeshes
        /// </summary>
        public List<List<GameObject>> combinedGameObjectFromSkinnedMeshList = new List<List<GameObject>>();

        /// <summary>
        /// The list of mesh results
        /// </summary>
		public List<MeshCombined> meshResults = new List<MeshCombined>();
        /// <summary>
        /// The number of original materials combined
        /// </summary>
        public int materialCombinedCount;
        /// <summary>
        /// The number of combined material created. If multimaterial is used, more than one material should be created
        /// </summary>
        public int combinedMaterialCount;
        /// <summary>
        /// The number of original meshes combined
        /// </summary>
        public int meshesCombinedCount;
        /// <summary>
        /// The number of skinnedMeshes combined
        /// </summary>
        public int skinnedMeshesCombinedCount;
        /// <summary>
        /// The number of vertex in combined mesh
        /// </summary>
        public int totalVertexCount;
        /// <summary>
        /// The number of submeshes
        /// </summary>
        public int subMeshCount;
        /// <summary>
        /// The duration of the process
        /// </summary>
        public TimeSpan duration;

        /// <summary>
        /// Clear all combine data
        /// </summary>
        public void Clear()
        {
            if (originalMaterialList != null)
            {
                for(int i=0; i<originalMaterialList.Count; i++)
                {
                    originalMaterialList[i].Clear();
                }
                originalMaterialList.Clear();
            }
			originalReferenceMaterial.Clear();
            materialCombinedCount = 0;
            combinedMaterials.Clear();
            combinedMaterialCount = 0;
            meshesCombinedCount = 0;
            skinnedMeshesCombinedCount = 0;
            totalVertexCount = 0;
            subMeshCount = 0;
            meshResults.Clear();
            combinedGameObjectFromMeshList.Clear();
            combinedGameObjectFromSkinnedMeshList.Clear();
        }

        /// <summary>
        /// Sets a combined material
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="combinedIndex"></param>
        /// <param name="isOriginal"></param>
        public void SetCombinedMaterial(Material mat, int combinedIndex, bool isOriginal)
        {
            if(combinedIndex < combinedMaterials.Count)
            {
                combinedMaterials[combinedIndex].material = mat;
                if(!isOriginal)
                {
                    combinedMaterials[combinedIndex].material.name += "_" + combinedMaterialCount;
                }
                combinedMaterials[combinedIndex].displayedIndex = combinedMaterialCount;
                combinedMaterials[combinedIndex].isOriginalMaterial = isOriginal;
            }
            combinedMaterialCount++;
        }

        /// <summary>
        /// Add a new combined material to the list
        /// </summary>
        public void AddNewCombinedMaterial()
        {
            combinedMaterials.Add(new CombinedMaterial());
        }

        /// <summary>
        /// Adds a new material to be combined
        /// </summary>
        /// <param name="mat"></param>
        /// <param name="combinedIndex"></param>
        public void AddMaterialToCombine(MaterialToCombine mat, int combinedIndex)
		{
			if (!originalReferenceMaterial.ContainsKey(combinedIndex)) {
				originalReferenceMaterial.Add (combinedIndex, mat.material.GetInstanceID ());
			}
            mat.index = originalMaterialList[combinedIndex].Count;
            originalMaterialList[combinedIndex].Add(mat.material.GetInstanceID(), mat);

            //Debug.Log("Adding new material to combine: " + mat.material.name + " at combinedIndex " + combinedIndex);
		}

        /// <summary>
        /// Add a new CombinedMesh
        /// </summary>
        /// <param name="combinedMesh"></param>
        /// <param name="combineInstanceID"></param>
        /// <param name="combinedIndex"></param>
        public void AddCombinedMesh(Mesh combinedMesh, CombineInstanceID combineInstanceID, int combinedIndex)
        {
			MeshCombined meshResult = new MeshCombined ();

            int vertexIndex = 0;
            int triangleIndex = 0;
            for (int i = 0; i < combineInstanceID.combineInstances.Count; i++) {
				if(!meshResult.instanceIds.Contains(combineInstanceID.instancesID[i]))
                {
                    vertexIndex += combineInstanceID.combineInstances[i].mesh.vertexCount;
                    triangleIndex += combineInstanceID.combineInstances[i].mesh.triangles.Length;
                    meshResult.names.Add(combineInstanceID.names[i]);
                    meshResult.instanceIds.Add(combineInstanceID.instancesID[i]);
					meshResult.indexes.Add(new CombineInstanceIndexes(combineInstanceID.combineInstances[i].mesh, vertexIndex, triangleIndex));
                }
            }

            meshResults.Add(meshResult);
        }

        /// <summary>
        /// Returns the index of a given material in the list 
        /// </summary>
        /// <param name="matToFind"></param>
        /// <param name="combinedIndex"></param>
        /// <returns></returns>
        public int FindCorrespondingMaterialIndex(Material matToFind, int combinedIndex)
        {
            if (combinedIndex < originalMaterialList.Count)
            {
                if (originalMaterialList[combinedIndex].ContainsKey(matToFind.GetInstanceID())) {
                    return originalMaterialList[combinedIndex][matToFind.GetInstanceID()].index;
                }
            }
            Debug.LogWarning("[Super Combiner] Material " + matToFind + " was not found in list " + combinedIndex);
            return 0;
		}

        /// <summary>
        /// Get the combined material associated to the source material in parameter
        /// </summary>
        /// <param name="sourceMaterial"></param>
        /// <returns></returns>
        public Material GetCombinedMaterial(Material sourceMaterial)
        {
            for(int i=0; i<originalMaterialList.Count; i++)
            {
				if (originalMaterialList [i].ContainsKey (sourceMaterial.GetInstanceID ())) 
				{
					return combinedMaterials[i].material;
				}                
            }

            Debug.LogWarning("[Super Combiner] Could not find combined material associated with " + sourceMaterial.name);
            return null;
        }

		/// <summary>
		/// Gets the combined index of the given material
		/// </summary>
		/// <returns>The combined index.</returns>
		/// <param name="sourceMaterial">Material.</param>
		public int GetCombinedIndex(Material sourceMaterial) 
		{
			for(int i=0; i<originalMaterialList.Count; i++)
			{
				if (originalMaterialList [i].ContainsKey (sourceMaterial.GetInstanceID ())) 
				{
					return i;
				}
			}

			Debug.LogWarning("[Super Combiner] Could not find combined material associated with " + sourceMaterial.name);
			return 0;
		}

        /// <summary>
        /// Return the number of combined material
        /// </summary>
        /// <returns></returns>
        public int GetCombinedIndexCount()
        {
            return originalMaterialList.Count;
        }
	}
}
