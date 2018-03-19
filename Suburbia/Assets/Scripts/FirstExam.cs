using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstExam : MonoBehaviour {
	
	CurrentTruckPosition currentTruckPosition;
	public static CurrentTimerState currentTimerState;
	GameObject _firetruck;
	WaitForSeconds waitforsec = new WaitForSeconds (0.5f);
	WaitForSeconds waitToSelectTimer = new WaitForSeconds(3f);

	public AudioClip toggleSound;
	public AudioClip timerSound;
	public AudioClip timerComplete;
	AudioSource audioSource;

	#region Enums
	public enum CurrentTruckPosition
	{
		Position1,
		Position2,
		Position3,
	}

	public enum CurrentTimerState
	{
		WaitingToCount,
		Counting,
		FinishedCounting,
	}
	#endregion

	#region unity callbacks
	void OnEnable()
	{
		SequenceController.FadingOut += OpeningMenu;
		gameObject.AddComponent<AudioSource> ();
	}

	void Start()
	{
		StartCoroutine (UIDropDown ());
		currentTruckPosition = CurrentTruckPosition.Position1;
		_firetruck = GameObject.FindGameObjectWithTag ("Firetruck");

		audioSource = GetComponent<AudioSource> ();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.A))
		{
			ToggleTruckPosition ();
			TimerState ();
		}
	}

	#endregion

	#region UIMenu

	void OpeningMenu()
	{
		GameObject firetruck = GameObject.FindGameObjectWithTag ("FiretruckMesh");
		Material[] fireTruckMaterials = firetruck.GetComponent<MeshRenderer> ().materials;

		Color truckColor = fireTruckMaterials [1].color;
		truckColor.a = 0f;
		fireTruckMaterials [1].color = truckColor;

		fireTruckMaterials[1].SetFloat("_Mode", 3);
		fireTruckMaterials[1].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
		fireTruckMaterials[1].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		fireTruckMaterials[1].SetInt("_ZWrite", 0);
		fireTruckMaterials[1].DisableKeyword("_ALPHATEST_ON");
		fireTruckMaterials[1].DisableKeyword("_ALPHABLEND_ON");
		fireTruckMaterials[1].EnableKeyword("_ALPHAPREMULTIPLY_ON");
		fireTruckMaterials[1].renderQueue = 3000;
	}
		
	IEnumerator UIDropDown()
	{
		GameObject gameSceneUI = GameObject.FindGameObjectWithTag ("GameSceneUI");

		RectTransform UIrt = gameSceneUI.GetComponent<RectTransform> ();
		Vector3 rectPos = UIrt.localPosition;
		float time = 0f;
		while (time <= 1f)
		{
			rectPos.y = Mathf.Lerp (350f, 250f, time);
			time += Time.deltaTime * 2f;
			UIrt.localPosition = rectPos;
			yield return null;
		}
	}

	#endregion

	#region Timer

	IEnumerator startCircleTimer(GameObject icon)
	{

		//get icon rectT
		RectTransform iconRect = icon.GetComponent<RectTransform>();

		//Vector3 for positioning
		Vector3 iconPosition = iconRect.localPosition;

		//Vector3 for rotation
		Quaternion iconQuaternion = iconRect.localRotation;
		Vector3 iconRotation = iconQuaternion.eulerAngles;

		//Vector3 for sizing
		Vector2 iconSize = iconRect.sizeDelta;

		//move the current circle up to perform some cool animation
		float t = 0f;

		while (t <= 1f)
		{
			//increase size
			iconSize.x = Mathf.Lerp(25f, 50f, t);
			iconSize.y = Mathf.Lerp (25f, 50f, t);

			//increase Y position
			iconPosition.y = Mathf.Lerp(0f, 60f, t);

			//apply rotation 
			iconRotation.x = Mathf.Lerp(0f, 360f, t);

			t += Time.deltaTime;

			iconRect.sizeDelta = iconSize;

			iconRect.localRotation = iconQuaternion;
			iconQuaternion.eulerAngles = iconRotation;

			iconRect.localPosition = iconPosition;

			yield return null;
		}
	}

	IEnumerator RingEffect(GameObject icon)
	{
		// play the "bing" sound when the timer is complete
		audioSource.clip = timerComplete;
		audioSource.Play ();

		//floating ring effect

		icon.GetComponent<Image> ().fillAmount = 1f;

		//get icon rectT
		RectTransform iconRect = icon.GetComponent<RectTransform>();

		//Vector3 for sizing
		Vector2 iconSize = iconRect.sizeDelta;

		//fade the ring color out
		Image iconImage = icon.GetComponent<Image>();
		Color iconColor = iconImage.color;

		float time = 0f;

		while (time <= 1f)
		{
			iconSize.x = Mathf.Lerp (25f, 70f, time);
			iconSize.y = Mathf.Lerp (25f, 70f, time);

			iconColor.a = Mathf.Lerp (1f, 0f, time);

			time += Time.deltaTime * 2f;

			iconImage.color = iconColor;
			iconRect.sizeDelta = iconSize;

			yield return null;
		}

		yield return null;
	}

	#endregion

	#region StateControllerFunctions
		
	void ToggleTruckPosition()
	{
		//the audio the is associated with the toggling
		audioSource.clip = toggleSound;
		audioSource.Play ();

		switch (currentTruckPosition)
		{
			case CurrentTruckPosition.Position1:
				
				Vector3 position2 = new Vector3 (866f, 29.5f, 450f);
				_firetruck.transform.localPosition = position2;
				_firetruck.transform.localRotation = Quaternion.Euler (0f, 0f, 0f);
				currentTruckPosition = CurrentTruckPosition.Position2;
				break;
			
			case CurrentTruckPosition.Position2:
				Vector3 position3 = new Vector3 (870f, 29.5f, 470f);
				_firetruck.transform.localPosition = position3;
				_firetruck.transform.localRotation = Quaternion.Euler (0f, 0f, 0f);
				currentTruckPosition = CurrentTruckPosition.Position3;
				break;

			case CurrentTruckPosition.Position3:
				Vector3 position1 = new Vector3 (870f, 29.5f, 439f);
				_firetruck.transform.localPosition = position1;
				_firetruck.transform.localRotation = Quaternion.Euler (0f, 40f, 0f);
				currentTruckPosition = CurrentTruckPosition.Position1;
				break;	
		}
	}

	void TimerState()
	{
		switch (currentTimerState)
		{
			case CurrentTimerState.WaitingToCount:
				StopAllCoroutines ();
				StartCoroutine (waitToSelect ());
				currentTimerState = CurrentTimerState.WaitingToCount;
				break;

			case CurrentTimerState.Counting:
				StopAllCoroutines ();
				StartCoroutine (originalPosition ());
				currentTimerState = CurrentTimerState.WaitingToCount;
				break;

			case CurrentTimerState.FinishedCounting:
				//move to the next panel
				break;

		}
	}

	#endregion

	#region Select Truck Position

	IEnumerator waitToSelect()
	{
		yield return waitToSelectTimer;

		//get the current icon
		GameObject icon = GameObject.FindGameObjectWithTag("GameScene").GetComponent<StepsIconController>().selectedIcon;
		StartCoroutine (selectPosition (icon));
		currentTimerState = CurrentTimerState.Counting;
	}

	IEnumerator selectPosition(GameObject icon)
	{
		yield return StartCoroutine (startCircleTimer (icon));

		// slight delay of half a second to let everything settle
		yield return waitforsec;

		//get the image component of the icon
		Image iconImage = icon.GetComponent<Image>();
		float fillAmount = iconImage.fillAmount;
		iconImage.fillClockwise = false;

		//playthetickingaudio
		audioSource.clip = timerSound;
		audioSource.Play ();

		float time = 0f;

		while (time <= 30f)
		{	
			fillAmount = Mathf.Lerp (1f, 0f, time / 30f);
			time += Time.deltaTime*2f;
			iconImage.fillAmount = fillAmount;

			yield return null;
		}

		audioSource.Stop ();
		StartCoroutine (Completed (icon));

	}

	#endregion

	#region Completed

	IEnumerator Completed(GameObject icon)
	{

		GameObject parent = icon.transform.parent.gameObject;

		foreach (Transform child in parent.transform)
		{
			if (child.name == "Complete")
			{
				GameObject completeGameObject = child.gameObject;
				Image completeGameObjectImage = completeGameObject.GetComponent<Image> ();
				Color color = completeGameObjectImage.color;

				completeGameObject.transform.localPosition = icon.transform.localPosition;

				float t = 0f;
				while (t <= 1f)
				{
					color.a = Mathf.Lerp (0f, 1f, t);
					t += Time.deltaTime * 4f;
					completeGameObjectImage.color = color;
					yield return null;
				}

				yield return StartCoroutine(RingEffect(icon));

				StartCoroutine (LerpToOriginalPosition (completeGameObject));
			}
		}

		//deactivate the ring
		icon.SetActive(false);

		// returning the truck back to its full opacity

		GameObject firetruck = GameObject.FindGameObjectWithTag ("FiretruckMesh");
		Material[] fireTruckMaterials = firetruck.GetComponent<MeshRenderer> ().materials;

		Color truckColor = fireTruckMaterials [1].color;
		truckColor.a = 1f;
		fireTruckMaterials [1].color = truckColor;

		fireTruckMaterials[1].SetFloat("_Mode", 0);
		fireTruckMaterials[1].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
		fireTruckMaterials[1].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
		fireTruckMaterials[1].SetInt("_ZWrite", 1);
		fireTruckMaterials[1].DisableKeyword("_ALPHATEST_ON");
		fireTruckMaterials[1].DisableKeyword("_ALPHABLEND_ON");
		fireTruckMaterials[1].EnableKeyword("_ALPHAPREMULTIPLY_ON");
		fireTruckMaterials[1].renderQueue = -1;

	}

	#endregion

	#region original position

	IEnumerator LerpToOriginalPosition(GameObject completeGameObject)
	{
		Vector3 tempVector = completeGameObject.transform.localPosition;
		Vector3 _originalPosition = new Vector3 (12.5f, 0f, 0f);

		float _t = 0f;
		while (_t <=1f)
		{
			tempVector = Vector3.Lerp (completeGameObject.transform.localPosition, _originalPosition, _t);
			_t += Time.deltaTime * 4f;
			completeGameObject.transform.localPosition = tempVector;
			yield return null;
		}

		currentTimerState = CurrentTimerState.FinishedCounting;
	}
		
	IEnumerator originalPosition()
	{

		//get the current icon
		GameObject icon = GameObject.FindGameObjectWithTag("GameScene").GetComponent<StepsIconController>().selectedIcon;

		//get icon rectT
		RectTransform iconRect = icon.GetComponent<RectTransform>();

		//Vector3 for positioning
		Vector3 iconPosition = iconRect.localPosition;

		//Vector3 for rotation
		Quaternion iconQuaternion = iconRect.localRotation;
		Vector3 iconRotation = iconQuaternion.eulerAngles;

		//Vector3 for sizing
		Vector2 iconSize = iconRect.sizeDelta;

		//return the progress back to a full circle
		Image iconImage = icon.GetComponent<Image>();
		iconImage.fillAmount = 1f;

		//move the current circle back to its original form
		float t = 0f;

		while (t <= 1f)
		{
			//increase size
			iconSize.x = Mathf.Lerp(iconSize.x, 25f, t);
			iconSize.y = Mathf.Lerp (iconSize.y, 25f, t);

			//increase Y position
			iconPosition.y = Mathf.Lerp(iconPosition.y, 0f, t);

			//apply rotation 
			iconRotation.x = Mathf.Lerp(iconRotation.x, 0f, t);

			t += Time.deltaTime * 2;

			iconRect.sizeDelta = iconSize;

			iconRect.localRotation = iconQuaternion;
			iconQuaternion.eulerAngles = iconRotation;

			iconRect.localPosition = iconPosition;

			yield return null;
		}

		StartCoroutine (waitToSelect ());
	}
	#endregion
}
