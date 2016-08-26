using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
using Vuforia;
using System.IO;

public class Information{
	public string distance;
	public string duration;
}



public class locationtest : MonoBehaviour 
{
	public Text t1,t2,t3,t4,t5,t6;
	private bool startsendingloc;
	private bool getdestination;
	private float longit,latit,altit;
	private string url,jsonstring,completeurl,destination,locationlink;
	public InputField mainInputField;
	public GameObject sprite,buttonn,inputfieldd;
	void Start(){
		/* mainInputField = this.GetComponent<InputField>();
		var se= new InputField.SubmitEvent();
		se.AddListener(SubmitName);
		mainInputField.onEndEdit = se;*/
		mainInputField.onEndEdit.AddListener(SubmitName); 
		startsendingloc=false;
		getdestination=true;
	}
	public  void SubmitName(string arg0)
	{
		//t5.text=arg0;
		destination=arg0;
	}
	
	IEnumerator Start1()

	{
		// First, check if user has location service enabled
			if (!Input.location.isEnabledByUser)
				yield break;
			
			// Start service before querying location

	     	Input.location.Start();
			
			// Wait until service initializes
			int maxWait = 10;
			while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
			{
				yield return new WaitForSeconds(1);
				maxWait--;
			}
			
			// Service didn't initialize in 20 seconds
			if (maxWait < 1)
			{
				print("Timed out");
			    t1.text="timedout";
				yield break;
			}
			
			// Connection has failed
			if (Input.location.status == LocationServiceStatus.Failed)
			{
				print("Unable to determine device location");
		     	t1.text="Unable to determine device location";
				yield break;
			}
			else
			{
		
			longit=Input.location.lastData.longitude;
			latit=Input.location.lastData.latitude;
			altit=Input.location.lastData.altitude;
			
			t1.text=longit.ToString ()+"'";
			t2.text=latit.ToString ()+"'";
			t3.text=altit.ToString()+"'";
			//mainInputField.onEndEdit.AddListener(SubmitName);
		//	Debug.Log (mainInputField);
			completeurl = "http://192.168.43.23:8000/info/start="+latit.ToString()+ ","+ longit.ToString() +"&end="+destination;
			t6.text="url sent";
			locationlink = "http://192.168.43.23:8000/status/start="+latit.ToString()+ ","+ longit.ToString();
			if(getdestination){
			//	Debug.Log ("getdestination");
				StartCoroutine(DownloadJson(completeurl));
				Debug.Log ("getdestination");
				}
			if(startsendingloc){
				Debug.Log ("startsending");
				StartCoroutine (UpdateLocation(locationlink));
					}
			}


	}

	/*




			 IEnumerator DownloadJson(string url){
				Debug.Log("Reached coroutine");
				string jsonUrl = url;
				WWW dataFromUrl = new WWW(jsonUrl);
				yield return dataFromUrl;
				t1.text="before if";
				if(dataFromUrl.error == null){

					Debug.Log ("Url fetch Successful");
					//resultText1.text=dataFromUrl.text;
					GetJsonFromString(dataFromUrl.text);
				}else{
					Debug.Log ("Url fetch Error");
				}
			}
			
			void GetJsonFromString(string jsonURL){
				
				//JsonData mData = JsonMapper.ToObject(jsonURL);
			JsonData testdata = JsonMapper.ToObject (jsonURL);
			Information info=new Information();
			info.duration=testdata["info"][0]["duration"].ToString();
			info.distance=testdata["info"][1].ToString();
			t4.text=info.distance;
			t5.text=info.duration;

		}



			

*/
	

	      IEnumerator DownloadJson(string url){
		//t4.text="received";
		//url = "http://192.168.43.23:8000/info/start="+latit.ToString()+ ","+ longit.ToString() +"&end="+destination;
			WWW datafromurl = new WWW(url);
		yield return datafromurl;
		//	Debug.Log ("as");
		//	t4.text="asasa";
		//	if(datafromurl.error== null){
			//	Debug.Log (" loaded successfully"+ datafromurl.text);
			//t4.text=datafromurl.text.ToString();
			//	t5.text="7777sas45854454";	
				jsonstring=datafromurl.text;
		t5.text=jsonstring;
		byte[] bytes=System.Text.Encoding.UTF8.GetBytes(jsonstring);
		File.WriteAllBytes(Application.persistentDataPath+"/J.json",bytes);

		string temp=File.ReadAllText(Application.persistentDataPath+"/J.json");

				JsonData testdata = JsonMapper.ToObject (temp);
				Information info=new Information();
				info.duration=testdata["info"][1]["duration"].ToString();
				info.distance=testdata["info"][0]["distance"].ToString();
				//t4.text=info.distance;
				t5.text=info.duration;
	        	startsendingloc=true;

				//Functioncall(datafromurl.text);
}

		//	jsonstring=url;
		//	t3.text=url;
			//yield return www;

				

		
			
			// Stop service if there is no need to query location updates continuously
//			Input.location.Stop();
		





	public void Buttonpressed(){
	
		//t3.text="ashsihs";
		getdestination=true;
		StartCoroutine (Start1());
		sprite.SetActive (false);
		buttonn.SetActive (false);
		inputfieldd.SetActive (false);

	}

	public void FixedUpdate(){

		if(startsendingloc){

			Debug.Log ("ashish");
			speed ();
			StartCoroutine (Start1());

		
//			StartCoroutine(UpdateLocation(completeurl));


		}
		}
	IEnumerator UpdateLocation(string locurl)
		{

						yield return new WaitForSeconds(3.0f);
						//t4.text=destination;
						//locationlink = "http://192.168.43.23:8000/status/start="+latit.ToString()+ ","+ longit.ToString();
						WWW datafromurl = new WWW(locurl);
						yield return datafromurl;
						jsonstring=datafromurl.text;
						byte[] bytes=System.Text.Encoding.UTF8.GetBytes(jsonstring);
						File.WriteAllBytes(Application.persistentDataPath+"/updatedloc.json",bytes);
						string temp=File.ReadAllText(Application.persistentDataPath+"/updatedloc.json");
						
						JsonData testdata = JsonMapper.ToObject (temp);
						string message=testdata["message"].ToString ();
					   t6.text=message;
		}

	public void speed()
	{ float speed=Random.Range (3.50f,3.9f);
		t4.text=speed.ToString ()+"Km/hr";


	}

	}