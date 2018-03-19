using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTruck : MonoBehaviour {

	public GameObject redLight1;
	public GameObject redLight2;

	public GameObject blueLight1;
	public GameObject blueLight2;

	bool lightOn = true;

	void OnEnable()
	{
		SequenceController.FadingOut += StopLight;
		SequenceController.secondExam += StartLight;
	}

	void OnDisable()
	{
		SequenceController.FadingOut -= StopLight;
		SequenceController.secondExam -= StartLight;
	}

	void StopLight()
	{
		lightOn = false;
	}

	void StartLight()
	{
		lightOn = true;
	}

	void Update()
	{
		if (lightOn)
		{
			RotateClockWiseObject (redLight1);
			RotateClockWiseObject (redLight2);
			RotateAntiClockWiseObject (blueLight1);
			RotateAntiClockWiseObject (blueLight2);
		}
	}

	void RotateClockWiseObject(GameObject light)
	{
		Quaternion lightRotation = light.transform.rotation;

		Vector3 lightRotationVector = lightRotation.eulerAngles;

		lightRotationVector.y += Time.deltaTime * 500f;

		lightRotation.eulerAngles = lightRotationVector;

		light.transform.rotation = lightRotation;
	}

	void RotateAntiClockWiseObject(GameObject light)
	{
		Quaternion lightRotation = light.transform.rotation;

		Vector3 lightRotationVector = lightRotation.eulerAngles;

		lightRotationVector.y -= Time.deltaTime * 500f;

		lightRotation.eulerAngles = lightRotationVector;

		light.transform.rotation = lightRotation;
	}
}
