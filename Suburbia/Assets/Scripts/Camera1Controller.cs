using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Camera1Controller : MonoBehaviour {

	public PostProcessingProfile pp;

	public AudioClip audioClip;
	public AudioClip clickSound;
	private AudioSource _audio;

	public float _depthField;
	private float _mainMenuDepth = 0.6f;

	public float _vignette;
	private float _mainMenuVignette = 1f;

	DepthOfFieldModel.Settings dopSettings;
	VignetteModel.Settings vigSettings;

	WaitForSeconds waitforSec = new WaitForSeconds(0.8f);

	void OnEnable()
	{
		SequenceController.FadingOut += DisableMainMenu;
	}

	void OnDisable()
	{
		SequenceController.FadingOut -= DisableMainMenu;	
	}

	void Awake()
	{
		dopSettings = pp.depthOfField.settings;
		dopSettings.focusDistance = _mainMenuDepth;
		pp.depthOfField.settings = dopSettings;

		vigSettings = pp.vignette.settings;
		vigSettings.intensity = _mainMenuVignette;
		pp.vignette.settings = vigSettings;
	}



	void DisableMainMenu()
	{
		StartCoroutine (disableMainMenu ());
		StartCoroutine (LerpCamPosition ());
		StartCoroutine (LerpCamRotation ());
	}

	IEnumerator disableMainMenu()
	{
		_audio = GetComponent<AudioSource> ();
		_audio.clip = clickSound;
		_audio.Play ();

		float time = 0f;
		while (time <= 1f)
		{
			dopSettings.focusDistance = Mathf.Lerp (_mainMenuDepth, _depthField, time);
			vigSettings.intensity = Mathf.Lerp (_mainMenuVignette, _vignette, time);
			time += (Time.deltaTime * 4f);
			pp.depthOfField.settings = dopSettings;
			pp.vignette.settings = vigSettings;
			yield return null;
		}
	}

	IEnumerator LerpCamPosition()
	{
		Vector3 newCamPosition = new Vector3 (-4.4f, 5.39f, -13f);
		Vector3 tempPosition = transform.localPosition;

		yield return waitforSec;

		_audio = GetComponent<AudioSource> ();
		_audio.clip = audioClip;
		_audio.Play ();

		float time = 0f;
		while (time <= 1f)
		{
			tempPosition = Vector3.Lerp (tempPosition, newCamPosition, time);
			time += Time.deltaTime;
			transform.localPosition = tempPosition;

			yield return null;
		}
	}

	IEnumerator LerpCamRotation()
	{
		Vector3 newCamRotaton = new Vector3 (16.15f, 3.13f, 0f);

		float x = transform.rotation.eulerAngles.x;
		float y = transform.rotation.eulerAngles.y;
		float z = transform.rotation.eulerAngles.z;

		Quaternion tempRotation = transform.localRotation;
		Vector3 rotation = tempRotation.eulerAngles;

		yield return waitforSec;

		float time = 0f;
		while (time <= 1f)
		{
			rotation.x = Mathf.Lerp (x, newCamRotaton.x, time);
			rotation.y = Mathf.Lerp (y, newCamRotaton.y, time);
			rotation.z = Mathf.Lerp (z, newCamRotaton.z, time);

			time += Time.deltaTime*2f;

			tempRotation.eulerAngles = rotation;
			transform.localRotation = tempRotation;

			yield return null;
		}

		yield return new WaitUntil (() => time >= 1f);

		SequenceController sequenceController = GameObject.Find ("GameController").GetComponent<SequenceController> ();

		sequenceController.SwitchOnGameObject ("GameScene");
	}
}
