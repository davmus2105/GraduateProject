using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceExtension
{
    public class ResourceFolderPath
    {
        public static readonly Dictionary<ResourceType, string> ResourceTypePath = new Dictionary<ResourceType, string>
        {
            [ResourceType.Prefab] = "Prefabs",            
            [ResourceType.AudioClip] = "Audio",
            [ResourceType.Sprite] = "Sprites",
            [ResourceType.Texture] = "Textures",
        };
    }
    public enum ResourceType
    {
        Texture,
        Sprite,
        AudioClip,
        Prefab,
    }
}
