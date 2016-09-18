using UnityEngine;
using System.Collections;

public class PlayerFightingSkills : MonoBehaviour {

#region Public_Variables
	public Transform roketPrefab;
    public Transform yellowBulletPrefab;
    public Transform yellowMuzzleFlash;
	public static PlayerFightingSkills instance;
#endregion

#region Private_Variables
    private Animator playerAnimator;
    private PlayerMovement playerMovementScript;

    //Spawn Points
    private Transform bulletSpawnPoint;
    private AudioSource bulletAudioSource;

#endregion

    // Use this for initialization
    void Awake () {
        playerAnimator = this.GetComponent<Animator>();
        playerMovementScript = this.GetComponent<PlayerMovement>();
        bulletSpawnPoint = this.transform.Find("SpawnPoints/BulletSpawnPoint");
        bulletAudioSource = this.GetComponent<AudioSource>();
		instance = this;
    }
	
	// Update is called once per frame
	void Update () {
       // FightBehaviour();
	}

	internal void PlayerFightAction(bool playerShoot){
		FightBehaviour (playerShoot);
	}

	internal void FightBehaviour(bool playerShoot) {
		if (playerShoot) {
            playerAnimator.SetBool("shoot",true);
        } else {
			playerAnimator.SetBool("shoot", false);
		}
    }

    internal void GenerateBullet() {      
        bulletAudioSource.Play();
		GameObject newBullet = this.gameObject;
		if (this.name.Contains ("ANDY")) {
			newBullet = PoolManager.Pools ["Bullets"].Spawn (yellowBulletPrefab.transform).gameObject;
		}
		else if(this.name.Contains("SAIRA")) {
			newBullet = PoolManager.Pools ["Bullets"].Spawn (roketPrefab.transform).gameObject;
		}
		newBullet.transform.position = bulletSpawnPoint.position;
		newBullet.transform.eulerAngles = new Vector3 (0 , 0 , Random.Range(-1,1));
        yellowMuzzleFlash.gameObject.SetActive(true);
    }    
}
