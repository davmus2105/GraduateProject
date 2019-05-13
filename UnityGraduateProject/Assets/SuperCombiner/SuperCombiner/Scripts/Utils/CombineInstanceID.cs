﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LunarCatsStudio.SuperCombiner
{
    /// <summary>
    /// Combine Instance ID
    /// </summary>
    public class CombineInstanceID
    {
        // List of combine instances
        public List<CombineInstance> combineInstances = new List<CombineInstance>();
        // List of instanceID of every gameObject to combine
        public List<int> instancesID = new List<int>();
        // List of name of every gameObject to combine
        public List<string> names = new List<string>();

        /// <summary>
        /// Add a new Combine Instance
        /// </summary>
        /// <param name="subMeshIndex"></param>
        /// <param name="mesh"></param>
        /// <param name="matrix"></param>
        /// <param name="instanceID"></param>
        /// <param name="name"></param>
        public void AddCombineInstance(int subMeshIndex, Mesh mesh, Matrix4x4 matrix, int instanceID, string name)
        {
            // Add the combine instance
            CombineInstance combineInstance = new CombineInstance();
            combineInstance.subMeshIndex = subMeshIndex;
            combineInstance.mesh = mesh;
            combineInstance.transform = matrix;
            combineInstances.Add(combineInstance);
            // Add the instanceID
            instancesID.Add(instanceID);
            // Add the name
            names.Add(name);
        }

        public void AddRange(CombineInstanceID instances)
        {
            combineInstances.AddRange(instances.combineInstances);
            instancesID.AddRange(instances.instancesID);
            names.AddRange(instances.names);
        }

        /// <summary>
        /// Clear data
        /// </summary>
        public void Clear()
        {
            combineInstances.Clear();
            instancesID.Clear();
            names.Clear();
        }

        /// <summary>
        /// Return the number of combineInstances instances
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return combineInstances.Count;
        }
    }
}
