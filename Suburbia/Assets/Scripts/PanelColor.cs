using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelColor : MonoBehaviour {

	Image image;

	void OnEnable()
	{
		SequenceController.FadingOut += DisableImage;
	}

	void OnDisable()
	{
		SequenceController.FadingOut -= DisableImage;
	}

	void Start()
	{
		image = GetComponent<Image> ();
	}

	void DisableImage()
	{
		StartCoroutine (disableImage ());
	}

	IEnumerator disableImage()
	{
		Color tempColor = image.color;
		float time = 0f;

		while (time <= 1f)
		{
			tempColor.a = Mathf.Lerp (1f, 0f, time);
			time += (Time.deltaTime * 2);
			image.color = tempColor;
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
