using System;
using UnityEngine;

[Serializable]
public struct BulletPoolConfig
{
	public string name;

    public Bullet prefab;

    public int defaultSize;

    public int maxSize;
}