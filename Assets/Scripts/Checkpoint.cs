using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public GameObject[] checkpoints;

    public float speed;
    private static string savefile = System.IO.Directory.GetCurrentDirectory() + "\\PlayerSave.json";
    private static playersave psave;


    public class playersave
    {
        public int world = 0;
        public int level = 0;
        public int checkpoint = 0;

        public playersave()
        {
            load();
        }

        public void setWorld(int world)
        {
            this.world = world;
            save();
        }
        public void setLevel(int level)
        {
            this.level = level;
            save();
        }
        public void setCheckpoint(int checkpoint)
        {
            this.checkpoint = checkpoint;
            save();
        }
        public void incrementCheckpoint()
        {
            this.checkpoint++;
            save();
        }

        public void load()
        {
            if (System.IO.File.Exists(savefile))
            {
                 string json = System.IO.File.ReadAllText(savefile);
                 JsonUtility.FromJsonOverwrite(json, this);
                 Debug.Log("Save Loaded");
            }
            else
            {
                Debug.Log("Save Not found");
            }
        }
        public void save()
        {
            string json = JsonUtility.ToJson(psave);

            using (System.IO.StreamWriter sw = System.IO.File.CreateText(savefile))
            {
                sw.Write(json);
                sw.Close();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        psave = new playersave();
        psave.load();
        

        Debug.Log("Start");

        foreach(GameObject obj in checkpoints)
        {
            if (obj != null)
            {
                BoxCollider2D c = obj.AddComponent<BoxCollider2D>() as BoxCollider2D;
                c.isTrigger = true;
            }
        }
        
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    void OnTriggerEnter2D(Collider2D Collider)
    {
        int index = 0;
        foreach (GameObject obj in checkpoints)
        {
            if (Collider.gameObject == obj)
            {
                Debug.Log("Collison Checkpoint " + index);
                psave.incrementCheckpoint();
            }
            index++;
        }
        Debug.Log("Checkpoint Data: " + psave.checkpoint);
    }


    void FixedUpdate()
    {

    }
}
