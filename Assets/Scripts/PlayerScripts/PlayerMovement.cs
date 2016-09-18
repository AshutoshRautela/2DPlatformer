using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {

#region Public_Variables
    public float playerSpeed = 11f;
    public static PlayerMovement instance = null;
    public float jumpForce = 10;
    public bool isGrounded = false;
   [SerializeField] private int health = 100;
#endregion

#region Private_Variables
    private bool right = true;
    private Animator playerAnimator;
    private float moveX = 0;
    private Rigidbody2D myRigidbody;
    private GamePlayPanel gamePlayPanelScript;
	[SerializeField]private bool simulateMoveRight = false;
	[SerializeField]private bool simulateMoveLeft = false;
	private CameraPlayerFollow cameraPlayerFollowSript;
#endregion

    // Use this for initialization
    void Awake () {
        if (instance == null) {
            instance = this;
        }
        playerAnimator = this.GetComponent<Animator>();
        myRigidbody = this.GetComponent<Rigidbody2D>();
        gamePlayPanelScript = GameObject.Find("GUI_Obj").transform.Find("GUI/GamePlay_Panel").GetComponent<GamePlayPanel>();
		cameraPlayerFollowSript = GameObject.Find ("Main Camera").GetComponent<CameraPlayerFollow> ();
		cameraPlayerFollowSript.focusPoint = this.transform.FindChild ("FocusPoint");
    }


	// Update is called once per frame
	void FixedUpdate () {
#if UNITY_EDITOR
     //   moveX = Input.GetAxis("Horizontal");
#endif
        playerAnimator.SetFloat("speed",Mathf.Abs(moveX));
        myRigidbody.velocity = new Vector2(moveX * playerSpeed * Time.deltaTime, myRigidbody.velocity.y);
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Jump();
        }
        if (moveX > 0 && !right) {
            Flip();
        }
        else if (moveX < 0 && right) {
            Flip();
        }
    }

	internal void Jump() {
//		Debug.Log ("Jump alled");
        if (isGrounded)
        {
	//		Debug.Log ("Player Jump Called");
            playerAnimator.SetBool("Jump",true);
            myRigidbody.AddForce(Vector2.up * jumpForce);
        }
    }

    private void Flip() {
        right = !right;
        Vector3 myScale = this.transform.localScale;
        myScale.x *= -1;
        this.transform.localScale = myScale;
    }

    internal bool Right {
        get {
            return right;
        }
    }

    internal float MoveX{
        get
        {
            return moveX;
        }
		set{
			moveX = value;
		}
    }

   void OnCollisionStay2D(Collision2D collider){		
		if (collider.gameObject.tag.Equals("Ground")) {			
			isGrounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D other){		
		if (other.gameObject.tag.Equals("Ground")) {			
			isGrounded = false;
		}
	}

    public int Health {
        set {
            health = value;
            gamePlayPanelScript.ReducePlayerHealth(health);
        }
        get { return health; }
    }

	public bool SimulateMovementLeft{
		set{
			simulateMoveLeft = value;
			if(value){
				moveX = -1;
			} else {
				if (!SimulateMovementRight) {
					moveX = 0;
				}
			}
		}
		get{
			return simulateMoveLeft;
		}	
	}

	public bool SimulateMovementRight {
		get{ 
			return simulateMoveRight;
		}
		set{ 
			simulateMoveRight = value;
			if (value) {
				moveX = 1;
			} else {
				if(!SimulateMovementLeft){
					moveX = 0;
				}
			}
		}

	}

}
