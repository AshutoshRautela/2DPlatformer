using UnityEngine;
using System.Collections;

public class MuzzleFlashScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
  

    void HideFlash() {
        //this.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.SetActive(false);
    }

}
