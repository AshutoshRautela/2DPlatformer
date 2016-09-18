using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUDPanel : MonoBehaviour {

#region Public_Variables
	public delegate void PlayerGenerated(GameObject player);
	public static event PlayerGenerated playerGenerated;

	public Transform andyPrefab;
	public Transform sairaPrefab;

	public Text playerNameHeading;
	public Text playerInfromationText;

	public string[] playerNames = { "ANDY" , "SAIRA" };
	public string[] playerIntro = {"Desription_1","Desription_2"};
#endregion

#region Private_Variables
	private Transform andy;
	private Transform saira;
	private string playerOpted = "";

#endregion

	void Awake(){
		andy = this.transform.Find("RightAnchor/ANDY");
		saira = this.transform.Find("RightAnchor/SAIRA");
	
	}


	// Use this for initialization
	void Start () {
		OnLeftButtonPressed ();
	}
	
	public void OnLeftButtonPressed(){
		if(GameManager.instane.myPlayer == GameManager.PlayerType.ANDY){
			LTDescr moveTween = LeanTween.moveLocal (andy.gameObject, new Vector3(-500, -4 ,0), 0.2f);
			//moveTween.setFrom (new Vector3(200 , -10 , 0));

			LTDescr moveTween2 = LeanTween.moveLocal (saira.gameObject, new Vector3(-230 ,-4 ,0), 0.2f);
			moveTween2.setFrom (new Vector3(230,-4,0));
			GameManager.instane.myPlayer = GameManager.PlayerType.SAIRA;
		}
		else if (GameManager.instane.myPlayer == GameManager.PlayerType.SAIRA) {
			LTDescr moveTween = LeanTween.moveLocal (andy.gameObject, new Vector3(-265 ,-10 ,0), 0.2f);
			moveTween.setFrom (new Vector3(265,-10,0));
			LTDescr moveTween2 = LeanTween.moveLocal (saira.gameObject, new Vector3(-500 ,-4 ,0), 0.2f);

			GameManager.instane.myPlayer = GameManager.PlayerType.ANDY;
		}
		UpdatePlayerIntro ();
	}

	public void OnRightButtonPressed(){
		if(GameManager.instane.myPlayer == GameManager.PlayerType.ANDY){
			LTDescr moveTween = LeanTween.moveLocal (andy.gameObject, new Vector3(265, -4 ,0), 0.2f);

			LTDescr moveTween2 = LeanTween.moveLocal (saira.gameObject, new Vector3(-230,-4 ,0), 0.2f);
			moveTween2.setFrom (new Vector3 (-500, -4, 0));

			GameManager.instane.myPlayer = GameManager.PlayerType.SAIRA;
		}
		else if (GameManager.instane.myPlayer == GameManager.PlayerType.SAIRA) {
			LTDescr moveTween = LeanTween.moveLocal (andy.gameObject, new Vector3(-265 ,-10 ,0), 0.2f);
			moveTween.setFrom (new Vector3(-500 , -4 , 0));
			LTDescr moveTween2 = LeanTween.moveLocal (saira.gameObject, new Vector3(230 ,-4 ,0), 0.2f);

			GameManager.instane.myPlayer = GameManager.PlayerType.ANDY;
		}
		UpdatePlayerIntro ();
	}

	private void UpdatePlayerIntro(){
		if (GameManager.instane.myPlayer == GameManager.PlayerType.ANDY) {
			playerNameHeading.text = "ANDY";
			playerInfromationText.text = playerIntro [0];
			playerOpted = "ANDY";
		}
		else if (GameManager.instane.myPlayer == GameManager.PlayerType.SAIRA) {
			playerNameHeading.text = "SAIRA";
			playerInfromationText.text = playerIntro [1];
			playerOpted = "SAIRA";
		}
	}

	public void OnPlayButtonPressed(){
		this.gameObject.SetActive (false);
		if (playerOpted.Equals("ANDY")) {
			GameObject andy = Instantiate (andyPrefab.gameObject, new Vector3 (0, 5, 0), Quaternion.identity) as GameObject;
			playerGenerated (andy);
		}
		else if (playerOpted.Equals("SAIRA")) {
			GameObject saira = Instantiate (sairaPrefab.gameObject, new Vector3 (0, 5, 0), Quaternion.identity) as GameObject;
			playerGenerated (saira);
		}

	}
}
