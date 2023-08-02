using UnityEngine;

public class CrystalShardTarget : ATarget
{
	#region Fields

	#endregion

	#region Methods

    private void Awake()
    {
        m_targeterScaleFactor *= m_origin.localScale.x;
    }

	#endregion
}
