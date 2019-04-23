using System.Collections;
using UnityEngine;
namespace CustomUnits
{
    [System.Serializable]
    public struct SavebleVector3
    {
        public float x, y, z;
        public SavebleVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }
        public static implicit operator Vector3(SavebleVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }
        public static implicit operator SavebleVector3(Vector3 rValue)
        {
            return new SavebleVector3(rValue.x, rValue.y, rValue.z);
        }
    }

    [System.Serializable]
    public struct SavebleQuaternion
    {
        public float x, y, z, w;
        public SavebleQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }
        public static implicit operator Quaternion(SavebleQuaternion rValue)
        {
            return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
        public static implicit operator SavebleQuaternion(Quaternion rValue)
        {
            return new SavebleQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
        }
    }
}
