using UnityEngine;

public delegate void CreatePool(PoolConfig config);
public delegate GameObject GetFromPool(GameObject gameObject);
public delegate void ReleaseFromPool(GameObject gameObject);

[CreateAssetMenu(fileName = "PoolingChannel", menuName = "Bitrost/PoolingChannel", order = 0)]
public class PoolingChannel : ScriptableObject
{
    public CreatePool onCreatePool = null;

    public GetFromPool onGetFromPool = null;

    public ReleaseFromPool onReleaseFromPool = null;

    private void OnEnable()
    {
        onCreatePool = (PoolConfig config) => { };
        onGetFromPool = (GameObject gameObject) =>
        {
            return null;
        };
        onReleaseFromPool = (GameObject gameObject) => { };
    }
}
