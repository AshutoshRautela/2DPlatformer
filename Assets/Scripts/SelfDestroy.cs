using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {

    public float despawnTime = 2;

	// Use this for initialization
	void Awake () {
	
	}

    void OnSpawned() {
        Invoke("SelfDespawn",despawnTime);
    }

    void SelfDespawn() {
        PoolManager.Pools["Bullet_Hit_Mark"].Despawn(this.transform);
    }
}
