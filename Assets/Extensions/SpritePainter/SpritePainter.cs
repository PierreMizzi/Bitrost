using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class SpritePainter : MonoBehaviour
{
	#region Fields 

	[Header("Sprites")]
	[SerializeField] private List<Sprite> name = new List<Sprite>();

	[Header("Scale")]
	[SerializeField]
	private bool m_applyRandomScale = true;

	[SerializeField]
	private float m_minRandomScale = 1;

	[SerializeField]
	private float m_maxRandomScale = 2;

	[Header("Rotation")]
	[SerializeField]
	private bool m_applyRandomRotation = true;

	#endregion

	#region Methods 



	#endregion
}