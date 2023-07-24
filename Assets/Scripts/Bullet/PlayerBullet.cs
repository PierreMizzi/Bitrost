using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;

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
                time,
                0,
                1,
                (float value) =>
                {
                    transform.position = m_track.spline.EvaluatePosition(value);
                }
            )
            .OnComplete(FireFromTurret);
    }

    public void FireFromTurret() { }
}
