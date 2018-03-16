using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHeadingColor : MonoBehaviour {

	public GradientScript gradientScript;

	void OnEnable()
	{
		SequenceController.FadingOut += DisableImage;
	}

	void OnDisable()
	{
		SequenceController.FadingOut -= DisableImage;
	}

	void DisableImage()
	{
		StartCoroutine (disableImage ());
	}

	IEnumerator disableImage()
	{
		Color tempTopColor = gradientScript.topColor;
		Color tempBottomColor = gradientScript.bottomColor;

		float time = 0f;

		while (time <= 1f)
		{
			tempTopColor.a = Mathf.Lerp (1f, 0f, time);
			tempBottomColor.a = Mathf.Lerp (gradientScript.bottomColor.a, 0f, time);
			time += (Time.deltaTime * 4f);
			gradientScript.topColor = tempTopColor;
			gradientScript.bottomColor = tempBottomColor;
			yield return null;
		}

		StartCoroutine (disableGameObject ());
	}

	IEnumerator disableGameObject()
	{
		this.gameObject.SetActive (false);
		yield return null;
	}

}
