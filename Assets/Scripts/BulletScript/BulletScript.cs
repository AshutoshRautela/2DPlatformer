using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    public enum BulletOwner {RobotBullet, Soldier_1_Bullet};
    public BulletOwner bulletOwner;
    public float bulletVelocity = 0.004f;
    private Rigidbody2D myRigidBody;
    private Camera followCamera;
    private Vector3 viewPortPos;
    private GameObject bulletHitMark;

	private GameObject redBloodSplat;
	private GameObject blakDekal;
	private GameObject explosionPrefab;

	// Use this for initialization
	void Awake () {
        myRigidBody = this.GetComponent<Rigidbody2D>();

		redBloodSplat = Resources.Load<GameObject> ("Prefabs/Decals/red_bloodspat");;
		blakDekal = Resources.Load<GameObject> ("Prefabs/Decals/paint-splash");
		explosionPrefab = Resources.Load<GameObject> ("Prefabs/GroundExplosion");
	}

    void Start() {
        followCamera = CameraPlayerFollow.instance.gameObject.GetComponent<Camera>();
        bulletHitMark = Resources.Load<GameObject>("Prefabs/Player_HIT");
    }

    void OnSpawned() {
        if (bulletOwner == BulletOwner.Soldier_1_Bullet)
        {
            if (!PlayerMovement.instance.Right)
            {
                bulletVelocity = -Mathf.Abs(bulletVelocity);
            }
            else {
                bulletVelocity = Mathf.Abs(bulletVelocity);
            }

        }
        else if (bulletOwner == BulletOwner.RobotBullet) {
            if (PlayerMovement.instance.transform.position.x < this.transform.position.x)
            {
                bulletVelocity = -Mathf.Abs(bulletVelocity);
                this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            }
            else {
                bulletVelocity = Mathf.Abs(bulletVelocity);
                this.transform.localScale = new Vector3( -Mathf.Abs(this.transform.localScale.x ), this.transform.localScale.y, this.transform.localScale.z);
            }
        }
    }

    void Update() {
        //Check if bullet is Out of Screen	
		if(bulletOwner == BulletOwner.Soldier_1_Bullet){
			if (PlayerMovement.instance.transform.position.x < this.transform.position.x)
			{
				//	bulletVelocity = -Mathf.Abs(bulletVelocity);
				this.transform.localScale = new Vector3(0.6f, this.transform.localScale.y, this.transform.localScale.z);
			}
			else {
				//	bulletVelocity = Mathf.Abs(bulletVelocity);
				this.transform.localScale = new Vector3(-0.6f, this.transform.localScale.y, this.transform.localScale.z);
			}
		}
        viewPortPos = followCamera.WorldToViewportPoint(this.transform.position);
		this.transform.Translate (this.transform.right * bulletVelocity * Time.deltaTime);
        if (viewPortPos.x < -0.2f || viewPortPos.x > 1.2f)
        {
           PoolManager.Pools["Bullets"].Despawn(this.transform);
        }
    }

   void OnTriggerEnter2D(Collider2D otherCollider) {
        //Debug.Log("Bullet hits "+otherCollider.name);
        if (this.bulletOwner == BulletOwner.RobotBullet) {
            if (otherCollider.tag.Equals("Player"))
            {
                Debug.Log("Soldier Hit by Robot");
                PlayerMovement.instance.Health -= 2;   
                PoolManager.Pools["Bullets"].Despawn(this.transform);
            }
        }
        else if (this.bulletOwner == BulletOwner.Soldier_1_Bullet) {
            if (otherCollider.tag.Equals("EnemyPlayerTrigger"))
            {
//                Debug.Log("Bullet Hits Robot");
				RaycastHit2D hit;
				hit = Physics2D.Raycast (this.transform.position, this.transform.right, 100);
					
				if (this.name.Contains ("Roket")) {
					otherCollider.transform.parent.GetComponent<RobotMovement> ().Health -= 60;	
					PoolManager.Pools["Explosions"].Spawn(explosionPrefab.transform, new Vector3(hit.point.x, hit.point.y + 1
						, 0), Quaternion.identity);
					Transform newMark = PoolManager.Pools ["Decals"].Spawn (blakDekal.transform, new Vector3(otherCollider.transform.position.x + Random.Range(-0.3f,0.1f), hit.point.y, 0),Quaternion.identity);
					newMark.transform.parent = otherCollider.transform;
					otherCollider.transform.parent.GetComponent<RobotMovement> ().decals.Add (newMark);
				} else {					
					Transform newMark = PoolManager.Pools ["Decals"].Spawn (redBloodSplat.transform, new Vector3(otherCollider.transform.position.x + Random.Range(-0.3f,0.1f), hit.point.y, 0),Quaternion.identity);
					newMark.transform.parent = otherCollider.transform;
					otherCollider.transform.parent.GetComponent<RobotMovement> ().Health -= 20;		
					otherCollider.transform.parent.GetComponent<RobotMovement> ().decals.Add (newMark);
				}
				PoolManager.Pools["Bullet_Hit_Mark"].Spawn(bulletHitMark.transform, new Vector3(hit.point.x, hit.point.y, 0), Quaternion.identity);
                PoolManager.Pools["Bullets"].Despawn(this.transform);
            }
        }      
    }  
}
