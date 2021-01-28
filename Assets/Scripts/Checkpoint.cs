using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public GameObject checkpoint1;
    /*public gameObject checkpoint2;
    public gameObject checkpoint3;
    public gameObject checkpoint4;
    public gameObject checkpoint5;

    public gameObject FinishCheckPoint;*/

    public float speed;
    private static string savefile = System.IO.Directory.GetCurrentDirectory() + "\\PlayerSave.json";
    private playersave psave;


    public class playersave
    {
        public int world = 0;
        public int level = 0;
        public int checkpoint = 0;

        public playersave()
        {

        }

        public void set(int world, int level, int checkpoint)
        {
            this.world = world;
            this.level = level;
            this.checkpoint = checkpoint;
        }
        public void load()
        {
            if (System.IO.File.Exists(savefile))
            {
                playersave data = JsonUtility.FromJson<playersave>(System.IO.File.ReadAllText(savefile));
                this.world = data.world;
                this.level = data.level;
                this.checkpoint = data.checkpoint;
            }
        }
        public void save()
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {

        psave = new playersave();
        psave.load();

        Debug.Log("Start");
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == checkpoint1)
        {
            Debug.Log("Collison Checkpoint 1");
            psave.checkpoint++;
        }

        Debug.Log("Checkpoint Data: " + psave.checkpoint);
    }

    void FixedUpdate()
    {

    }
}
