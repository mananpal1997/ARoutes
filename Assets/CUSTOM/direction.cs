using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class direction : MonoBehaviour {

	public GameObject left,right,forward;
	public Text dire;
	int i=0;
	// Use this for initialization

	string[] dir = new string[] {" ","Head northeast toward Hostel 6 Enterance","Turn left toward Sardar Vallabhbhai Engineering College Rd","Turn right toward Sardar Vallabhbhai Engineering College Rd","Turn left toward Sardar Vallabhbhai Engineering College Rd","Turn right toward Sardar Vallabhbhai Engineering College Rd","Turn left onto Sardar Vallabhbhai Engineering College Rd","At Keval Chowk, take the 3rd exit", "Pass by Party Hunterz (on the right in 77m)","Turn right at Blue Planet onto Shiv Sadan Rd", "Destination will be on the right"};

	void Start () {

		left.SetActive (false);
		right.SetActive(false);
		forward.SetActive (true);
		dire.text=dir[0].ToString ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void leftb(){
		left.SetActive (true);
		right.SetActive(false);
		forward.SetActive (false);
		dire.text=dir[++i].ToString ();
	}
	public void rightb(){
		left.SetActive (false);
		right.SetActive(true);
		forward.SetActive (false);
		dire.text=dir[++i].ToString ();
	}

	public void forwardb(){
		left.SetActive (false);
		right.SetActive(false);
		forward.SetActive (true);
		dire.text=dir[++i].ToString ();
	}




}
