using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeployFallingObject : MonoBehaviour
{
    public GameObject FallingObjectPrefab;
    public float RespawnTime = 1.0f;
    private GameObject SpawnerBox;

    void Start()
    {
        /* Tyler McPhee
         *      Finds the object of the spawnerbox then starts the coroutine
         */
        SpawnerBox = GameObject.Find("FallingObjectSpawner");
        StartCoroutine(ObjectSpawn());
    }

    IEnumerator ObjectSpawn()
    {
        /* Tyler McPhee
         *      Loops to generate infinite objects
         *      Adds a copy of the falling object prefab to the game at the position of the spawner box
         */
        while (true)
        {
            yield return new WaitForSeconds(RespawnTime);
            GameObject fop = Instantiate(FallingObjectPrefab) as GameObject;
            fop.transform.position = new Vector2(Random.Range(-SpawnerBox.transform.position.x, SpawnerBox.transform.position.x), SpawnerBox.transform.position.y);
        }
    }
}
