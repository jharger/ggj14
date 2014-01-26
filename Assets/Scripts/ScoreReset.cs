using UnityEngine;
using System.Collections;

public class ScoreReset : MonoBehaviour {
	void Awake () {
		PlayerPrefs.SetInt("playerLoveLevel", 0);
		PlayerPrefs.SetInt("playerGreedLevel", 0);
		PlayerPrefs.SetInt("playerSmartLevel", 0);
	}
	
}
