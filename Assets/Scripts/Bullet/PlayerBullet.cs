using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;
using System.Collections;

public class PlayerBullet : Bullet
{
    private BulletTrack m_track = null;

    private Tween m_tween = null;

    public void Initialize(BulletTrack track)
    {
        m_track = track;

        float time = m_track.spline.GetLength() / m_track.bulletSpeed;

        m_tween = DOVirtual
            .Float(
                0f,
                1f,
                time,
                (float value) =>
                {
                    transform.position = m_track.spline.EvaluatePosition(value);
                    transform.up = m_track.spline.EvaluateTangent(value);
                }
            )
            .SetEase(Ease.Linear)
            .OnComplete(FireFromTurret);
    }

    public void FireFromTurret()
    {
        transform.up = ((TurretNode)m_track.lastNode).aimDirection;
        StartCoroutine("FreeMouvement");
    }

    public IEnumerator FreeMouvement()
    {
        while (LevelManager.IsInsideArena(transform.position))
        {
            transform.position += transform.up * m_track.bulletSpeed * Time.deltaTime;
            yield return null;
        }

        // TODO : DestroyBullets
        Destroy(gameObject);

        yield return null;
    }
}
