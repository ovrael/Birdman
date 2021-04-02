using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionPassiveBranch : MonoBehaviour
{
	[Header("Color")]
	[SerializeField] Color availableBranchColor;
	[SerializeField] Color disableBranchColor;

	[Header("Spells")]
	[SerializeField] PassiveNodeScript previousNode;
	[SerializeField] PassiveNodeScript connectedNode;

	private Image branch;

	void Start()
	{
		branch = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		if (connectedNode.isPicked)
		{
			if (previousNode == null || previousNode.isPicked)
				branch.color = availableBranchColor;
		}
		else
		{
			branch.color = disableBranchColor;
		}
	}
}
