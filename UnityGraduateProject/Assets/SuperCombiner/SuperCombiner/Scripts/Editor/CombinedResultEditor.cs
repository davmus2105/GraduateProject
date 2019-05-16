using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LunarCatsStudio.SuperCombiner 
{
	/// <summary>
	/// Combined result editor.
	/// </summary>
	[CustomEditor(typeof(CombinedResult))]
	public class CombinedResultEditor : Editor {

		// Reference to the SuperCombiner script
		private CombinedResult _combinedResult;

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		private void OnEnable()
		{
			_combinedResult = (CombinedResult)target;
		}

		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
            DisplayStats();
            DisplayCombinedMaterial();
            DisplayCombinedMeshResults();
		}

        /// <summary>
        /// Display the combined mesh result section
        /// </summary>
        private void DisplayCombinedMeshResults()
        {
            GUILayout.Label("Combined meshes", EditorStyles.whiteBoldLabel);
            if(_combinedResult.meshResults.Count > 0)
            {
                _combinedResult.showCombinedMeshes = EditorGUILayout.Foldout(_combinedResult.showCombinedMeshes, "Combined meshes (" + _combinedResult.meshResults.Count + ")");
                if (_combinedResult.showCombinedMeshes)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    for (int i = 0; i < _combinedResult.meshResults.Count; i++)
                    {
                        _combinedResult.meshResults[i].showMeshCombined = EditorGUILayout.Foldout(_combinedResult.meshResults[i].showMeshCombined, "Combined mesh " + i + " (" + _combinedResult.meshResults[i].names.Count + ")");
                        if(_combinedResult.meshResults[i].showMeshCombined)
                        {
                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                            for (int j = 0; j < _combinedResult.meshResults[i].names.Count; j++)
                            {
                                _combinedResult.meshResults[i].indexes[j].showCombinedInstanceIndex = EditorGUILayout.Foldout(_combinedResult.meshResults[i].indexes[j].showCombinedInstanceIndex, "source object " + j + ": " + _combinedResult.meshResults[i].names[j] + "");
                                if(_combinedResult.meshResults[i].indexes[j].showCombinedInstanceIndex)
                                {
                                    EditorGUILayout.TextField("name", _combinedResult.meshResults[i].names[j]);
                                    EditorGUILayout.IntField("instance Id", _combinedResult.meshResults[i].instanceIds[j]);
                                    EditorGUILayout.IntField("first vertex index", _combinedResult.meshResults[i].indexes[j].firstVertexIndex);
                                    EditorGUILayout.IntField("vertex count", _combinedResult.meshResults[i].indexes[j].vertexCount);
                                    EditorGUILayout.IntField("first triangle index", _combinedResult.meshResults[i].indexes[j].firstTriangleIndex);
                                    EditorGUILayout.IntField("triangle count", _combinedResult.meshResults[i].indexes[j].triangleCount);
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            } else
            {
                GUILayout.Label("No mesh were combined", EditorStyles.wordWrappedLabel);
            }
        }

        /// <summary>
        /// Display the general stat section
        /// </summary>
        private void DisplayStats()
        {
            // Display settings sections
            GUILayout.Label("General information", EditorStyles.whiteBoldLabel);
            EditorGUILayout.LabelField(_combinedResult.materialCombinedCount + " materials were combined");
            EditorGUILayout.LabelField(_combinedResult.meshesCombinedCount + " meshes were combined");
            EditorGUILayout.LabelField(_combinedResult.skinnedMeshesCombinedCount + " skinnedMeshes were combined");
            EditorGUILayout.LabelField(_combinedResult.subMeshCount + " subMeshes were found");
            EditorGUILayout.LabelField(_combinedResult.totalVertexCount + " vertices where combined");
            //EditorGUILayout.LabelField("All combined in " + _combinedResult.duration);

            EditorGUILayout.Space();
        }

        /// <summary>
        /// Display the combined material section
        /// </summary>
        private void DisplayCombinedMaterial()
        {
            GUILayout.Label("Combined material(s)", EditorStyles.whiteBoldLabel);
            _combinedResult.showCombinedMaterials = EditorGUILayout.Foldout(_combinedResult.showCombinedMaterials, "Combined materials (" + _combinedResult.combinedMaterialCount + ")");
            if (_combinedResult.showCombinedMaterials)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                for (int i = 0; i < _combinedResult.combinedMaterials.Count; i++)
                {
                    if (_combinedResult.combinedMaterials[i].material != null)
                    {
                        _combinedResult.combinedMaterials[i].showCombinedMaterial = EditorGUILayout.Foldout(_combinedResult.combinedMaterials[i].showCombinedMaterial, "Combined material " + _combinedResult.combinedMaterials[i].displayedIndex);
                        if (_combinedResult.combinedMaterials[i].showCombinedMaterial)
                        {
                            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                            // Show combined material
                            EditorGUILayout.ObjectField("Material " + _combinedResult.combinedMaterials[i].displayedIndex, _combinedResult.combinedMaterials[i].material, typeof(Material), true);
                            // Show UVs
                            _combinedResult.combinedMaterials[i].showUVs = EditorGUILayout.Foldout(_combinedResult.combinedMaterials[i].showUVs, new GUIContent("UVs (" + _combinedResult.combinedMaterials[i].uvs.Length + ")", "Each UV rectangle below correspond to a specific location in the altas texture"));
                            if (_combinedResult.combinedMaterials[i].showUVs)
                            {
                                for (int j = 0; j < _combinedResult.combinedMaterials[i].uvs.Length; j++)
                                {
                                    EditorGUILayout.RectField("uv[" + j + "]", _combinedResult.combinedMaterials[i].uvs[j]);
                                }
                            }
                            // Show mesh uv bounds
                            _combinedResult.combinedMaterials[i].showMeshUVBounds = EditorGUILayout.Foldout(_combinedResult.combinedMaterials[i].showMeshUVBounds, new GUIContent("Mesh uv bounds (" + _combinedResult.combinedMaterials[i].meshUVBounds.Count + ")", "Each rectangle below correspond to the UV bound of the original mesh. This helps to see if meshes has UVs out of [0, 1] bounds."));
                            if (_combinedResult.combinedMaterials[i].showMeshUVBounds)
                            {
                                for (int j = 0; j < _combinedResult.combinedMaterials[i].meshUVBounds.Count; j++)
                                {
                                    EditorGUILayout.RectField("bound[" + j + "]", _combinedResult.combinedMaterials[i].meshUVBounds[j]);
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}
