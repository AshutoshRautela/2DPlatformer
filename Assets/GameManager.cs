using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

#region Public_Variables
	public enum PlayerType {ANDY , SAIRA};
	public PlayerType myPlayer = PlayerType.ANDY;

	public static GameManager instane ;
#endregion

#region Private_Variables

#endregion



	// Use this for initialization
	void Awake () {
		instane = this;
	}
	
	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit ();
		}
	}
}
