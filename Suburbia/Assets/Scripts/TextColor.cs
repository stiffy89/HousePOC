using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColor : MonoBehaviour {

	Text text;

	void OnEnable()
	{
		SequenceController.FadingOut += DisableText;
	}

	void OnDisable()
	{
		SequenceController.FadingOut -= DisableText;
	}

	void Start()
	{
		text = GetComponent<Text> ();
	}

	void DisableText()
	{
		StartCoroutine (disableText ());
	}

	IEnumerator disableText()
	{
		Color tempColor = text.color;
		float time = 0f;

		while (time <= 1f)
		{
			tempColor.a = Mathf.Lerp (1f, 0f, time);
			time += (Time.deltaTime * 4f);
			text.color = tempColor;
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
