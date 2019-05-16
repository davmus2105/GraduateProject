using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using LunarCatsStudio.SuperCombiner;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace LunarCatsStudio.SuperCombiner {
	/// <summary>
	/// Main class of Super Combiner asset.
	/// </summary>
	public class SuperCombiner : MonoBehaviour
    {
		public enum CombineStatesList {Uncombined, Combining, CombinedMaterials, Combined}
		public List<int> TextureAtlasSizes = new List<int>() { 
			32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 
		};
		public List<string> TextureAtlasSizesNames = new List<string>() { 
			"32", "64", "128", "256", "512", "1024", "2048", "4096", "8192"
		};

		public CombineStatesList combiningState = CombineStatesList.Uncombined;
		public List<LunarCatsStudio.SuperCombiner.TexturePacker> texturePackers = new List<LunarCatsStudio.SuperCombiner.TexturePacker>();
		public LunarCatsStudio.SuperCombiner.MeshCombiner meshCombiner = new LunarCatsStudio.SuperCombiner.MeshCombiner();

        // Editor Foldouts
        public bool _showInstructions = true;
        public bool _showCombineSettings = false;
        public bool _showMeshSettings = false;
        public bool _showTextureSettings = true;
        public bool _showAdditionalParameters = false;
        public bool _showMeshResults = false;
        public bool _showOriginalMaterials = false;
        public bool _showCombinedAtlas = false;
        public bool _showCombinedMaterials = false;
        public bool _showCombinedMesh = false;
        public bool _showSaveOptions = false;
        public bool _showMultiMaterials = false;

        // Editable Parameters
        public string sessionName = "combinedSession";
		public bool combineAtRuntime = false;
		public int textureAtlasSize = 1024;
		public List<string> customTextureProperies = new List<string> ();
	    public float tilingFactor = 1f;
        public int atlasPadding = 0;
        public bool combineMeshes = false;
		public int meshOutput;
		public int maxVerticesCount = 65534;
		public bool combineMaterials = true;
	    public GameObject targetGameObject;

		// Multiple materials
		public bool multipleMaterialsMode = false;
		public List<Material> multiMaterials0 = new List<Material>();
		public List<Material> multiMaterials1 = new List<Material>();
		public List<Material> multiMaterials2 = new List<Material>();
        public List<Material> multiMaterials3 = new List<Material>();
        public List<Material> multiMaterials4 = new List<Material>();
        public List<Material> multiMaterials5 = new List<Material>();
        public List<Material> multiMaterials6 = new List<Material>();
        public List<Material> multiMaterials7 = new List<Material>();
        public List<Material> multiMaterials8 = new List<Material>();
        public List<Material> multiMaterials9 = new List<Material>();
        public List<Material> multiMaterials10 = new List<Material>();
        public List<Material> multiMaterialsAllOthers = new List<Material>();
        /// <summary>
        /// The list of multi material list defined by user
        /// </summary>
        public List<List<Material>> multiMaterialsList = new List<List<Material>>();
        public int _multiMaterialsCount;
        /// <summary>
        /// The list of material list to combine
        /// </summary>
        List<List<MaterialToCombine>> materialsToCombine = new List<List<MaterialToCombine>>();

        // Saving options
        public bool savePrefabs = true;
		public bool saveMeshObj = false;
		public bool saveMeshFbx = false;
		public bool saveMaterials = true;
		public bool saveTextures = true;
		public string folderDestination = "Assets/SuperCombiner/Combined";

        // Internal combine process variables
        /// <summary>
        /// List of all original MeshRenderer in children to combine
        /// </summary>
        public List<MeshRenderer> meshList = new List<MeshRenderer>();
        /// <summary>
		/// List of all original SkinnedMeshRenderer in children to combine
        /// </summary>
        public List<SkinnedMeshRenderer> skinnedMeshList = new List<SkinnedMeshRenderer>();
        /// <summary>
        /// List of copied meshes instancesId associated with their original sharedMesh and sharedMaterial instanceId. 
        /// This is usefull not to save duplicated mesh when exporting
		/// Key=Mesh instanceID
		/// Value=Concatenation of sharedMeshName + sharedMaterialName + GameObjectName
        /// </summary>
        public Dictionary<int, string> uniqueCombinedMeshId = new Dictionary<int, string>();
        /// <summary>
        /// Links original shared meshes with the copy created
		/// Key=original shared mesh instanceID
		/// Value=The value of uniqueCombinedMeshId[Key]
        /// </summary>
		public Dictionary<int, string> copyMeshId = new Dictionary<int, string>();        
        /// <summary>
        /// List of transformed game objects for prefab saving
        /// </summary>
        public List<GameObject> toSavePrefabList = new List<GameObject>();
        /// <summary>
        /// List of transformed game objects for saving purpose
        /// </summary>
        public List<MeshRenderer> toSaveObjectList = new List<MeshRenderer>();
        /// <summary>
        /// List of meshes to save
        /// </summary>
        public List<Mesh> toSaveMeshList = new List<Mesh>();
        /// <summary>
        /// List of transformed skinned game objects for saving purpose
        /// </summary>
        public List<SkinnedMeshRenderer> toSaveSkinnedObjectList = new List<SkinnedMeshRenderer>();
        // <summary>
        // CombinedGameObjects[i] will use uvs[combinedTextureIndex[i]]
        // </summary>
        //public List<int> combinedTextureIndex = new List<int>();

		/// <summary>
		/// The parent GameObject for every combined object
		/// </summary>
		public GameObject targetParentForCombinedGameObjects;

	    private DateTime timeStart;             // The date time when starting the process

	    /// <summary>
		/// The result of the combine process is stored in this class
	    /// </summary>
	    public CombinedResult combinedResult;

		private SuperCombiner() {
			// Nothing to do here
		}

		void Start() {
			if (combineAtRuntime) {
	            CombineChildren();
			}
		}

        /// <summary>
        /// Find and fill the list of enabled meshes to combine
        /// </summary>
        public void FindMeshesToCombine()
        {
            meshList = FindEnabledMeshes(transform);
            skinnedMeshList = FindEnabledSkinnedMeshes(transform);
        }

        /// <summary>
        /// Combine process
        /// </summary>
        public void CombineChildren()
		{
	        timeStart = DateTime.Now;
	        combiningState = CombineStatesList.Combining;

            // Getting the list of meshes ...
            FindMeshesToCombine();

            Combine(meshList, skinnedMeshList);
	    }

        /// <summary>
	    /// Combine Materials and Create Atlas texture
	    /// </summary>
	    /// <param name="meshesToCombine"></param>
	    /// <param name="skinnedMeshesToCombine"></param>
        /// <returns>True if process has been successfull</returns>
	    public bool CombineMaterials(List<MeshRenderer> meshesToCombine, List<SkinnedMeshRenderer> skinnedMeshesToCombine)
	    {
#if UNITY_EDITOR
	        // UI Progress bar display in Editor
	        EditorUtility.DisplayProgressBar("Super Combiner", "Materials and textures listing...", 0.1f);
#endif
            // Initialize multi material parameters
            InitializeMultipleMaterialElements();

            // If combinedResult has not been created yet, create it
            if (combinedResult == null)
            {
                combinedResult = (CombinedResult)ScriptableObject.CreateInstance(typeof(CombinedResult));
            }

            // Getting list of materials
            List<MaterialToCombine> enabledMaterials = FindEnabledMaterials(meshesToCombine, skinnedMeshesToCombine);
            combinedResult.materialCombinedCount = enabledMaterials.Count;

            foreach (MaterialToCombine mat in enabledMaterials)
            {
                bool found = false;
                for (int i = 0; i < multiMaterialsList.Count; i++)
                {
                    if (multiMaterialsList[i].Contains(mat.material))
                    {
                        // This material was listed in the multi material list by user, we add it to it's right index in 'materialsToCombine' list
                        materialsToCombine[i].Add(mat);
                        found = true;
                    }
                }
                if(!found)
                {
                    // This material was not listed in the multi material list, so we add it to the last element
                    materialsToCombine[materialsToCombine.Count - 1].Add(mat);
                }
            }

            //  List all texture from enabled materials to be combined
            int progressCount = 0;
            for (int i=0; i<materialsToCombine.Count; i++)
            {
				combinedResult.originalMaterialList.Add(new Dictionary<int, MaterialToCombine>());
                combinedResult.AddNewCombinedMaterial();
				if (i == multiMaterialsList.Count && materialsToCombine[i].Count > 0 || i < multiMaterialsList.Count && multiMaterialsList[i].Count > 0)
                {
                    // Instanciate a new texture Packer
                    TexturePacker texturePacker = new TexturePacker
                    {
                        // Assign the combinedResult reference to texturePacker
                        CombinedResult = combinedResult,
                        CombinedIndex = i
					};

					// Setting up the custom shader property names
					texturePacker.SetCustomPropertyNames (customTextureProperies);
					// Add this texture packer to the list
					texturePackers.Add (texturePacker);

					foreach (MaterialToCombine mat in materialsToCombine[i])
                    {
						combinedResult.AddMaterialToCombine (mat, i);
#if UNITY_EDITOR
                        // Cancelable  UI Progress bar display in Editor
                        bool cancel = false;
                        EditorUtility.DisplayProgressBar ("Super Combiner", "Processing material " + mat.material.name, progressCount / (float)enabledMaterials.Count);
						if (cancel)
                        {
							UnCombine ();
							return false;
						}
#endif
						// Add all textures from this material on the list of textures
						texturePacker.SetTextures (mat.material, combineMaterials, mat.GetScaledAndOffsetedUVBounds (), mat.uvBounds, tilingFactor);                        

						/*if (!mat.HasProperty ("_MainTex") || mat.mainTexture == null) {
                        // Correction of uv for mesh without diffuse texture
                            uvBound.size = Vector2.Scale (uvBound.size, new Vector2 (1.2f, 1.2f));
                            uvBound.position -= new Vector2 (0.1f, 0.1f);
                        }*/
						progressCount++;
					}

					if (materialsToCombine [i].Count == 0)
                    {
                        if(multiMaterialsList[i].Count == 0)
                        {
                            Debug.LogWarning("[Super Combiner] Source materials group " + i + " is empty. Skipping this combine process");
                        } else
                        {
						    Debug.LogWarning ("[Super Combiner] Cannot combined materials for group " + i + " because none of the material were found in the list of game objects to combine");
                        }
					}
                    else if (materialsToCombine [i].Count == 1)
                    {
						Debug.Log ("[Super Combiner] Only one material found for group " + i + ", skipping combine material process and keep this material for the combined mesh.");
                        combinedResult.SetCombinedMaterial(materialsToCombine[i][0].material, i, true);
						combinedResult.combinedMaterials [i].uvs = new Rect[1];
						combinedResult.combinedMaterials [i].uvs [0] = new Rect (0, 0, 1, 1);
                        texturePacker.SetCopyedMaterial(materialsToCombine[i][0].material);
                    } else
                    {
#if UNITY_EDITOR
						// UI Progress bar display in Editor
						EditorUtility.DisplayProgressBar ("Super Combiner", "Packing textures...", 0f);
#endif
						// Pack the textures
						texturePacker.PackTextures (textureAtlasSize, atlasPadding, combineMaterials, sessionName);
					}
				} else
                {
					// There are no materials to combine in this combinedIndex
					texturePackers.Add (null);
				}
            }

            combiningState = CombineStatesList.CombinedMaterials;
#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
            return false;
        }

        public void SetTargetParentForCombinedGameObject()
        {
            if (targetGameObject == null)
            {
                // Create the parent Game object
                targetParentForCombinedGameObjects = new GameObject(sessionName);
                targetParentForCombinedGameObjects.transform.parent = this.transform;
                targetParentForCombinedGameObjects.transform.localPosition = Vector3.zero;
            }
            else
            {
                targetParentForCombinedGameObjects = targetGameObject;
            }
        }

		/// <summary>
		/// Combines the meshes
		/// </summary>
		/// <param name="meshesToCombine">Meshes to combine.</param>
		/// <param name="skinnedMeshesToCombine">Skinned meshes to combine.</param>
	    public void CombineMeshes(List<MeshRenderer> meshesToCombine, List<SkinnedMeshRenderer> skinnedMeshesToCombine, Transform parent)
	    {
            // Assign the combinedResult reference to texturePacker and MeshCombiner
            meshCombiner.CombinedResult = combinedResult;

	        combinedResult.meshesCombinedCount = meshesToCombine.Count;
	        combinedResult.skinnedMeshesCombinedCount = skinnedMeshesToCombine.Count;

	        // Check if there is at least 2 meshes in the current gameobject
	        if (combineMeshes)
	        {
	            if (meshesToCombine.Count + skinnedMeshesToCombine.Count < 1)
	            {
	                if (meshesToCombine.Count == 0)
	                {
	#if UNITY_EDITOR
	                    EditorUtility.DisplayDialog("Super Combiner", "Zero meshes found.\nUnable to proceed without at least 1 mesh.", "Ok");
	#endif
	                    UnCombine();
	                }
	                return;
	            }
	        }

	        // Parametrize MeshCombiner
	        meshCombiner.SetParameters(maxVerticesCount, sessionName);

#if UNITY_EDITOR
            // UI Progress bar display in Editor
            EditorUtility.DisplayProgressBar("Super Combiner", "Combining meshes", 0.5f);
#endif

            // Combine process
            if (combineMeshes)
			{
                // Get the ordered by combinedIndex list of meshes to combine
                List<MeshRendererAndOriginalMaterials> meshIndexedList = GetMeshRenderersByCombineIndex(meshesToCombine, skinnedMeshesToCombine, targetParentForCombinedGameObjects.transform);                

                for (int i=0; i<combinedResult.GetCombinedIndexCount(); i++)
                {
                    combinedResult.combinedGameObjectFromMeshList.Add(new List<GameObject>());
                    combinedResult.combinedGameObjectFromSkinnedMeshList.Add(new List<GameObject>());

                    if (combinedResult.originalMaterialList[i].Count > 0)
                    {
                        if ((LunarCatsStudio.SuperCombiner.MeshOutput)meshOutput == LunarCatsStudio.SuperCombiner.MeshOutput.Mesh)
                        {
                            // Combine the meshes together
                            combinedResult.combinedGameObjectFromMeshList[i] = meshCombiner.CombineToMeshes(meshIndexedList[i].meshRenderers, meshIndexedList[i].skinnedMeshRenderers, parent, i);

                            // Add the copy mesh instanceId with its original sharedMesh and sharedMaterial instanceId
                            foreach (GameObject go in combinedResult.combinedGameObjectFromMeshList[i])
                            {
                                uniqueCombinedMeshId.Add(go.GetComponent<MeshFilter>().sharedMesh.GetInstanceID(), go.name);
                            }
                        }
                        else
                        {
                            combinedResult.combinedGameObjectFromSkinnedMeshList[i] = meshCombiner.CombineToSkinnedMeshes(meshIndexedList[i].meshRenderers, meshIndexedList[i].skinnedMeshRenderers, parent, i);

                            // Add the copy mesh instanceId with its original sharedMesh and sharedMaterial instanceId
                            foreach (GameObject go in combinedResult.combinedGameObjectFromSkinnedMeshList[i])
                            {
                                uniqueCombinedMeshId.Add(go.GetComponent<SkinnedMeshRenderer>().sharedMesh.GetInstanceID(), go.name);
                            }
                        }

                        if (combinedResult.combinedGameObjectFromMeshList.Count + combinedResult.combinedGameObjectFromSkinnedMeshList.Count == 0)
                        {
                            Debug.LogError("[Super Combiner] No mesh could be combined");
                            // Error, Nothing could be combined
                            //UnCombine();
                            //return;
                        }
                    }
                    // Remove all temporary splitted GameObjects created
                    for(int j=0; j< meshIndexedList[i].splittedGameObject.Count; j++)
                    {
                        DestroyImmediate(meshIndexedList[i].splittedGameObject[j]);
                    }
                }
			}
			else
			{
				// Create a copy of all game objects children of this one
				CopyGameObjectsHierarchy(parent);

	            List<MeshRenderer> copyMeshList = FindEnabledMeshes(parent);
				List<SkinnedMeshRenderer> copySkinnedMeshList = FindEnabledSkinnedMeshes(parent);

                // Get the ordered by combinedIndex list of meshes to combine
                List<MeshRendererAndOriginalMaterials> copyMeshIndexedList = GetMeshRenderersByCombineIndex(copyMeshList, copySkinnedMeshList, null);
                
                for (int i = 0; i < combinedResult.GetCombinedIndexCount(); i++) 
				{
                    combinedResult.combinedGameObjectFromMeshList.Add(new List<GameObject>());
                    combinedResult.combinedGameObjectFromSkinnedMeshList.Add(new List<GameObject>());

                    // Generate the new GameObjects and assign combined materials to renderers
                    if (copyMeshIndexedList[i].meshRenderers.Count > 0)
                    {
                        combinedResult.combinedGameObjectFromMeshList[i].AddRange(GenerateTransformedGameObjects(parent, copyMeshIndexedList[i].meshRenderers));
                    }
                    if (copyMeshIndexedList[i].skinnedMeshRenderers.Count > 0)
                    {
                        combinedResult.combinedGameObjectFromSkinnedMeshList[i].AddRange(GenerateTransformedGameObjects(parent, copyMeshIndexedList[i].skinnedMeshRenderers));
                    }

                    // Generate new UVs only if there are more than 1 material combined
                    if (combinedResult.originalMaterialList[i].Count > 1)
                    {
                        for (int j = 0; j < copyMeshIndexedList[i].meshRenderers.Count; j++)
                        {
                            GenerateUVs(copyMeshIndexedList[i].meshRenderers[j].GetComponent<MeshFilter>().sharedMesh, copyMeshIndexedList[i].originalMaterials[j], copyMeshIndexedList[i].meshRenderers[j].name, i);
                        }
                        for (int j = 0; j < copyMeshIndexedList[i].skinnedMeshRenderers.Count; j++)
                        {
                            GenerateUVs(copyMeshIndexedList[i].skinnedMeshRenderers[j].sharedMesh, copyMeshIndexedList[i].originalskinnedMeshMaterials[j], copyMeshIndexedList[i].skinnedMeshRenderers[j].name, i);
                        }
                    }
				}
			}
            combiningState = CombineStatesList.Combined;
            // Deactivate original renderers
            DisableRenderers(meshList, skinnedMeshList);

#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
        }

        /// <summary>
        /// Return the list of MeshRendererAndOriginalMaterials for a given list of MeshRenderer and SkinnedMeshRenderer to combine.
        /// The returned list also contains the list of splitted submeshes if this was necessary
        /// </summary>
        /// <param name="meshRenderers"></param>
        /// <param name="skinnedMeshRenderers"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private List<MeshRendererAndOriginalMaterials> GetMeshRenderersByCombineIndex(List<MeshRenderer> meshRenderers, List<SkinnedMeshRenderer> skinnedMeshRenderers, Transform parent)
        {
            // The list to be returned
            List<MeshRendererAndOriginalMaterials> meshRenderersByCombineIndex = new List<MeshRendererAndOriginalMaterials>();
            // A temporary list of list of submeshes indexes to be splitted
            List<List<int>> submeshToCombinedIndex = new List<List<int>>();

            if(combinedResult.originalMaterialList.Count == 0)
            {
                Debug.LogError("[Super Combiner] List of materials to combine has been lost. Try to uncombine and combine again.");
                return meshRenderersByCombineIndex;
            }

            // Initialize lists
            for(int i=0; i<combinedResult.originalMaterialList.Count; i++)
            {
                meshRenderersByCombineIndex.Add(new MeshRendererAndOriginalMaterials());
                submeshToCombinedIndex.Add(new List<int>());
            }

            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                Material[] materials = meshRenderer.sharedMaterials;
                // We assume here the number of sharedMaterials is equal to the number of submeshes
                combinedResult.subMeshCount += materials.Length - 1;
                // List all combinedIndex for each material in this meshRenderer
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null)
                    {
                        int index = combinedResult.GetCombinedIndex(materials[i]);
                        submeshToCombinedIndex[index].Add(i);
                    } else
                    {
                        Debug.LogWarning("[Super Combiner] MeshRenderer of '" + meshRenderer.name + "' has some missing material references.");
                    }
                }
                // If needed, split the submeshes
                bool hasSplitSubmeshes = false;
                for (int i = 0; i < combinedResult.originalMaterialList.Count; i++)
                {
                    if (submeshToCombinedIndex[i].Count > 0)
                    {
                        if (submeshToCombinedIndex[i].Count < materials.Length)
                        {
                            // Some materials in this meshRenderer correspond to different combined index, split submesh accordingly
                            MeshRenderer newMesh = SubmeshSplitter.SplitSubmeshes(meshRenderer.GetComponent<MeshFilter>(), submeshToCombinedIndex[i].ToArray(), i);
                            meshRenderersByCombineIndex[i].meshRenderers.Add(newMesh);
                            meshRenderersByCombineIndex[i].originalMaterials.Add(newMesh.sharedMaterials);
                            meshRenderersByCombineIndex[i].splittedGameObject.Add(newMesh.gameObject);
                            // Debug.Log("[Super Combiner] Splitting submeshes for " + meshRenderer);
                            hasSplitSubmeshes = true;
                        }
                        else
                        {
                            // All materials in this meshRenderer correspond to the same combined index, no need to split submesh
                            meshRenderersByCombineIndex[i].meshRenderers.Add(meshRenderer);
                            meshRenderersByCombineIndex[i].originalMaterials.Add(meshRenderer.sharedMaterials);
                        }
                    }
                }
                // If mesh has been splitted we don't combine mesh, destroy the old meshRenderer and MeshFilter component because there are copies that won't be used anymore
                if (hasSplitSubmeshes && parent == null)
                {
                    DestroyImmediate(meshRenderer.GetComponent<MeshFilter>());
                    DestroyImmediate(meshRenderer);
                }
                // Clear the combined index list
                for (int i = 0; i < combinedResult.originalMaterialList.Count; i++)
                {
                    submeshToCombinedIndex[i].Clear();
                }
            }

            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                Material[] materials = skinnedMeshRenderer.sharedMaterials;
                combinedResult.subMeshCount += materials.Length - 1;
                // List all combinedIndex for each material in this meshRenderer
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null)
                    {
                        int index = combinedResult.GetCombinedIndex(materials[i]);
                        submeshToCombinedIndex[index].Add(i);
                    }
                    else
                    {
                        Debug.LogWarning("[Super Combiner] SkinnedMeshRenderer of '" + skinnedMeshRenderer.name + "' has some missing material references.");
                    }
                }
                // If needed, split the submeshes
                bool hasSplitSubmeshes = false;
                for (int i = 0; i < combinedResult.originalMaterialList.Count; i++)
                {
                    if (submeshToCombinedIndex[i].Count > 0)
                    {
                        if (submeshToCombinedIndex[i].Count < materials.Length)
                        {
                            // Some materials in this meshRenderer correspond to different combined index, split submesh accordingly
                            SkinnedMeshRenderer newMesh = SubmeshSplitter.SplitSubmeshes(skinnedMeshRenderer, submeshToCombinedIndex[i].ToArray(), i);
                            meshRenderersByCombineIndex[i].skinnedMeshRenderers.Add(newMesh);
                            meshRenderersByCombineIndex[i].originalskinnedMeshMaterials.Add(newMesh.sharedMaterials);
                            meshRenderersByCombineIndex[i].splittedGameObject.Add(newMesh.gameObject);                            
                            // Debug.Log("[Super Combiner] Splitting submeshes for " + skinnedMeshRenderer);
                            hasSplitSubmeshes = true;
                        }
                        else
                        {
                            // All materials in this meshRenderer correspond to the same combined index, no need to split submesh
                            meshRenderersByCombineIndex[i].skinnedMeshRenderers.Add(skinnedMeshRenderer);
                            meshRenderersByCombineIndex[i].originalskinnedMeshMaterials.Add(skinnedMeshRenderer.sharedMaterials);
                        }
                    }
                }
                // If mesh has been splitted we don't combine mesh, destroy the old meshRenderer and MeshFilter component because there are copies that won't be used anymore
                if (hasSplitSubmeshes && parent == null)
                {
                    DestroyImmediate(skinnedMeshRenderer.GetComponent<MeshFilter>());
                    DestroyImmediate(skinnedMeshRenderer);
                }
                // Clear the combined index list
                for (int i = 0; i < combinedResult.originalMaterialList.Count; i++)
                {
                    submeshToCombinedIndex[i].Clear();
                }
            }

            return meshRenderersByCombineIndex;
        }        

        /// <summary>
        /// Combine the specified MeshRenderers and SkinnedMeshRenderers
        /// </summary>
        /// <param name="meshesToCombine">Meshes to combine.</param>
        /// <param name="skinnedMeshesToCombine">Skinned meshes to combine.</param>
        public void Combine(List<MeshRenderer> meshesToCombine, List<SkinnedMeshRenderer> skinnedMeshesToCombine)
	    {
            // Start timer if necessary
	        if(combiningState == CombineStatesList.Uncombined)
	        {
	            timeStart = DateTime.Now;
	            combiningState = CombineStatesList.Combining;
	        }

	        Debug.Log("[Super Combiner] Start processing...");

#if UNITY_EDITOR
	        // UI Progress bar display in Editor
	        EditorUtility.DisplayProgressBar("Super Combiner", "Meshes listing...", 0.1f);
#endif            

            // Combine Materials
            bool cancel = CombineMaterials(meshesToCombine, skinnedMeshesToCombine);
            if (cancel)
            {
#if UNITY_EDITOR
                EditorUtility.ClearProgressBar();
#endif
                return;
            }

            // Initialte target parent gameObject
            SetTargetParentForCombinedGameObject();

            // Combine Meshes
            CombineMeshes(meshesToCombine, skinnedMeshesToCombine, targetParentForCombinedGameObjects.transform);

	#if UNITY_EDITOR
	        // Combine process is finished
	        EditorUtility.ClearProgressBar();
	#endif

	        // Process is finished
	        combiningState = CombineStatesList.Combined;
	        combinedResult.duration = DateTime.Now - timeStart;
	        Debug.Log("[Super Combiner] Successfully combined game objects!\nExecution time is " + combinedResult.duration);
	    }

        /// <summary>
        /// Initialize multiple material elements
        /// </summary>
        private void InitializeMultipleMaterialElements()
        {
            if (multipleMaterialsMode)
            {
                multiMaterialsList.Add(multiMaterials0);
                multiMaterialsList.Add(multiMaterials1);
                multiMaterialsList.Add(multiMaterials2);
                multiMaterialsList.Add(multiMaterials3);
                multiMaterialsList.Add(multiMaterials4);
                multiMaterialsList.Add(multiMaterials5);
                multiMaterialsList.Add(multiMaterials6);
                multiMaterialsList.Add(multiMaterials7);
                multiMaterialsList.Add(multiMaterials8);
                multiMaterialsList.Add(multiMaterials9);
                multiMaterialsList.Add(multiMaterials10);
            }
            // Fill the materials to combine list
            for (int i = 0; i < multiMaterialsList.Count + 1; i++)
            {
                // The last one in this list correspond to all other materials
                materialsToCombine.Add(new List<MaterialToCombine>());
            }
        }

        /// <summary>
        /// Copy all GameObjects children
        /// </summary>
        /// <param name="parent"></param>
        private void CopyGameObjectsHierarchy(Transform parent) {
			Transform[] children = this.transform.GetComponentsInChildren<Transform>();

			foreach (Transform child in children) {
				if (child.parent == this.transform && child != parent) {
					GameObject go = InstantiateCopy (child.gameObject, false);
					go.transform.SetParent (parent);
				}
			}
		}

        /// <summary>
        /// Generate the new uvs of the mesh in texture atlas
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="originalMaterials"></param>
        /// <param name="objectName"></param>
        /// <param name="combinedIndex"></param>
        private void GenerateUVs(Mesh mesh, Material[] originalMaterials, string objectName, int combinedIndex)
        {
			int[] textureIndexes = new int[originalMaterials.Length];

	        for (int j = 0; j < originalMaterials.Length; j++)
            {
				Material mat = originalMaterials [j];
				textureIndexes [j] = combinedResult.FindCorrespondingMaterialIndex (mat, combinedIndex);
	        }
			
			if (!meshCombiner.GenerateUV (mesh, textureIndexes, combinedResult.combinedMaterials[combinedIndex].scaleFactors.ToArray(), objectName, combinedIndex))
            {
				UnCombine();
				return;
			}
		}

        /// <summary>
        /// Reactivate original GameObjects
        /// </summary>
        /// <param name="meshes"></param>
        /// <param name="skinnedMeshes"></param>
        private void EnableRenderers(List<MeshRenderer> meshes, List<SkinnedMeshRenderer> skinnedMeshes)
		{
			foreach (MeshRenderer go in meshes)
			{
				if (go != null) {
					go.gameObject.SetActive (true);
				}
			}
	        foreach (SkinnedMeshRenderer go in skinnedMeshes)
	        {
	            if (go != null)
	            {
	                go.gameObject.SetActive(true);
	            }
	        }
	    }

        /// <summary>
        /// Deactivate original GameObjects
        /// </summary>
        /// <param name="meshes"></param>
        /// <param name="skinnedMeshes"></param>
        private void DisableRenderers(List<MeshRenderer> meshes, List<SkinnedMeshRenderer> skinnedMeshes)
		{
	        foreach (MeshRenderer go in meshes)
	        {
				if (go != null && go.gameObject != targetGameObject)
	            {
	                go.gameObject.SetActive(false);
	            }
	        }
	        foreach (SkinnedMeshRenderer go in skinnedMeshes)
	        {
				if (go != null && go.gameObject != targetGameObject)
	            {
	                go.gameObject.SetActive(false);
	            }
	        }
	    }

        /// <summary>
        /// Generate the new transformed gameobjects and apply new materials to them, when no combining meshes
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="originalMeshRenderer"></param>
        /// <returns></returns>
        private List<GameObject> GenerateTransformedGameObjects(Transform parent, List<MeshRenderer> originalMeshRenderer)
		{
			List<GameObject> copyList = new List<GameObject> ();

			for(int i=0; i<originalMeshRenderer.Count; i++) {
				// Copy the new mesh to the created GameObject copy
				Mesh copyOfMesh = meshCombiner.copyMesh(originalMeshRenderer[i].GetComponent<MeshFilter> ().sharedMesh);

				// Add the copy mesh instanceId with its original sharedMesh and sharedMaterial instanceId
				if (originalMeshRenderer [i].GetComponent<Renderer> ().sharedMaterial != null) {
					uniqueCombinedMeshId.Add (copyOfMesh.GetInstanceID (), originalMeshRenderer [i].GetComponent<MeshFilter> ().sharedMesh.GetInstanceID ().ToString () + originalMeshRenderer [i].GetComponent<Renderer> ().sharedMaterial.GetInstanceID ().ToString () + copyOfMesh.name);
				} else {
					uniqueCombinedMeshId.Add (copyOfMesh.GetInstanceID (), originalMeshRenderer [i].GetComponent<MeshFilter> ().sharedMesh.GetInstanceID ().ToString ()  + copyOfMesh.name);
				}
				copyMeshId[originalMeshRenderer[i].GetComponent<MeshFilter> ().sharedMesh.GetInstanceID()] = uniqueCombinedMeshId[copyOfMesh.GetInstanceID ()];
				originalMeshRenderer[i].GetComponent<MeshFilter> ().sharedMesh = copyOfMesh;

	#if UNITY_EDITOR
	            // Unwrap UV2 for lightmap
	            Unwrapping.GenerateSecondaryUVSet(originalMeshRenderer[i].GetComponent<MeshFilter>().sharedMesh);
	#endif

	            // Assign new materials
	            if (combineMaterials) {
                    Material[] originalMaterials = originalMeshRenderer[i].GetComponent<Renderer>().sharedMaterials;
                    Material[] newMats = new Material[originalMaterials.Length];
					for (int k = 0; k < newMats.Length; k++)
                    {
						newMats [k] = combinedResult.GetCombinedMaterial(originalMaterials[k]);
					}
					originalMeshRenderer[i].GetComponent<Renderer>().sharedMaterials = newMats;
				}
				else {
                    // If materials are not combined
					/*Material[] mat = objects [i].GetComponent<Renderer> ().sharedMaterials;
					Material[] newMats = new Material[mat.Length];
					for (int a = 0; a < mat.Length; a++) {
						newMats [a] = texturePackers[0].getTransformedMaterialValue (objects [i].GetComponent<Renderer> ().sharedMaterials [a].name);
						// Find corresponding material
						combinedTextureIndex.Add (combinedResult.FindCorrespondingMaterialIndex(mat[a], 0));
					}
					objects[i].GetComponent<Renderer> ().sharedMaterials = newMats;*/
				}

				copyList.Add(originalMeshRenderer[i].gameObject);
			}

			return copyList;
		}

        /// <summary>
        /// Generate the new transformed gameobjects and apply new materials to them, when no combining meshes
        /// For Skinned Mesh renderers, when no combining meshes
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="originalSkinnedMeshRenderer"></param>
        /// <returns></returns>
        private List<GameObject> GenerateTransformedGameObjects(Transform parent, List<SkinnedMeshRenderer> originalSkinnedMeshRenderer) 
		{
			List<GameObject> copyList = new List<GameObject> ();

			for(int i=0; i<originalSkinnedMeshRenderer.Count; i++) {
				// Copy the new mesh to the created GameObject copy
				Mesh copyOfMesh = meshCombiner.copyMesh(originalSkinnedMeshRenderer[i].GetComponent<SkinnedMeshRenderer> ().sharedMesh);

				// Add the copy mesh instanceId with its original sharedMesh and sharedMaterial instanceId
				if (originalSkinnedMeshRenderer [i].GetComponent<Renderer> ().sharedMaterial != null) {
					uniqueCombinedMeshId.Add (copyOfMesh.GetInstanceID (), originalSkinnedMeshRenderer [i].GetComponent<SkinnedMeshRenderer> ().sharedMesh.GetInstanceID ().ToString () + originalSkinnedMeshRenderer [i].GetComponent<Renderer> ().sharedMaterial.GetInstanceID ().ToString () + copyOfMesh.name);
				} else {
					uniqueCombinedMeshId.Add (copyOfMesh.GetInstanceID (), originalSkinnedMeshRenderer [i].GetComponent<SkinnedMeshRenderer> ().sharedMesh.GetInstanceID ().ToString () + copyOfMesh.name);
				}
				copyMeshId[originalSkinnedMeshRenderer[i].GetComponent<SkinnedMeshRenderer> ().sharedMesh.GetInstanceID()] = uniqueCombinedMeshId[copyOfMesh.GetInstanceID ()];
				originalSkinnedMeshRenderer[i].GetComponent<SkinnedMeshRenderer>  ().sharedMesh = copyOfMesh;

	#if UNITY_EDITOR
	            // Unwrap UV2 for lightmap
	            //Unwrapping.GenerateSecondaryUVSet(skinnedObjects[i].GetComponent<SkinnedMeshRenderer>().sharedMesh);
	#endif

	            // Assign new materials
	            if (combineMaterials) {
                    Material[] originalMaterials = originalSkinnedMeshRenderer[i].GetComponent<Renderer>().sharedMaterials;
                    Material[] newMats = new Material[originalMaterials.Length];
					for (int k = 0; k < newMats.Length; k++)
                    {
                        newMats[k] = combinedResult.GetCombinedMaterial(originalMaterials[k]);
                    }
					originalSkinnedMeshRenderer[i].GetComponent<SkinnedMeshRenderer>().sharedMaterials = newMats;					
				}
				else {
                    // If materials are not combined
                    /*Material[] mat = skinnedObjects [i].sharedMaterials;
					Material[] newMats = new Material[mat.Length];
					for (int a = 0; a < mat.Length; a++) {
						newMats [a] = texturePackers[0].getTransformedMaterialValue (skinnedObjects [i].sharedMaterials [a].name);
						// Find corresponding material
						combinedTextureIndex.Add (combinedResult.FindCorrespondingMaterialIndex(mat[a], 0));
					}
					skinnedObjects[i].GetComponent<SkinnedMeshRenderer> ().sharedMaterials = newMats;*/
                }

                copyList.Add(originalSkinnedMeshRenderer[i].gameObject);
			}
			
			return copyList;
		}

		// Instantiate a copy of the GameObject, keeping it's transform values identical
		private GameObject InstantiateCopy(GameObject original, bool deleteChidren = true) {
			GameObject copy = Instantiate(original) as GameObject;
			copy.transform.parent = original.transform.parent;
			copy.transform.localPosition = original.transform.localPosition;
			copy.transform.localRotation = original.transform.localRotation;
			copy.transform.localScale = original.transform.localScale;
			copy.name = original.name;

			if (deleteChidren) {
				// Delete all children
				foreach (Transform child in copy.transform) {
					DestroyImmediate (child.gameObject);
				}
			}

			return copy;
		}

		// Find all enabled mesh colliders
		private List<MeshCollider> FindEnabledMeshColliders(Transform parent) {
			MeshCollider[] colliders;
			colliders = parent.GetComponentsInChildren<MeshCollider> ();

			List<MeshCollider> meshColliders = new List<MeshCollider> ();
			foreach (MeshCollider collider in colliders) {
				if (collider.sharedMesh != null) {
					meshColliders.Add (collider);
				}
			}

			return meshColliders;
		}

		// Find and store all enabled meshes
		private List<MeshRenderer> FindEnabledMeshes(Transform parent)
		{
			MeshFilter[] filters;
			filters = parent.GetComponentsInChildren<MeshFilter>();

			List<MeshRenderer> meshRendererList = new List<MeshRenderer>();

			foreach (MeshFilter filter in filters) {
				if (filter.sharedMesh != null) {
					MeshRenderer renderer = filter.GetComponent<MeshRenderer> ();
					if (renderer != null && renderer.enabled && renderer.sharedMaterials.Length > 0) {
						meshRendererList.Add (renderer);
					}
				}
			}

			return meshRendererList;
		}

		// Find and store all enabled skin meshes
		private List<SkinnedMeshRenderer> FindEnabledSkinnedMeshes(Transform parent)
		{
			// Skinned meshes
			SkinnedMeshRenderer[] skinnedMeshes = parent.GetComponentsInChildren<SkinnedMeshRenderer>();

			List<SkinnedMeshRenderer> skinnedMeshRendererList = new List<SkinnedMeshRenderer>();

			foreach (SkinnedMeshRenderer skin in skinnedMeshes) {
				if(skin.sharedMesh != null) {
					if(skin.enabled && skin.sharedMaterials.Length > 0) {
						skinnedMeshRendererList.Add(skin);
					}
				}
			}

			return skinnedMeshRendererList;
		}

        /// <summary>
        /// Find and return all enabled materials in given meshes and skinnedMeshes
        /// </summary>
        /// <param name="meshes"></param>
        /// <param name="skinnedMeshes"></param>
        /// <returns></returns>
        private List<MaterialToCombine> FindEnabledMaterials(List<MeshRenderer> meshes, List<SkinnedMeshRenderer> skinnedMeshes)
		{
            // List of materials linked with their instanceID
            Dictionary<int, MaterialToCombine> matList = new Dictionary<int, MaterialToCombine> ();

			// Meshes renderer
			foreach (MeshRenderer mesh in meshes) {
				Rect uvBound = getUVBounds(mesh.GetComponent<MeshFilter> ().sharedMesh.uv);

				foreach (Material material in mesh.sharedMaterials) {
					if (material != null) {
						int instanceId = material.GetInstanceID ();

						if (!matList.ContainsKey (instanceId)) {
                            // Material has not been listed yet, add it to the list
                            MaterialToCombine matToCombine = new MaterialToCombine();
                            matToCombine.material = material;
                            matToCombine.uvBounds = uvBound;
                            matList.Add (instanceId, matToCombine);
						} else {
                            // This material has already been found, check if the uv bounds is bigger
                            Rect maxRect = getMaxRect(matList[instanceId].uvBounds, uvBound);
                            MaterialToCombine matToCombine = matList[instanceId];
                            matToCombine.uvBounds = maxRect;
                            matList[instanceId] = matToCombine;
                        }
                    } else
                    {
                        // The material is null
                    }
				}
			}

			// SkinnedMeshes renderer
			foreach (SkinnedMeshRenderer skinnedMesh in skinnedMeshes) {
				Rect uvBound = getUVBounds(skinnedMesh.sharedMesh.uv);

				foreach (Material material in skinnedMesh.sharedMaterials) {
					if (material != null) {
						int instanceId = material.GetInstanceID ();
						
						if (!matList.ContainsKey (instanceId)) {
                            // Material has not been listed yet, add it to the list
                            MaterialToCombine matToCombine = new MaterialToCombine();
                            matToCombine.material = material;
                            matToCombine.uvBounds = uvBound;
                            matList.Add(instanceId, matToCombine);
                        } else {
                            // This material has already been found, check if the uv bounds is bigger
                            Rect maxRect = getMaxRect(matList[instanceId].uvBounds, uvBound);
                            MaterialToCombine matToCombine = matList[instanceId];
                            matToCombine.uvBounds = maxRect;
                            matList[instanceId] = matToCombine;
                        }
					} else
                    {
                        // The material is null
                    }
                }
			}

            return new List<MaterialToCombine>(matList.Values);
		}

		// Return the bound of the uv list (min, max for x and y axis)
		private Rect getUVBounds(Vector2[] uvs) {
			Rect uvBound = new Rect (0, 0, 1, 1);
			for (int i = 0; i < uvs.Length; i++) {
				if (uvs [i].x < 0 && uvs[i].x < uvBound.xMin) {
					uvBound.xMin = uvs [i].x;
				}
				if (uvs [i].x > 1 && uvs [i].x > uvBound.xMax) {
					uvBound.xMax = uvs [i].x;
				}
				if (uvs [i].y < 0 && uvs[i].y < uvBound.yMin) {
					uvBound.yMin = uvs [i].y;
				}
				if (uvs [i].y > 1 && uvs [i].y > uvBound.yMax) {
					uvBound.yMax = uvs [i].y;
				}
			}
			return uvBound;
		}

		// Return the maximum rect based on the two rect parameters
		private Rect getMaxRect(Rect uv1, Rect uv2) {
			Rect newRect = new Rect();
			newRect.xMin = Math.Min (uv1.xMin, uv2.xMin);
			newRect.yMin = Math.Min (uv1.yMin, uv2.yMin);
			newRect.xMax= Math.Max (uv1.xMax, uv2.xMax);
			newRect.yMax = Math.Max (uv1.yMax, uv2.yMax);
			return newRect;
		}

        /// <summary>
        /// Reverse combine process, destroy all created objects and reactivate original mesh renderers
        /// </summary>
        public void UnCombine()
		{
#if UNITY_EDITOR
			// Hide progressbar
			EditorUtility.ClearProgressBar();
#endif

			// Reactivate original renderers
			EnableRenderers (meshList, skinnedMeshList);

            if (targetParentForCombinedGameObjects == targetGameObject && combinedResult != null)
            {
                for (int i = 0; i < combinedResult.GetCombinedIndexCount(); i++)
                {
                    if (combinedResult.combinedGameObjectFromMeshList.Count > i)
                    {
                        foreach (GameObject go in combinedResult.combinedGameObjectFromMeshList[i])
                        {
                            DestroyImmediate(go);
                        }
                        foreach (GameObject go in combinedResult.combinedGameObjectFromSkinnedMeshList[i])
                        {
                            DestroyImmediate(go);
                        }
                    }
                }
			} else
			{
				DestroyImmediate (targetParentForCombinedGameObjects);
			}

            // Clear the packed textures
            texturePackers.Clear ();            
            materialsToCombine.Clear ();
            multiMaterialsList.Clear ();
            meshCombiner.Clear ();
			meshList.Clear ();
			skinnedMeshList.Clear ();
			uniqueCombinedMeshId.Clear ();
			copyMeshId.Clear ();
			toSavePrefabList.Clear ();
			toSaveObjectList.Clear ();
			toSaveMeshList.Clear ();
			toSaveSkinnedObjectList.Clear ();

            if (combinedResult != null)
            {
                combinedResult.Clear();
            }
	        combiningState = CombineStatesList.Uncombined;

			Debug.Log ("[Super Combiner] Successfully uncombined game objects.");
		}

        /// <summary>
        /// Get the first level children list of the parents
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private List<Transform> GetFirstLevelChildren(Transform parent) {
			List<Transform> children = new List<Transform>();
			for(int i=0; i<parent.transform.childCount; i++) {
				children.Add(parent.transform.GetChild(i));
			}
			return children;
		}

        /// <summary>
        /// Save combined objects
        /// </summary>
        public void Save()
        {
#if UNITY_EDITOR
            // Combine process is finished
            EditorUtility.ClearProgressBar();

            if (folderDestination == "")
            {
                // Default export folder destination
                folderDestination = "Assets/SuperCombiner/Combined";
            }

            // Check if destination folder exists
            if (!Directory.Exists(folderDestination))
            {
                Directory.CreateDirectory(folderDestination);
            }

            // Generate new instances (copy from modifiedObjectList) to be saved, so that objects in modifiedObjectList won't be affected by user's modification/deletion
            toSavePrefabList.Clear();
            toSaveObjectList.Clear();
            toSaveMeshList.Clear();
            toSaveSkinnedObjectList.Clear();
            Material[] savedMaterial = new Material[combinedResult.GetCombinedIndexCount()];

            for (int i = 0; i < combinedResult.GetCombinedIndexCount(); i++)
            {
                if (combinedResult.combinedMaterials[i].material != null)
                {
                    texturePackers[i].GenerateCopyedMaterialToSave();

                    if (texturePackers[i].GetCombinedMaterialToSave() == null)
                    {
                        Debug.LogError("[Super Combiner] Instance of combined material has been lost, try to combine again before saving.");
                    }
                    else
                    {
                        // We need to know if the combined material has already been saved
                        savedMaterial[i] = AssetDatabase.LoadAssetAtPath<Material>(folderDestination + "/Materials/" + texturePackers[i].copyedMaterials.name + ".mat");
                    }
                }
            }

            if (combiningState == CombineStatesList.Combined)
            {
                // List of all different meshes found on every game objects to save, with no duplication
                Dictionary<string, Mesh> meshMaterialId = new Dictionary<string, Mesh>();

                bool outputSkinnedMesh = false;
                List<Transform> children = new List<Transform>();
                if (combineMeshes && (LunarCatsStudio.SuperCombiner.MeshOutput)meshOutput == LunarCatsStudio.SuperCombiner.MeshOutput.SkinnedMesh)
                {
                    // If output is skinnedMesh, we save a copy of modifiedParent as prefab
                    GameObject copy = InstantiateCopy(targetParentForCombinedGameObjects, false);
                    toSavePrefabList.Add(copy);
                    outputSkinnedMesh = true;
                    children = GetFirstLevelChildren(copy.transform);
                }
                else
                {
                    children = GetFirstLevelChildren(targetParentForCombinedGameObjects.transform);
                }

                // Generate copy of game objects to be saved
                foreach (Transform child in children)
                {
                    GameObject copy;
                    if (outputSkinnedMesh)
                    {
                        copy = child.gameObject;
                    }
                    else
                    {
                        copy = InstantiateCopy(child.gameObject, false);
                    }

                    List<MeshRenderer> meshes = FindEnabledMeshes(copy.transform);
                    List<SkinnedMeshRenderer> skinnedMeshes = FindEnabledSkinnedMeshes(copy.transform);
                    List<MeshCollider> meshColliders = FindEnabledMeshColliders(copy.transform);

                    // Create a copy of mesh
                    foreach (MeshRenderer mesh in meshes)
                    {
                        int instanceId = mesh.GetComponent<MeshFilter>().sharedMesh.GetInstanceID();

                        if (uniqueCombinedMeshId.ContainsKey(instanceId))
                        {
                            if (meshMaterialId.ContainsKey(uniqueCombinedMeshId[instanceId]))
                            {
                                // This mesh is shared with other game objects, so we reuse the first instance to avoid duplication
                                mesh.GetComponent<MeshFilter>().sharedMesh = meshMaterialId[uniqueCombinedMeshId[instanceId]];
                            }
                            else
                            {
                                Mesh copyOfMesh = meshCombiner.copyMesh(mesh.GetComponent<MeshFilter>().sharedMesh);
                                mesh.GetComponent<MeshFilter>().sharedMesh = copyOfMesh;
                                meshMaterialId.Add(uniqueCombinedMeshId[instanceId], copyOfMesh);
                                toSaveMeshList.Add(copyOfMesh);
                            }

                            // Apply a copy of the material to save
                            Material[] newMat = new Material[mesh.sharedMaterials.Length];
                            for (int j = 0; j < mesh.sharedMaterials.Length; j++)
                            {
                                if (combineMaterials)
                                {
                                    // Get the index of this combined material
                                    int index = 0;
                                    for (int k = 0; k < combinedResult.combinedMaterials.Count; k++)
                                    {
                                        if (mesh.sharedMaterials[j] == combinedResult.combinedMaterials[k].material)
                                        {
                                            index = k;
                                        }
                                    }
                                    if (savedMaterial[index] != null)
                                    {
                                        // If the combined material already exists, assign it
                                        newMat[j] = savedMaterial[index];
                                    }
                                    else
                                    {
                                        newMat[j] = texturePackers[index].GetCombinedMaterialToSave();
                                    }
                                }
                                else
                                {
                                    //newMat[j] = texturePackers[i].GetTransformedMaterialToSave(mesh.sharedMaterials[j].name);
                                }
                            }

                            mesh.sharedMaterials = newMat;
                            toSaveObjectList.Add(mesh);
                        }
                        else
                        {
                            Debug.LogError("[Super Combiner] Could not find " + mesh.name + " in uniqueCombinedMeshId, data may has been lost, try to combine again before saving.");
                        }

                    }
                    foreach (SkinnedMeshRenderer skinnedmesh in skinnedMeshes)
                    {
                        int instanceId = skinnedmesh.sharedMesh.GetInstanceID();

                        if (uniqueCombinedMeshId.ContainsKey(instanceId))
                        {
                            if (meshMaterialId.ContainsKey(uniqueCombinedMeshId[instanceId]))
                            {
                                // This mesh is shared with other game objects, so we reuse the first instance to avoid duplication
                                skinnedmesh.sharedMesh = meshMaterialId[uniqueCombinedMeshId[instanceId]];
                            }
                            else
                            {
                                Mesh copyOfMesh = meshCombiner.copyMesh(skinnedmesh.sharedMesh);
                                skinnedmesh.sharedMesh = copyOfMesh;
                                meshMaterialId.Add(uniqueCombinedMeshId[instanceId], copyOfMesh);
                                toSaveMeshList.Add(copyOfMesh);
                            }

                            // Apply a copy of the material to save
                            Material[] newMat = new Material[skinnedmesh.sharedMaterials.Length];
                            for (int j = 0; j < skinnedmesh.sharedMaterials.Length; j++)
                            {
                                if (combineMaterials)
                                {
                                    // Get the index of this combined material
                                    int index = 0;
                                    for (int k = 0; k < combinedResult.combinedMaterials.Count; k++)
                                    {
                                        if (skinnedmesh.sharedMaterials[j] == combinedResult.combinedMaterials[k].material)
                                        {
                                            index = k;
                                        }
                                    }
                                    if (savedMaterial[index] != null)
                                    {
                                        // If the combined material already exists, assign it
                                        newMat[j] = savedMaterial[index];
                                    }
                                    else
                                    {
                                        newMat[j] = texturePackers[index].GetCombinedMaterialToSave();
                                    }
                                }
                                else
                                {
                                    //newMat[j] = texturePackers[i].GetTransformedMaterialToSave(skinnedmesh.sharedMaterials[j].name);
                                }
                            }
                            skinnedmesh.sharedMaterials = newMat;
                            toSaveSkinnedObjectList.Add(skinnedmesh);
                        }
                        else
                        {
                            Debug.LogError("[Super Combiner] Could not find " + skinnedmesh.name + " in uniqueCombinedMeshId, data may has been lost, try to combine again before saving.");
                        }

                    }

                    // Assign to mesh colliders the mesh that will be saved
                    foreach (MeshCollider collider in meshColliders)
                    {
                        int instanceId = collider.sharedMesh.GetInstanceID();

                        string id = null;
                        copyMeshId.TryGetValue(instanceId, out id);
                        if (id != null)
                        {
                            if (meshMaterialId.ContainsKey(id))
                            {
                                collider.sharedMesh = meshMaterialId[id];
                            }
                        }
                        else
                        {
                            // This means the collider has a mesh that is not present in the combine list
                            // In this case, keep the meshCollider component intact
                        }
                    }

                    // Add this GameObject to the list of prefab to save
                    if (!outputSkinnedMesh)
                    {
                        toSavePrefabList.Add(copy);
                    }
                }
            }

            // Saving process
            if (saveTextures)
            {
                for (int i = 0; i < combinedResult.GetCombinedIndexCount(); i++)
                {
                    if (combinedResult.combinedMaterials[i].material != null)
                    {
                        Saver.SaveTextures(i, folderDestination, sessionName, texturePackers[i]);
                    }
                }
            }
            if (saveMaterials)
            {
                for (int i = 0; i < combinedResult.GetCombinedIndexCount(); i++)
                {
                    if (combinedResult.combinedMaterials[i].material != null)
                    {
                        Saver.SaveMaterial(i, folderDestination, sessionName, texturePackers[i]);
                    }
                }
            }
            if (savePrefabs)
            {
                Saver.SavePrefabs(toSavePrefabList, toSaveMeshList, folderDestination, sessionName);
                for (int n = 0; n < toSavePrefabList.Count; n++)
                {
                    DestroyImmediate(toSavePrefabList[n]);
                }
                toSavePrefabList.Clear();
                toSaveMeshList.Clear();
            }
            if (saveMeshObj)
            {
                for (int i = 0; i < combinedResult.GetCombinedIndexCount(); i++)
                {
                    if (combinedResult.combinedMaterials[i].material != null)
                    {
                        Saver.SaveMeshesObj(combinedResult.combinedGameObjectFromMeshList[i], combinedResult.combinedGameObjectFromSkinnedMeshList[i], folderDestination);
                        for (int n = 0; n < toSaveObjectList.Count; n++)
                        {
                            DestroyImmediate(toSaveObjectList[n]);
                        }
                        for (int n = 0; n < toSaveSkinnedObjectList.Count; n++)
                        {
                            DestroyImmediate(toSaveSkinnedObjectList[n]);
                        }
                        toSaveObjectList.Clear();
                        toSaveSkinnedObjectList.Clear();
                    }
                }
            }
            if (saveMeshFbx)
            {
                //SaveMeshesFbx();
            }
            
            // Saves the combined result asset
            Saver.SaveCombinedResults(combinedResult, folderDestination, sessionName);
            
            EditorUtility.DisplayDialog("Super Combiner", "Objects saved in '" + folderDestination + "/' \n\nThanks for using Super Combiner.", "Ok");

            // Hide progressbar
            EditorUtility.ClearProgressBar();
#endif
        }
    }
}