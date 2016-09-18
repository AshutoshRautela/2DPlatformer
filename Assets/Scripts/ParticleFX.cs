using UnityEngine;
using System.Collections;

public class ParticleFX : MonoBehaviour {
	
	// Use this for initialization
	void OnSpawned () {
		Invoke ("AutoDestruct",2);
	}

	private void AutoDestruct(){
		PoolManager.Pools ["EnemyDeathParticle"].Despawn (this.transform);
	}
}
