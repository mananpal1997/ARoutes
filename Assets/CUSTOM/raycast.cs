using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class raycast : MonoBehaviour {
	
	RaycastHit mainhit;
	public Vector3 target;
	public Text t1,t2;
	public float dis;
	public float altitude;
	//public GameObject crosshair;
	//public GameObject catapult;
	// Use this for initialization
	void Start () {
		dis=4000;
		Input.location.Start();
		//LocationInfo li = new LocationInfo();
		mainhit=new RaycastHit();
	}
		
	// Update is called once per frame
	void Update () {
		print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
	//	altitude=li.longitude;
	//	Debug.Log ("long   "+ altitude.ToString ());
	//	Debug.Log ("lat   "+LocationInfo.latitude.ToString ());
	//	Debug.Log ("alt   "+LocationInfo.altitude.ToString ());
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));
		//= this.camera. ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay (ray.origin, ray.direction * 500 , Color.green);
		//Debug.DrawRay (camera.transform.position, Vector3.forward * 50, Color.green);
		
		
		if (Physics.Raycast (ray, out mainhit, dis)) {	


			t2.text=mainhit.point.ToString ();

			target.x = mainhit.point.x - 0.01f;
			target.y = mainhit.point.y - 0.01f;
			target.z = mainhit.point.z - 0.2f*Time.deltaTime;
		//	crosshair.transform.position = target;
			t1.text=target.ToString ();
		
}
	}
}
