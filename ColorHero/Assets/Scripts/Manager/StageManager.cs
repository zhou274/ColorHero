using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ROFLPlay.Localization;
using ROFLPlay.DiffuseX;
using ROFLPlay.Bridge;
using TMPro;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;
using UnityEngine.SceneManagement;

namespace ColorHero
{
	public class StageManager : MonoBehaviour, DiffuseResponse, AdsResponse
	{

		class StageTargetData
		{
			public StageManager.CellType type;
			public int index;
		}

		public static StageManager instance;

		public TextMeshProUGUI titleStageText;
		public TextMeshProUGUI titleTimeText;
		public TextMeshProUGUI titleScoreText;

		public TextMeshProUGUI bestScoreText;
		public TextMeshProUGUI finalScoreText;

		public Animator scoreBonusAnimator;
		public TextMeshProUGUI scoreBonusText;

		int finalScore = 0;
		int stageScore = 0;

		public GameObject targetPanel;
		public TextMeshProUGUI targetPanelStageText;
		public TextMeshProUGUI targetPanelTextText;
		public TextMeshProUGUI targetPanelBorderText;
		public TextMeshProUGUI targetPanelRectText;
		


		public TextMeshProUGUI targetInfoTest;
		public TextMeshProUGUI targetInfoBorder;
		public TextMeshProUGUI targetInfoRect;


		public GameObject playingPanel;
		public GridLayoutGroup parent;


		public GameObject pausePanel;
		public GameObject winningPanel;
		public GameObject losingPanel;

		public TextMeshProUGUI stageTimeText;
		public TextMeshProUGUI stageScoreText;

		public Animator timeBonusAnimator;
		public TextMeshProUGUI timeBonusText;

		public float startTime = 60.0f;
		float leftTime = 60.0f, stageTime = 0f;
		float chainGap = 0f;
		int chainCount = 0;

		public StageData[] stages;

		public enum CellType
		{
			TEXT,
			BORDER,
			RECT}
		;

		public GameObject[] cellPrefabs;

		public ColorData[] colors;

		public int startStage;


		public AudioSource audioSourceTimer;
		public SFXPlayer sfxPlayer;


		int stage;

		[HideInInspector] public bool interactable = false;

		List<StageTargetData> targetList;

		bool isPaused, isGameOver;

		bool playerResponseFlag;

		const string KEY_BEST_SCORE = "BEST_SCORE";

		const string LOCALIZATION_KEY_TIME = "Time";
		const string LOCALIZATION_KEY_STAGE = "Stage";
		const string LOCALIZATION_KEY_SCORE = "Score";
		const string LOCALIZATION_KEY_TEXT = "Text";
		const string LOCALIZATION_KEY_BORDER = "Border";
		const string LOCALIZATION_KEY_RECT = "Rect";


        public string clickid;
        private StarkAdManager starkAdManager;

        void Awake ()
		{
			if (instance == null) {
				instance = this;
			} else if (instance != this) {
				Destroy (gameObject);
			}

			targetList = new List<StageTargetData> ();
		}

		void OnEnable ()
		{
			targetPanel.SetActive (false);
			playingPanel.SetActive (false);
			pausePanel.SetActive (false);
			winningPanel.SetActive (false);
			losingPanel.SetActive (false);

			StartGame ();
		}

		public void StartGame ()
		{
			stage = startStage;

			leftTime = startTime;

			finalScore = 0;

			isPaused = false;

			isGameOver = false;

			StartCoroutine (StageLoop ());
		}
		

		public void StartStage ()
		{
			playerResponseFlag = true;
		}

		public void NextStage ()
		{
			winningPanel.SetActive (false);

			stage++;

			if (Random.Range (0, 3) == 0) {
				if (DiffuseManager.instance.Diffuse (this)) {
					return;
				} else {
					if(AdsManager.instance.ShowInterstitialAd(this)) {
						return;
					}
				}
			}

			playerResponseFlag = true;
		}

		public void RestartStage ()
		{
			StopAllCoroutines ();

			losingPanel.SetActive (false);

			StartGame ();
		}

		public void PauseGame ()
		{
			pausePanel.SetActive (true);

			isPaused = true;
		}

		public void ResumeGame ()
		{
			pausePanel.SetActive (false);

			isPaused = false;
		}

		public void LeaveGame ()
		{
			StopAllCoroutines ();

			gameObject.SetActive (false);

			GameManager.instance.LeaveGame ();
		}

		public void OnMoreGameSelected()
		{
			ROFLPlay.DiffuseX.DiffuseManager.instance.MoreGame ();
		}

		IEnumerator StageLoop ()
		{
			yield return StartCoroutine (StageStarting ());
			yield return StartCoroutine (StagePlaying ());
			yield return StartCoroutine (StageEnding ());

			yield return StartCoroutine (StageLoop ());
		}

		IEnumerator StageStarting ()
		{
			interactable = false;

			titleStageText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_STAGE) + "\n " + stage;
			titleTimeText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_TIME) + "\n " + Mathf.Floor (leftTime);
			titleScoreText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_SCORE) + "\n " + finalScore;

			targetInfoTest.gameObject.SetActive (false);
			targetInfoBorder.gameObject.SetActive (false);
			targetInfoRect.gameObject.SetActive (false);

			targetPanel.SetActive (true);

			playerResponseFlag = false;

			StageData data = GetStageData (stage);

			InitStageTargets (data);
		
			int typeText = 0;
			int typeBorder = 0;
			int typeRect = 0;

			foreach (StageTargetData target in targetList) {
				switch (target.type) {
				case StageManager.CellType.TEXT:
					typeText++;
					break;
				case StageManager.CellType.BORDER:
					typeBorder++;
					break;
				case StageManager.CellType.RECT:
					typeRect++;
					break;
				}
			}

			targetPanelStageText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_STAGE) + " " + stage;

			if (typeText > 0) {
				targetPanelTextText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_TEXT) + "\nx" + typeText;
				targetPanelTextText.gameObject.SetActive (true);
			} else {
				targetPanelTextText.gameObject.SetActive (false);
			}

			if (typeBorder > 0) {
				targetPanelBorderText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_BORDER) + "\nx" + typeBorder;
				targetPanelBorderText.gameObject.SetActive (true);
			} else {
				targetPanelBorderText.gameObject.SetActive (false);
			}

			if (typeRect > 0) {
				targetPanelRectText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_RECT) + "\nx" + typeRect;
				targetPanelRectText.gameObject.SetActive (true);
			} else {
				targetPanelRectText.gameObject.SetActive (false);
			}

			while (!playerResponseFlag) {
				yield return null;
			}

			targetPanel.SetActive (false);
			playingPanel.SetActive (true);

			UpdateStageTargetInfo ();

			int buttonWidth = Screen.width * data.rowCount / data.cellCount;
			parent.cellSize = new Vector2 (buttonWidth, buttonWidth / 3);
			parent.constraintCount = data.rowCount;

			switch (Random.Range (0, 4)) {
			case 0:
				parent.startCorner = GridLayoutGroup.Corner.LowerLeft;
				break;
			case 1:
				parent.startCorner = GridLayoutGroup.Corner.LowerRight;
				break;
			case 2:
				parent.startCorner = GridLayoutGroup.Corner.UpperLeft;
				break;
			case 3:
				parent.startCorner = GridLayoutGroup.Corner.UpperRight;
				break;
			default:
				break;
			}

			for (int i = parent.transform.childCount - 1; i >= 0; i--) {
				GameObject cell = parent.transform.GetChild (i).gameObject;
				cell.transform.parent = null;
				Destroy (cell);
			}

			for (int i = 0; i < data.cellCount; i++) {
				bool isTarget = false;
				CellType type;

				StageTargetData target = CheckStagetTarget (i);
				if (target != null) {
					isTarget = true;
					type = target.type;
				} else {
					type = targetList [Random.Range (0, targetList.Count)].type;
				}

				int rightColorDataIndex = Random.Range (0, colors.Length);
				ColorData rightColorData = colors [rightColorDataIndex];
				ColorData wrongColorData = GetWrongColorData (rightColorDataIndex);

				switch (type) {
				case StageManager.CellType.TEXT:
					{
						GameObject button = Instantiate (cellPrefabs [0]) as GameObject;
						TextButton textButton = button.GetComponent<TextButton> ();
						textButton.Initialize (type, LocalizationManager.instance.GetLocalizedValue (rightColorData.name), isTarget ? rightColorData.color : wrongColorData.color, isTarget);
						button.transform.parent = parent.transform;
						break;
					}
				case StageManager.CellType.BORDER:
					{
						GameObject button = Instantiate (cellPrefabs [1]) as GameObject;
						ImageButton imageButton = button.GetComponent<ImageButton> ();
						imageButton.Initialize (type, rightColorData.color, LocalizationManager.instance.GetLocalizedValue (isTarget ? rightColorData.name : wrongColorData.name), wrongColorData.color, isTarget);
						button.transform.parent = parent.transform;
						break;
					}
				case StageManager.CellType.RECT:
					{
						GameObject button = Instantiate (cellPrefabs [2]) as GameObject;
						ImageButton imageButton = button.GetComponent<ImageButton> ();
						imageButton.Initialize (type, rightColorData.color, LocalizationManager.instance.GetLocalizedValue (isTarget ? rightColorData.name : wrongColorData.name), wrongColorData.color, isTarget);
						button.transform.parent = parent.transform;
						break;
					}
				}
			}

			float interval = Mathf.Clamp (1.0f / data.cellCount, 0.05f, 0.5f);
			int[] indexs = new int[parent.transform.childCount];
			for (int i = 0; i < parent.transform.childCount; i++) {
				indexs [i] = i;
			}
			for (int i = 0, leftCount = parent.transform.childCount; i < parent.transform.childCount; i++) {
				int rand = Random.Range (0, leftCount);

				Transform transform = parent.transform.GetChild (indexs [rand]);
				TextButton button = transform.gameObject.GetComponent<TextButton> ();
				if (button != null) {
					button.SetVisible ();
				}

				indexs [rand] = indexs [leftCount - 1];
				leftCount--;

				yield return new WaitForSeconds (interval);
			}

			stageTime = 0f;
			stageScore = 0;
			chainCount = 0;
			chainGap = 0f;

			interactable = true;
		}

		IEnumerator StagePlaying ()
		{
			while (targetList.Count > 0) {
				if (!isPaused) {
					leftTime -= Time.deltaTime;
					stageTime += Time.deltaTime;
					chainGap += Time.deltaTime;

					if (leftTime <= 10f) {
						if (!audioSourceTimer.isPlaying) {
							audioSourceTimer.Play ();
						}
					} else {
						if (audioSourceTimer.isPlaying) {
							audioSourceTimer.Stop ();
						}
					}
				}

				if (leftTime <= 0) {
					isGameOver = true;
					break;
				}

				titleTimeText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_TIME) + "\n" + Mathf.Floor (leftTime);

				yield return null;
			}

			while (true) {
				bool flag = false;
				TextButton[] cells = parent.GetComponentsInChildren<TextButton> ();
				foreach (TextButton textButton in cells) {
					if (textButton.IsAnimating ()) {
						flag = true;
					}
				}

				if (flag) {
					yield return null;
				} else {
					break;
				}
			}
		}




		public void AddTime()
		{
            ShowVideoAd("2lgn0lki0mhf2311ii",
            (bol) => {
                if (bol)
                {
                    //stage = startStage;
                    StopAllCoroutines();
                    isGameOver = false;
                    leftTime += 30;
                    losingPanel.SetActive(false);
                    StartCoroutine(StageLoop());



                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
            
        }



		IEnumerator StageEnding ()
		{
			if (audioSourceTimer.isPlaying) {
				audioSourceTimer.Stop ();
			}

			if (!isGameOver) {
				sfxPlayer.PlayWin ();

				winningPanel.SetActive (true);
				stageTimeText.text = Mathf.Floor (stageTime) + "";
				stageScoreText.text = stageScore + "";
                ShowInterstitialAd("6ai0h928ccefbbg0je",
            () => {

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });
            } else {
				GameServicesManager.instance.SubmitScore (finalScore);

				sfxPlayer.PlayLose ();

				losingPanel.SetActive (true);
				finalScoreText.text = finalScore + "";

				int best = PlayerPrefs.GetInt (KEY_BEST_SCORE, 0);
				if (best <= finalScore) {
					best = finalScore;
					PlayerPrefs.SetInt (KEY_BEST_SCORE, best);
				}
				bestScoreText.text = best + "";
                ShowInterstitialAd("6ai0h928ccefbbg0je",
            () => {

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });
            }
				
			playerResponseFlag = false;
			while (!playerResponseFlag) {
				yield return null;
			}
		}


		StageData GetStageData (int stage)
		{
			if (stage >= stages.Length) {
				return stages [stages.Length - 1];
			} else {
				return stages [stage - 1];
			}
		}


		void InitStageTargets (StageData data)
		{
			targetList.Clear ();

			int[] indexs = new int[data.cellCount];
			int indexCount = data.cellCount;
			for (int i = 0; i < data.cellCount; i++) {
				indexs [i] = i;
			}

			for (int i = 0; i < data.targetCount; i++) {
				StageTargetData target = new StageTargetData ();

				int type = Random.Range (0, cellPrefabs.Length);
				switch (type) {
				case 0:
					target.type = StageManager.CellType.TEXT;
					break;
				case 1:
					target.type = StageManager.CellType.BORDER;
					break;
				case 2:
					target.type = StageManager.CellType.RECT;
					break;
				default:
					break;
				}

				int r = Random.Range (0, indexCount);
				target.index = indexs [r];
				indexs [r] = indexs [indexCount - 1];
				indexCount--;

				targetList.Add (target);
			}
		}

		StageTargetData CheckStagetTarget (int i)
		{
			foreach (StageTargetData data in targetList) {
				if (data.index == i) {
					return data;
				}
			}

			return null;
		}

		ColorData GetWrongColorData (int rightIndex)
		{
			int[] indexs = new int[colors.Length - 1];

			for (int i = 0, j = 0; i < colors.Length; i++) {
				if (i != rightIndex) {
					indexs [j++] = i;
				}
			}

			int rand = indexs [Random.Range (0, indexs.Length)];

			return colors [rand];
		}


		public void SubmitCell (StageManager.CellType type, bool isRight)
		{
			if (isRight) {
				foreach (StageTargetData target in targetList) {
					if (target.type == type) {
						targetList.Remove (target);
						break;
					}
				}

				UpdateStageTargetInfo ();

				float firstSightBonus = Mathf.Max (0, (5.0f - stageTime));

				if (chainGap >= 3f) {
					chainCount = 0;
				} else {
					chainCount++;
				}
				chainGap = 0f;

				sfxPlayer.PlayRightWithStep (chainCount);

				float chainBonus = Mathf.Pow (3f, (float)chainCount);


				int score = (int)Mathf.Floor ((firstSightBonus + chainBonus) * 10);

				stageScore += score;
				finalScore += score;

				scoreBonusText.text = "+" + score;
				scoreBonusAnimator.SetTrigger ("Bonus");

				titleScoreText.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_SCORE) + "\n" + finalScore;

				int bonusTime = (int)Mathf.Floor (firstSightBonus + chainBonus);
				timeBonusText.text = "+" + bonusTime;
				timeBonusAnimator.SetTrigger ("Bonus");

				leftTime += bonusTime;
			} else {
				sfxPlayer.PlayWrong ();

				timeBonusAnimator.SetTrigger ("Penalty");

				leftTime -= 1f;
			}
		}

		private void UpdateStageTargetInfo ()
		{
			int typeText = 0;
			int typeBorder = 0;
			int typeRect = 0;

			foreach (StageTargetData target in targetList) {
				switch (target.type) {
				case StageManager.CellType.TEXT:
					typeText++;
					break;
				case StageManager.CellType.BORDER:
					typeBorder++;
					break;
				case StageManager.CellType.RECT:
					typeRect++;
					break;
				}
			}

			if (typeText > 0) {
				targetInfoTest.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_TEXT) + " x " + typeText;
				targetInfoTest.gameObject.SetActive (true);
			} else {
				targetInfoTest.gameObject.SetActive (false);
			}

			if (typeBorder > 0) {
				targetInfoBorder.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_BORDER) + " x " + typeBorder;
				targetInfoBorder.gameObject.SetActive (true);
			} else {
				targetInfoBorder.gameObject.SetActive (false);
			}

			if (typeRect > 0) {
				targetInfoRect.text = LocalizationManager.instance.GetLocalizedValue (LOCALIZATION_KEY_RECT) + " x " + typeRect;
				targetInfoRect.gameObject.SetActive (true);
			} else {
				targetInfoRect.gameObject.SetActive (false);
			}
		}




		public void OnDiffuseResponse (ROFLPlay.DiffuseX.ResponseCode code)
		{
			playerResponseFlag = true;
		}

		public void OnInterstitialAdDismissed ()
		{
			playerResponseFlag = true;
		}

		public void OnRewardedVideoAdDidRewardUser ()
		{
		}

		public void OnRewardedVideoAdDismissed ()
		{
			playerResponseFlag = true;
		}
        public void getClickid()
        {
            var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
            if (launchOpt.Query != null)
            {
                foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                    if (kv.Value != null)
                    {
                        Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                        if (kv.Key.ToString() == "clickid")
                        {
                            clickid = kv.Value.ToString();
                        }
                    }
                    else
                    {
                        Debug.Log(kv.Key + "<-参数-> " + "null ");
                    }
            }
        }

        public void apiSend(string eventname, string clickid)
        {
            TTRequest.InnerOptions options = new TTRequest.InnerOptions();
            options.Header["content-type"] = "application/json";
            options.Method = "POST";

            JsonData data1 = new JsonData();

            data1["event_type"] = eventname;
            data1["context"] = new JsonData();
            data1["context"]["ad"] = new JsonData();
            data1["context"]["ad"]["callback"] = clickid;

            Debug.Log("<-data1-> " + data1.ToJson());

            options.Data = data1.ToJson();

            TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
               response => { Debug.Log(response); },
               response => { Debug.Log(response); });
        }


        /// <summary>
        /// </summary>
        /// <param name="adId"></param>
        /// <param name="closeCallBack"></param>
        /// <param name="errorCallBack"></param>
        public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
        {
            starkAdManager = StarkSDK.API.GetStarkAdManager();
            if (starkAdManager != null)
            {
                starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
            }
        }
        /// <summary>
        /// 播放插屏广告
        /// </summary>
        /// <param name="adId"></param>
        /// <param name="errorCallBack"></param>
        /// <param name="closeCallBack"></param>
        public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
        {
            starkAdManager = StarkSDK.API.GetStarkAdManager();
            if (starkAdManager != null)
            {
                var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
                mInterstitialAd.Load();
                mInterstitialAd.Show();
            }
        }

        public void Restert()
        {
            SceneManager.LoadScene("Main");
        }
    }



}

