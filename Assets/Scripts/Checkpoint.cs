using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using playersave = PlayerSave.playersave;

public class Checkpoint : MonoBehaviour
{
    public bool UseCheckpointGlitch = false;
    public bool ResetCheckpointOnStart = false;
    public bool CheckpointVelocityGlitch = false;
    public bool isEndofWorld = false;
    
    public GameObject[] checkpoints;
    public GameObject FinishCheckpoint; 
    public TMP_Text timeText;
    public TMP_Text BestTimeText;
    public AudioSource checkpointSound;

    private Rigidbody2D r;
    private static playersave psave;
    private float levelTime;
    private int minutes = 1;
    private bool finished = false;

    /*  Tyler McPhee
     *  Sets the Player checkpoint to the first checkpoint, checkpoint 0
     *  Respawns the player on the Last checkpoint, checkpoint 0
     */
    public void RestartLevel()
    {
        psave.setLevel(0);
        SetPlayerLastCheckpoint();
    }

    /*  Tyler McPhee
     *  Finds a given checkpoint object in the checkpoints array
     *  INPUT: GameObject checkpoint
     *  OUTPUT: Index value in checkpoints array
     */
    public int find_checkpoint_index(GameObject checkpoint)
    {
        int index = 0;
        foreach (GameObject obj in checkpoints)
        {
            if (obj == checkpoint)
            {
                Debug.Log(index);
                return index;
            }
            index++;
        }
        return -1;
    }

    /*  Tyler McPhee
     *  Gets the postiton of the last checkpoint and sets its value to the player
     *  Optional glitch of not resetting the players velocity
     */
    public void SetPlayerLastCheckpoint()
    {
        r.transform.position = GetLastCheckpointPosition();
        if (CheckpointVelocityGlitch == false)
        {
            r.velocity = new Vector3(0, 0, 0);
        }
        
    }

    /*  Tyler McPhee
     *  Gets the index of the last checkpoint the player passed though.
     *  Checks to see of the index is in the checkpoints array. If not use the closet checkpoint index
     *  OUTPUT: Vector3 -> Position of last checkpoint found in checkpoints array
     */
    public Vector3 GetLastCheckpointPosition()
    {
        //Make sure the last checkpoint is in range of array
        int lastcheckpoint = psave.getCheckpoint();
        if (lastcheckpoint >= checkpoints.Length)
        {
            lastcheckpoint = checkpoints.Length - 1;
        }
        return new Vector3(checkpoints[lastcheckpoint].transform.position.x, checkpoints[lastcheckpoint].transform.position.y, 0);
    }

    //On Script load
    void Awake()
    {
        psave = new playersave();
        psave.load();

        if(checkpoints.Length < 1)
        {
            Debug.Log("Checkpoints ERROR: Minimum 1 checkpoint needs to be set");
        }

        /*  Tyler McPhee
         *  Loop though all the checkpoint objects in the cehckpoints array and add a BoxCollider2D with isTrigger set
         */
        foreach (GameObject obj in checkpoints)
        {
            if (obj != null)
            {
                BoxCollider2D c = obj.AddComponent<BoxCollider2D>() as BoxCollider2D;
                c.isTrigger = true;
            }
        }

        /*  Tyler McPhee
         *  Adds a BoxCollider2D with isTrigger set to the finished checkpoint if exists
         */
        if (FinishCheckpoint != null)
        {
            BoxCollider2D fc = FinishCheckpoint.AddComponent<BoxCollider2D>() as BoxCollider2D;
            fc.isTrigger = true;
        }

        /*  Tyler McPhee
         *  Optional clear player data on start
         */
        if (ResetCheckpointOnStart)
        {
            psave.resetdata();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        r = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        levelTime = 0f;
        if (timeText != null)
        {
            timeText.text = "Elapsed Time:\t" + levelTime;
        }
        else
        {
            Debug.Log("timeText Missing");
        }
        if(BestTimeText != null)
        {
            BestTimeText.text = "Best Time:\t" + parseTime(psave.getBestTime());
        }
        
    }

    void OnTriggerEnter2D(Collider2D Collider)
    {
        /*  Tyler McPhee
         *  Checks to see if the player hit the finished checkpoint
         *  If so increments the level and resets the checkpoint data
         */
        if (Collider.gameObject == FinishCheckpoint)
        {
            Debug.Log("Player Finished Level");
            psave.saveBestTime(levelTime);
            finished = true;
            checkpointSound.Play();
            psave.setCheckpoint(0);

            //If its the end of the world levels increment the world and reset checkpoints and levels
            if (isEndofWorld)
            {
                psave.incrementWorld();
                psave.setLevel(0);

            }
            else
            {
                psave.incrementLevel();
            }
            
            SceneManager.LoadScene("PatchNotes");
            return;
        }

        /*  Tyler McPhee
         *  Loops though all checkpoints and see if the checkpont the player hit is in the checkpoints array
         *  If so either increments the checkpoint data
         */
        foreach (GameObject obj in checkpoints)
        {
            if (Collider.gameObject == obj)
            {
                int hit_checkpoint = find_checkpoint_index(obj);
                int last_checkpoint = psave.getCheckpoint();

                //If defined then increment checkpoint else only increment if checkpoint is the next
                if (UseCheckpointGlitch)
                {
                    psave.incrementCheckpoint();
                    checkpointSound.Play();
                }
                //Check if the checkpoint has already been passed though
                else if (hit_checkpoint > last_checkpoint)
                {
                    psave.setCheckpoint(hit_checkpoint);
                    checkpointSound.Play();
                }
                return;
            }
        }        
    }


    private string parseTime(float levelTime)
    {
        if(levelTime == -1)
        {
            return "NA";
        }
        string timeString = "0.0";
        minutes = (int)levelTime / 60;
        if (levelTime > 60)
        {
            timeString = minutes + ":" + TimerRounding(levelTime % 60);
        }
        else timeString = "" + TimerRounding(levelTime);
        return timeString;
    }


    void Update()
    {
        //if the player hasn't reached the finish, update the timer; else set it to green
        if (timeText != null)
        {
            if (!finished)
            {
                levelTime += Time.deltaTime;
                string timeString = parseTime(levelTime);
                timeText.text = "Elapsed Time:\t" + timeString;
            }
            else timeText.color = new Vector4(0f, 1.0f, 0f, 1.0f);
        }

        
    }

    //custom rounding function to display the time to 1 decimal place
    private float TimerRounding(float time)
    {
        return Mathf.Round(time * 10.0f) / 10.0f;
    }
}
