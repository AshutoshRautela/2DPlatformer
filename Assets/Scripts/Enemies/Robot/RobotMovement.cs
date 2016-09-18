using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotMovement : MonoBehaviour
{

    #region Public_Variables
	public List<Transform> decals;
    private float enemySpeed = 700;
    [SerializeField]private int health = 100;
	private float playerEnemyDistanceThreshold = 4f;
	[SerializeField]private GameObject enemyDeathParticle;
    public Transform bulletPrefab;
    #endregion

    #region Private_Variables
    private bool moveRight = false;
    private Transform playerTransform;
    private Rigidbody2D myRigidBody;
    private Vector3 myScale;
    private float moveX = 1;
    private Transform healthBar;

    [SerializeField]
    private float myVelocity;
	[SerializeField]private bool followPlayer = true;
	[SerializeField]private bool canFollowPlayer = false;
    [SerializeField]private float distanceWithPlayer;    
    private Animator myAnimator;
    private Transform bulletSpawnPoint;
    private Transform bulletMuzzleFlash;
	private float heightDistane;
	private Transform pooManager;
    #endregion

    // Use this for initialization



    void Awake()
    {        
        myRigidBody = this.GetComponent<Rigidbody2D>();
        myScale = this.transform.localScale;
        myAnimator = this.GetComponent<Animator>();
        healthBar = this.transform.Find("HealthValue/BG_Filler/Filler_GO/Filler").transform;
        bulletSpawnPoint = this.transform.Find("SpawnPoints/BulletSpawnPoint").transform;
        bulletMuzzleFlash = bulletSpawnPoint.transform.parent.transform.FindChild("Muzzle_Flash");
		pooManager = GameObject.Find ("PoolManager").transform;
		PlayerHUDPanel.playerGenerated += PlayerHUDPanel_playerGenerated;
    }

    void PlayerHUDPanel_playerGenerated (GameObject player)
    {
		playerTransform = player.transform;
    }

    void Start()
    {
      //  playerTransform = PlayerMovement.instance.transform;
		enemySpeed = Random.Range(250,450);
    }

	void SpawnDeathEffect(){
		PoolManager.Pools ["EnemyDeathParticle"].Spawn (enemyDeathParticle.transform, this.transform.position, Quaternion.identity);
	}

    void OnSpawned()
    {
        Health = 100;
		enemySpeed = Random.Range(250,450);
        playerEnemyDistanceThreshold = Random.Range(2, 3);
		if (!playerTransform) {
			try{
				playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;	
			}
			catch(UnityException ex){
				Debug.Log ("annot find Initial Player");
			}
		}
		foreach(Transform deal in decals){
			deal.transform.parent = pooManager;
			PoolManager.Pools ["Decals"].Despawn (deal);
		}
		decals.Clear ();
    }

    // Update is called once per frame
    void Update()
    {
		if (playerTransform) {
			SetDirection();	
			//RandomMovement ();     
			Movement();
		}	
    }

	private void RandomMovement(){
		myAnimator.SetFloat("speed",1);
		myRigidBody.velocity = new Vector2(Mathf.Sin(1) * enemySpeed * Time.deltaTime, 0);
		myVelocity = myRigidBody.velocity.magnitude;        
	}

    private bool FollowPlayer {
        set {
            followPlayer = value;
            if (value)
            {
                StartCoroutine("RefollowPlayer");
            }
            else {
                StopCoroutine("RefollowPlayer");
                canFollowPlayer = false;
                myAnimator.SetFloat("speed", 0);
            }
        }
        get { return followPlayer;  }
    }

    internal void Movement() {
		//Debug.Log ("Movement Funtion");
		if (heightDistane <= 2) {
			if (distanceWithPlayer >= playerEnemyDistanceThreshold) {
				if (!FollowPlayer) {
					FollowPlayer = true;                
				}
				MovePlayer ();
			} else {
				if (FollowPlayer) {
					FollowPlayer = false;
				}            
			}
		}
		distanceWithPlayer = Vector2.Distance(this.transform.position,playerTransform.position);        
		heightDistane = Mathf.Abs (playerTransform.position.y - this.transform.position.y);
    }

    private IEnumerator RefollowPlayer()
    {
        myAnimator.SetBool("shoot",true);
        yield return new WaitForSeconds(1.5f);
        myAnimator.SetBool("shoot", false);
        canFollowPlayer = true;
    }

    internal void MovePlayer() {
        if (canFollowPlayer)
        {
            myAnimator.SetFloat("speed",1);
            myRigidBody.velocity = new Vector2(moveX * enemySpeed * Time.deltaTime, 0);
            myVelocity = myRigidBody.velocity.magnitude;            
        }
    }

    internal void SetDirection()
    {
        if (playerTransform.position.x < this.transform.position.x)
        {
            MoveRight = false;
        }
        else if (playerTransform.position.x > this.transform.position.x)
        {
            MoveRight = true;
        }
    }

    private bool MoveRight {
        set {
            moveRight = value;
            if (moveRight)
            {
                this.transform.localScale = myScale;
                moveX = 1;
            }
            else {
                this.transform.localScale = new Vector3(-myScale.x, myScale.y, myScale.z);
                moveX = -1;
            }
        }
        get { return moveRight; }
    }

    public int Health {
        set {
            value = Mathf.Clamp(value, 0, 100);   
            health = value;
            healthBar.localScale = new Vector3((float)health/100, healthBar.localScale.y, healthBar.localScale.z);

            if (health == 0) {
				foreach(Transform deal in decals){
					deal.transform.parent = pooManager;
					PoolManager.Pools ["Decals"].Despawn (deal);
				}
				decals.Clear ();
                PoolManager.Pools["Enemy"].Despawn(this.transform);
				SpawnDeathEffect ();
				EnemySpawner.instance.totalEnemys--;
            }
        }
        get {
            return health;
        }
    }

    private void GenerateBullet() {
		Debug.Log ("Robot Shoot alled");
        GameObject generateBullet = PoolManager.Pools["Bullets"].Spawn(bulletPrefab.transform, bulletSpawnPoint.position, Quaternion.identity).gameObject;
        bulletMuzzleFlash.gameObject.SetActive(true);
    }

}
