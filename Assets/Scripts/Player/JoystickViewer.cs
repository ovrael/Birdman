using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickViewer : MonoBehaviour
{
	[SerializeField] Image circle;
	[SerializeField] Image handler;

	void HideJoystick()
	{
		circle.color = new Color(circle.color.r, circle.color.g, circle.color.b, 0);
		handler.color = new Color(circle.color.r, circle.color.g, circle.color.b, 0);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Vector3 touchPosition = touch.position;

			if (touchPosition.x >= 1600 && touchPosition.y <= 750)
			{
				circle.color = new Color(circle.color.r, circle.color.g, circle.color.b, 1);
				handler.color = new Color(circle.color.r, circle.color.g, circle.color.b, 1);
			}
			else
			{
				HideJoystick();
			}

			Debug.Log(touchPosition);
		}
		else
		{
			HideJoystick();
		}
	}

}
