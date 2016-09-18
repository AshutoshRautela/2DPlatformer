using UnityEngine;
using System.Collections;
using System.Collections.Generic;



/// <description>
///		Add description of the class
/// </description>
public class SimpleSpawner : MonoBehaviour 
{
    /// <summary>
    /// The prefab to spawn from.
    /// </summary>
    public string poolName;
    public Transform testPrefab;
    public int spawnAmount = 50;
    public float spawnInterval = 0.25f;

    public string particlesPoolName;
    public ParticleSystem particleSystemPrefab;  // <-- ParticleEmitter

    private SpawnPool shapesPool; 
    private SpawnPool particlesPool;

    private void Start()
    {
        this.shapesPool = PoolManager.Pools[this.poolName];

        this.StartCoroutine(Spawner());
        this.StartCoroutine(ParticleSpawner());
    }


    /// <summary>
    /// Spawn a particle instance at start, and respawn it to test particle reactivation
    /// </summary>
    private IEnumerator ParticleSpawner()
    {
        this.particlesPool = PoolManager.Pools[this.particlesPoolName];

        ParticleSystem prefab = this.particleSystemPrefab;
        Vector3 prefabXform = this.particleSystemPrefab.transform.position;
        Quaternion prefabRot = this.particleSystemPrefab.transform.rotation;

        // Spawn an emitter that will auto-despawn when all particles die
        //  testEmitterPrefab is already a ParticleEmitter, so just pass it.
        // Note the return type is also a ParticleEmitter
        ParticleSystem emitter = this.particlesPool.Spawn(prefab, prefabXform, prefabRot);

        while (emitter.IsAlive(true))
        {
            // Wait for a little while to be sure we can see it despawn
            yield return new WaitForSeconds(3);
        }

        this.particlesPool.Spawn(prefab, prefabXform, prefabRot);
    }


    /// <summary>
    /// Spawn an instance every this.spawnInterval
    /// </summary>
    private IEnumerator Spawner()
    {
        int count = this.spawnAmount;
        Transform inst;
        while (count > 0)
        {
            // Spawn in a line, just for fun
            inst = this.shapesPool.Spawn(this.testPrefab);
            inst.localPosition = new Vector3((this.spawnAmount+2) - count, 0, 0);
            count--;

            yield return new WaitForSeconds(this.spawnInterval);
        }

        this.StartCoroutine(Despawner());

        yield return null;
    }


    /// <summary>
    /// Despawn an instance every this.spawnInterval
    /// </summary>
    private IEnumerator Despawner()
    {
        // create a short reference to the pool
        while (this.shapesPool.Count > 0)
        {
            // Despawn the last instance (like dequeue in a queue because 
            //   Despawn() will also remove the item from the list, so the list
            //   is being changed in place.)
            Transform instance = this.shapesPool[shapesPool.Count - 1];
            this.shapesPool.Despawn(instance);  // Internal count--

            yield return new WaitForSeconds(this.spawnInterval);
        }

        yield return null;
    }

}