using UnityEngine;
using System;

[Serializable]
public class PoolConfig
{
    public string name;

    public GameObject prefab;

    public int defaultSize;

    public int maxSize;
}
