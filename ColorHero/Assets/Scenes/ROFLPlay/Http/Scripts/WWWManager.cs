using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ROFLPlay.Http
{
	public class WWWManager : MonoBehaviour
	{

		static List<WWWRequest> requests = new List<WWWRequest> ();
		int requestIndex;
		bool requestFlag;



		public void ClearQueue ()
		{
			requests.Clear ();
		}

		public void AddQueue (WWWRequest request)
		{
			requests.Add (request);
		}

		public void StartQueue ()
		{
			requestIndex = 0;

			StartCoroutine (RequestQueue ());
		}

		IEnumerator RequestQueue ()
		{
			do {
				WWWRequest request = FindRequest ();

				if (request == null) {
					break;
				}

				request.requestTime--;

				requestFlag = true;
				StartCoroutine (SendRequest (request));

				while (requestFlag) {
					yield return null;
				}
			} while(true);
		}

		WWWRequest FindRequest ()
		{
			for (int i = 0; i < requests.Count; i++) {
				if (requestIndex >= requests.Count) {
					requestIndex = 0;
				}

				WWWRequest request = requests [requestIndex];

				if (request.requestTime > 0) {
					requestIndex++;
					return request;
				}

				requestIndex++;
			}

			return null;
		}

		IEnumerator SendRequest (WWWRequest request)
		{
//			Debug.Log ("request url is " + request.url);
			WWW www = new WWW (request.url);

			yield return www;

			if (www.isDone && www.responseHeaders.ContainsKey ("STATUS") && www.responseHeaders ["STATUS"].IndexOf ("200") != -1) {
				request.requestTime = 0;

				if (request.response != null) {
					request.response.OnWWWResponse (request, www);
				}
			} else {
				if (request.response != null) {
					request.response.OnWWWResponse (request, null);
				}
			}

			requestFlag = false;
		}
	}

}

