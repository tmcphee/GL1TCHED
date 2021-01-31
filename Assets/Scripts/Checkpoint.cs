using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public GameObject[] checkpoints;

    private static string savefile = System.IO.Directory.GetCurrentDirectory() + "\\PlayerSave.json";
    private static playersave psave;


    public class playersave
    {
        private int world = 0;
        private int level = 0;
        private int checkpoint = 0;

        public playersave()
        {
            load();
        }

        //Sets Level data
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

        //Returns Level data
        public int getWorld()
        {
            return this.world;
        }
        public int getLevel()
        {
            return this.level;
        }
        public int getCheckpoint()
        {
            return this.checkpoint;
        }


        //Loads level data from file
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
        //Saves level data to file
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

    //Finds an checkpoint object index in checkpoints array
    public int find_checkpoint_index(GameObject checkpoint)
    {
        int index = 0;
        foreach (GameObject obj in checkpoints)
        {
            if (obj == checkpoint)
            {
                return index;
            }
            index++;
        }
        return -1;
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

    //Checks if the player touched a checkpoint
    void OnTriggerEnter2D(Collider2D Collider)
    {
        foreach (GameObject obj in checkpoints)
        {
            if (Collider.gameObject == obj)
            {
                Debug.Log("Collison Checkpoint: " + find_checkpoint_index(obj) + "\nCheckpoint Data: " + psave.getCheckpoint());
                psave.incrementCheckpoint();
            }
        }
    }


    void FixedUpdate()
    {

    }
}
