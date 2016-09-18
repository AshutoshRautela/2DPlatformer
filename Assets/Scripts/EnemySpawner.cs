using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

#region Public_Variables
	public int enemyLimit;
	public int totalEnemys;
    public Transform enemyPrefab;
    public static EnemySpawner instance;
	private List<Transform> enemySpawnPoints;
#endregion

#region Private_Variables    
    private CameraPlayerFollow cameraFollowScript;
   [SerializeField]private Vector2 spawnLimit;
#endregion

    // Use this for initialization
    void Awake () {      
        if (instance == null) {
            instance = this;
        }
		enemySpawnPoints = new List<Transform> ();
		GameObject spawnPointsHolder = GameObject.Find ("EnemySpawnPoints");
		foreach (Transform item in spawnPointsHolder.transform) {
			if (!enemySpawnPoints.Contains(item)) {
				enemySpawnPoints.Add (item);
			}
		}
	}

    void Start() {
        cameraFollowScript = CameraPlayerFollow.instance.GetComponent<CameraPlayerFollow>();
      //  Invoke("SpawnEnemy", Random.Range(spawnLimit.x, spawnLimit.y));
		PlayerHUDPanel.playerGenerated += PlayerHUDPanel_playerGenerated;
    }

    void PlayerHUDPanel_playerGenerated (GameObject player)
    {
		Invoke("SpawnEnemy", Random.Range(spawnLimit.x, spawnLimit.y));
    }


    internal void SpawnEnemy() {     
		if (totalEnemys < enemyLimit) {
			totalEnemys++;
			Vector3 enemyViewPortPos = new Vector3(1.5f, 0.5f, 5);
			Vector3 enemyWorldPos = enemySpawnPoints [Random.Range (0, enemySpawnPoints.Count)].position + new Vector3(Random.Range(-2,2),0,0) ;
			GameObject newEnemy = PoolManager.Pools["Enemy"].Spawn(enemyPrefab.transform, enemyWorldPos, Quaternion.identity).gameObject;
			Invoke("SpawnEnemy", Random.Range(spawnLimit.x, spawnLimit.y));
		}      
    }
}
