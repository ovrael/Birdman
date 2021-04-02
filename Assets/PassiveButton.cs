using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveButtonOld : MonoBehaviour
{
	[SerializeField] bool isNodeActive = false;
	[SerializeField] PassiveNodeScript passiveNode;

	[SerializeField] Shader activeShader;
	[SerializeField] Shader inactiveShader;
	private Image image;

	private void Awake()
	{
		image = GetComponent<Image>();

		if (!isNodeActive)
			DeactivateNode();
	}


	public void ActivateNode()
	{
		Material material = image.material;
		material.shader = activeShader;
	}
	public void DeactivateNode()
	{
		Material material = image.material;
		material.shader = inactiveShader;
	}
}
