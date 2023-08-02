using UnityEngine;

public class CrystalShardTarget : ATarget
{
	#region Fields

    [SerializeField]
    private Transform m_origin;

    public new Vector3 targeterScale
    {
        get
        {
            return new Vector3(
                m_origin.transform.localScale.x * m_targeterScale,
                m_origin.transform.localScale.y * m_targeterScale,
                1
            );
        }
    }

	#endregion

	#region Methods

	#endregion
}
