using UnityEngine;
using System.Collections;

public class LightFlickering : MonoBehaviour {
	
	Light flickeringLight;
	private float lightIntensity;

	void Awake()
	{
		flickeringLight = GetComponent<Light> ();
	}

	void  Update (){


		StartCoroutine("Wait");

	}

	IEnumerator  Wait (){
		yield return new WaitForSeconds(Random.Range(0.5f,5.0f));
		lightIntensity = (Random.Range(1.0f,2.0f));
		flickeringLight.intensity = lightIntensity;
	}


}