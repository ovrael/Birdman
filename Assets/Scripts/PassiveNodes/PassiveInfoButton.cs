using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassiveInfoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] GameObject passiveInfo;
	[SerializeField] PassiveNodeScript passiveNode;

	TMP_Text passiveName;
	TMP_Text description;

	private void Start()
	{
		passiveName = passiveInfo.transform.Find("Name").GetComponent<TMP_Text>();
		description = passiveInfo.transform.Find("Description").GetComponent<TMP_Text>();
	}

	private void SetDescription()
	{
		passiveName.text = passiveNode.nodeName;
		description.text = passiveNode.description;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		SetDescription();
		passiveInfo.SetActive(true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		passiveInfo.SetActive(false);
	}
}
