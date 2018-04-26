using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public delegate void ResponseCallBack (QuestionEntity questionObject);

public class WebserviceManager : MonoBehaviour
{
	private readonly string QUESTIONS_URL = "https://private-5b1d8-sampleapi187.apiary-mock.com/questions";

	private static WebserviceManager mInstance;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		DontDestroyOnLoad (this);
	}

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static WebserviceManager Instance {
		get {
			if (mInstance == null) {
				mInstance = new GameObject ("WebserviceManager").AddComponent<WebserviceManager> ();
			}
			return mInstance;
		}
	}

	/// <summary>
	/// Gets the data from server.
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void GetDataFromServer (ResponseCallBack callback)
	{
		StartCoroutine (GetDataFromServerCor (callback));
	}

	/// <summary>
	/// Gets the data from server cor.
	/// </summary>
	/// <returns>The data from server cor.</returns>
	/// <param name="callback">Callback.</param>
	private IEnumerator GetDataFromServerCor (ResponseCallBack callback)
	{
		WWW www = new WWW (QUESTIONS_URL);
		yield return www;
		QuestionEntity questionObject = null;
		if (!string.IsNullOrEmpty (www.error)) {
			print ("Error : " + www.error);
		} else {
			string serviceData = www.text;
			string jsonStr = JSON.Parse (serviceData) [0].ToString ();
			Debug.Log (jsonStr);
			questionObject = JsonUtility.FromJson<QuestionEntity> (jsonStr);
		}
		callback (questionObject);
	}
}

[System.Serializable]
public class QuestionEntity
{
	public string question;
	public string published_at;
	public ChoiceEntity[] choices;
}

[System.Serializable]
public class ChoiceEntity
{
	public string choice;
	public string votes;
}
