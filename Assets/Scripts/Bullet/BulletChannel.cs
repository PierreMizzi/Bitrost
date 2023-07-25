using UnityEngine;

[CreateAssetMenu(fileName = "BulletChannel", menuName = "Bitrost/Bullet/BulletChannel", order = 0)]
public class BulletChannel : ScriptableObject
{
    public InstantiateBulletDelegate onInstantiateBullet = null;

    public ReleaseBulletDelegate onReleaseBullet = null;

    private void OnEnable()
    {
        onInstantiateBullet = (
            IBulletLauncher launcher,
            BulletType type,
            Vector3 position,
            Vector3 orientation
        ) => { };
        onReleaseBullet = (Bullet bullet) => { };
    }
}
