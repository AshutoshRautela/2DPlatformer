using UnityEngine;
using System.Collections;

public class CameraPlayerFollow : MonoBehaviour {

 #region Public_Variables
    public enum CameraFollowType {Update, FixedUpdate, LateUpdate };
    public CameraFollowType followFunction = CameraFollowType.Update;
    public Transform focusPoint;
    public Vector2 followOffset;
    public Vector2 smoothTime = new Vector2(5, 2);
    public static CameraPlayerFollow instance;
 #endregion

 #region Private_Variables
    private Vector2 refVel;
    private Vector2 newPos;
 #endregion

    // Use this for initialization
    void Awake () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (!focusPoint) {
			return;
		}
        if (followFunction == CameraFollowType.Update)
        {			
            FollowFunctionality();
        }
	}

    void FixedUpdate() {
		if (!focusPoint) {
			return;
		}
        if (followFunction == CameraFollowType.FixedUpdate)
        {
            FollowFunctionality();
        }
    }

    void LateUpdate() {
		if (!focusPoint) {
			return;
		}
        if (followFunction == CameraFollowType.LateUpdate)
        {
            FollowFunctionality();
        }
    }

    void FollowFunctionality() {
        //this.transform.position = Vector3.SmoothDamp(this.transform.position, new Vector3(focusPoint.position.x + followOffset.x, focusPoint.position.y + followOffset.y, this.transform.position.z), ref refVel, 3 * Time.deltaTime);
        newPos.x = Mathf.SmoothDamp(this.transform.position.x, focusPoint.position.x + followOffset.x, ref refVel.x, smoothTime.x * Time.deltaTime);
        newPos.y = Mathf.SmoothDamp(this.transform.position.y, focusPoint.position.y + followOffset.y, ref refVel.y, smoothTime.y * Time.deltaTime);

        this.transform.position = new Vector3(newPos.x, newPos.y, this.transform.position.z);
    }
}
