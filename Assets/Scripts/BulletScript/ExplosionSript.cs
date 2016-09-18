using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionSript : MonoBehaviour {

	private Animator myANimator;

	// Use this for initialization
	void Awake () {
		myANimator = this.GetComponent<Animator> ();
	}
	
	void OnSpawned(){
		myANimator.Play ("GroundExplosion");
		Invoke ("SelfDestrut",0.6f);
	}


	private void SelfDestrut(){
		PoolManager.Pools ["Explosions"].Despawn (this.transform);
	}
}
