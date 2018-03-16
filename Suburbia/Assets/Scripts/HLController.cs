using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class HLController : MonoBehaviour {

	Highlighter highlighter;

	// Use this for initialization
	void Awake () 
	{
		highlighter = GetComponent<Highlighter> ();
	}
	
	void Start()
	{
		highlighter.ConstantOn (Color.green);
	}
}
