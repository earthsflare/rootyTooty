﻿// THis script prevents Z-fightitng on the camera view
// Perspective2DSortMode.cs
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode()]
public class Perspective2DSortMode : MonoBehaviour {
	void Awake () {
		GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
	}
}