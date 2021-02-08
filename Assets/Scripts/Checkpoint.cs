using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using playersave = PlayerSave.playersave;

public class Checkpoint : MonoBehaviour
{
    public bool UseCheckpointGlitch = false;
    public bool ResetCheckpointOnStart = false;
    public bool CheckpointVelocityGlitch = false;
    public Rigidbody2D r;
    public GameObject[] checkpoints;
    public GameObject FinishCheckpoint; 
    public TMP_Text timeText;

    private static playersave psave;
    private float levelTime;
    private int minutes = 1;
    private bool finished = false;

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

    public void SetPlayerLastCheckpoint()
    {
        r.transform.position = r.GetComponent<Checkpoint>().GetLastCheckpointPosition();
        if (CheckpointVelocityGlitch == false)
        {
            r.velocity = new Vector3(0, 0, 0);
        }
        
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
        //return checkpoints[lastcheckpoint].transform.position;
        return new Vector3(checkpoints[lastcheckpoint].transform.position.x, checkpoints[lastcheckpoint].transform.position.y, 0);
    }

    void Awake()
    {
        psave = new playersave();
        psave.load();

        if(checkpoints.Length < 1)
        {
            Debug.Log("Checkpoints ERROR: Minimum 1 checkpoint needs to be set");
        }

        //Loop though all the defined checkpoint objects and add a BoxCollider2D with isTrigger set
        foreach (GameObject obj in checkpoints)
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

        if (ResetCheckpointOnStart)
        {
            psave.resetdata();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        levelTime = 0f;
        timeText.text = "Elapsed Time:\t" + levelTime;
    }

    //Checks if the player touched a checkpoint
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if(Collider.gameObject == FinishCheckpoint)
        {
            Debug.Log("Player Finished Level");
            finished = true;
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
                //Debug.Log("Hit Checkpoint: " + find_checkpoint_index(obj));
                return;
            }
        }        
    }


    void Update()
    {
        //if the player hasn't reached the finish, update the timer; else set it to green
        if (!finished)
        {
            levelTime += Time.deltaTime;
            minutes = (int)levelTime / 60;
            if (levelTime > 60)
            {
                timeText.text = "Elapsed Time:\t" + minutes + ":" + TimerRounding(levelTime % 60);
            }
            else timeText.text = "Elapsed Time:\t" + TimerRounding(levelTime);
        }
        else timeText.color = new Vector4(0f, 1.0f, 0f, 1.0f);
    }

    //custom rounding function to display the time to 1 decimal place
    private float TimerRounding(float time)
    {
        return Mathf.Round(time * 10.0f) / 10.0f;
    }
}
