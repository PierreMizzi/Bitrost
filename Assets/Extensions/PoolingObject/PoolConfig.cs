using UnityEngine;
using System;

namespace PierreMizzi.Useful.PoolingObjects
{
    [Serializable]
    public class PoolConfig
    {
        public string name;

        public GameObject prefab;

        public int defaultSize;

        public int maxSize;
    }
}
