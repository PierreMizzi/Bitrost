using UnityEngine;

public enum TargetType
{
    None,
    Module,
    CrystalShard
}

[RequireComponent(typeof(BoxCollider2D))]
public abstract class ATarget : MonoBehaviour
{
	#region Fields

    [SerializeField]
    private TargetType m_type = TargetType.None;
    public TargetType type
    {
        get { return m_type; }
    }

    [SerializeField]
    protected float m_targeterScale = 1f;

    public Vector3 targeterScale
    {
        get { return new Vector3(m_targeterScale, m_targeterScale, m_targeterScale); }
    }

	#endregion

	#region Methods

	#endregion
}
