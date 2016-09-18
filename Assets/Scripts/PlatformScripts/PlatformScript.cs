using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformScript : MonoBehaviour {
	
	public GameObject enemyPrefab;
	public Transform player;


	void Awake(){

	}

	void Start(){
		PlayerHUDPanel.playerGenerated += PlayerGenerated;

	}

	private void PlayerGenerated(GameObject player){
		this.player = player.transform;
	}

	private void SpawnEnemy(){
		
	}

	// Update is called once per frame
	void Update () {
		if (!player) {
			return;
		}
	
	}
}
