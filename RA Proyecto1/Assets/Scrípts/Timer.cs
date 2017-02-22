using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

 public Text timerText;
 //public float tiempo = 0.0f;
 private float startTime;

void Start (){
	startTime = Time.time;
}
 public void Update() {
 	float t = Time.time - startTime;
 	//tiempo -= Time.deltaTIme;
 	//tiempoText.text = "" + tiempo.ToString("f0");
 	string minutes = ((int) t / 60).ToString();
 	string seconds = (t % 60).ToString("f2");
 	timerText.text = minutes + ":" + seconds;
 }
}
