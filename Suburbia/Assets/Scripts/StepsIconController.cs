using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsIconController : MonoBehaviour {

	List <GameObject> stepIcons = new List <GameObject> ();

	public Color completeColor;

	public GameObject selectedIcon;

	void Start()
	{
		foreach (Transform child in transform)
		{
			if (child.CompareTag ("StepIcon"))
			{
				stepIcons.Add (child.gameObject);
			}
		}

		foreach (Transform item in stepIcons[0].transform)
		{
			if (item.gameObject.name == "Incomplete")
			{
				HighLightIcon (item.gameObject);
			}
		}
	}

	void ColorIcon(Color color)
	{

	}

	void HighLightIcon(GameObject icon)
	{
		StartCoroutine (highLightIcon (icon));
		selectedIcon = icon;
	}

	IEnumerator highLightIcon(GameObject _icon)
	{
		Image iconImage = _icon.GetComponent<Image> ();
		float time = 0f;

		while (time <= 1f)
		{
			Color tempColor = iconImage.color;
			tempColor = Color.Lerp (iconImage.color, completeColor, time);
			time += Time.deltaTime;
			iconImage.color = tempColor;

			yield return null;
		}
	}

	void Update()
	{
	}
}
