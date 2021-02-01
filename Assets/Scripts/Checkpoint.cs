using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool UseCheckpointGlitch = false;
    public GameObject[] checkpoints;
    public GameObject FinishCheckpoint;
    
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
        public void incrementLevel()
        {
            this.level++;
            save();
        }
        public void incrementWorld()
        {
            this.world++;
            save();
        }
        public void resetdata()
        {
            this.world = 0;
            this.level = 0;
            this.checkpoint = 0;
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
                Debug.Log("SAVE: " + json);
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

    //Gets the position of the last checkpoint
    public Vector3 GetLastCheckpointPosition()
    {
        //Make sure the last checkpoint is in range of array
        int lastcheckpoint = psave.getCheckpoint();
        if (lastcheckpoint > checkpoints.Length)
        {
            lastcheckpoint = checkpoints.Length - 1;
        }
        return checkpoints[lastcheckpoint].transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        psave = new playersave();
        psave.load();
        
        Debug.Log("Start");

        //Loop though all the defined checkpoint objects and add a BoxCollider2D with isTrigger set
        foreach(GameObject obj in checkpoints)
        {
            if (obj != null)
            {
                BoxCollider2D c = obj.AddComponent<BoxCollider2D>() as BoxCollider2D;
                c.isTrigger = true;
            }
        }

        //adds a BoxCollider2D with isTrigger set to the finished checkpoint if exists
        if (FinishCheckpoint != null)
        {
            BoxCollider2D fc = FinishCheckpoint.AddComponent<BoxCollider2D>() as BoxCollider2D;
            fc.isTrigger = true;
        }

        psave.resetdata();
    }

    //Checks if the player touched a checkpoint
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if(Collider.gameObject == FinishCheckpoint)
        {
            Debug.Log("Player Finished Level");
            psave.incrementLevel();
            psave.setCheckpoint(0);
            return;
        }

        foreach (GameObject obj in checkpoints)
        {
            if (Collider.gameObject == obj)
            {
                int hit_checkpoint = find_checkpoint_index(obj);
                int last_checkpoint = psave.getCheckpoint();
                if (UseCheckpointGlitch)//If defined then increment checkpoint else only increment if checkpoint is the next
                {
                    psave.incrementCheckpoint();
                }
                else if (hit_checkpoint > last_checkpoint)
                {
                    psave.setCheckpoint(hit_checkpoint);
                }
                Debug.Log("Hit Checkpoint: " + find_checkpoint_index(obj));
                return;
            }
        }
    }


    void FixedUpdate()
    {

    }
}
