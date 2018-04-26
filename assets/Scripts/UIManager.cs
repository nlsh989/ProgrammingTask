using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private GameObject choicePrefab;
	[SerializeField]
	private GameObject refreshIcan;
	[SerializeField]
	private Text questionText;
	[SerializeField]
	private Transform choicesPanel;

	private bool mIsRotating = false;

	/// <summary>
	/// Raises the clicked refresh button event.
	/// </summary>
	public void OnClickedRefreshBtn ()
	{
		if (!mIsRotating) {
			//Hit the webservice here
			WebserviceManager.Instance.GetDataFromServer (ServerResponseCallBack);
			StartCoroutine (RotateCor ());
		}
	}

	/// <summary>
	/// Rotates the cor.
	/// </summary>
	/// <returns>The cor.</returns>
	IEnumerator RotateCor ()
	{
		mIsRotating = true;
		float speed = 1f;
		while (mIsRotating) {
			refreshIcan.transform.Rotate (Vector3.forward * speed);
			yield return null;
		}
	}

	/// <summary>
	/// Servers the response call back.
	/// </summary>
	/// <param name="questionObject">Question object.</param>
	void ServerResponseCallBack (QuestionEntity questionObject)
	{
		mIsRotating = false;
		if (questionObject != null) {
			StartCoroutine (SetUICor (questionObject));
		}
	}

	/// <summary>
	/// Sets the user interface cor.
	/// </summary>
	/// <returns>The user interface cor.</returns>
	/// <param name="questionObject">Question object.</param>
	IEnumerator SetUICor (QuestionEntity questionObject)
	{
		//clear old ui
		for (int i = 0; i < choicesPanel.childCount; i++) {
			Destroy (choicesPanel.GetChild (i).gameObject);
		}

		//wait until destroy old ui
		while (choicesPanel.childCount > 0) {
			yield return new WaitForEndOfFrame ();
		}

		//set new ui
		questionText.text = questionObject.question;
		foreach (var choiceObj in questionObject.choices) {
			GameObject clone = Instantiate (choicePrefab, choicesPanel);
			Text[] textArr = clone.GetComponentsInChildren<Text> ();
			textArr [0].text = choiceObj.choice;
			textArr [1].text = choiceObj.votes;
		}
	}

}
