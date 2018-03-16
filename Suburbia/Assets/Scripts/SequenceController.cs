using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceController : MonoBehaviour {

	public enum CurrentLearningState
	{
		MainMenu,
		SituationAssessment,
		PriorityState,
		ExecutingObjectives,
	}

	public delegate void FadeOut();
	public static event FadeOut FadingOut;
	public CurrentLearningState currentState;

	GameObject RigidBodyFPSController;
	GameObject CameraController;
	GameObject OpeningScene;
	GameObject GameScene;

	List <GameObject> gameObjects = new List <GameObject>();

	void Awake()
	{
		RigidBodyFPSController = GameObject.FindGameObjectWithTag ("Player");
		CameraController = GameObject.FindGameObjectWithTag ("OverPlayer");
		OpeningScene = GameObject.FindGameObjectWithTag ("OpeningScene");
		GameScene = GameObject.FindGameObjectWithTag ("GameScene");

		gameObjects.Add (RigidBodyFPSController);
		gameObjects.Add (CameraController);
		gameObjects.Add (OpeningScene);
		gameObjects.Add (GameScene);
	}

	void OnEnable()
	{
		RigidBodyFPSController.SetActive (false);
		GameScene.SetActive (false);
		CameraController.SetActive (true);
		OpeningScene.SetActive (true);
	}

	void Start()
	{
		currentState = CurrentLearningState.MainMenu;
	}

	public void SwitchOnGameObject(string objectName)
	{
		foreach (GameObject _object in gameObjects)
		{
			if (_object.name == objectName)
			{
				_object.SetActive (true);
			}
		}
	}

	void Update()
	{

		if (Input.GetKeyDown (KeyCode.A))
		{
			switch (currentState)
			{
				case CurrentLearningState.MainMenu:
					currentState = CurrentLearningState.SituationAssessment;
					if (FadingOut != null)
					{
						FadingOut ();
					}
					break;
				case CurrentLearningState.SituationAssessment:
					if (FirstExam.currentTimerState == FirstExam.CurrentTimerState.FinishedCounting)
					{
						currentState = CurrentLearningState.PriorityState;
						if (FadingOut != null)
						{
							FadingOut ();
						}
					}
					break;
			}
		}
	}
}
