using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/* NOTE:
 * In order for this script to work properly, the bottom left of the camera's view should be near (0, 0, 0) in the level's worldspace */
public class GrabObject : MonoBehaviour
{
    public float GrabDistance;
    public bool GlitchMode;
    public bool GrabEnemies;
    public bool PreserveMomentum;

    private Rigidbody2D player;
    private Camera cam;
    private Vector3 m;
    private Vector3 old_m;
    private Vector3 deltaV;
    private Vector3 cameraPos;
    private Vector3 originalPos;
    private Rigidbody2D box;
    private float m_Angle;
    private float boxDistance;
    private bool GrabToggle;
    private GameObject[] Enemies;


    // instantiates some variables like the camera position and this box object
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraPos = cam.GetComponent<Transform>().position;
        cameraPos[2] = 0;

        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        if (GrabEnemies)
        {
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }

        box = GetComponent<Rigidbody2D>();
        originalPos = box.position;
        GrabToggle = false;
        m = Vector3.zero;
        deltaV = Vector3.zero;
    }
    

    /* Andrew Greer 
     *  - for every frame, calculates the mouse position in worldspace coordinates
     *  - if the player clicks on the box, it grabs the box using one of 2 different modes
     *      - normal mode attaches the box to the cursor
     *      - glitchmode snaps the box to a fixed radius circle around the player based on mouse angle (incredibly glitchy, but in a good way)
     *  - respawns the box if it falls below the map
     *  - optional mode for grabbing an enemy as well as the box
     */
    void Update()
    {
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, 0f);

        // saves the old mouse position before updating to calculate an instaneous "mouse velocity vector"
        old_m = m;
        m = ScreenSpaceToWorldSpace(Input.mousePosition);

        // uses the previous mouse position for imparting 'momentum vector' on objects when thrown
        if (PreserveMomentum)
        {
            deltaV = m - old_m;
        }

        boxDistance = Vector2.Distance(new Vector2(playerPos[0], playerPos[1]), box.position);

        if (Input.GetKey(KeyCode.E))
        {
            GrabToggle = !GrabToggle;
        }

        /* Andrew Greer
         *  - if grab enemies is enabled for the level and the array of all enemies is not empty, binds the enemy to the mouse position
         *      (very similar to grabbing a box except there's no check if the player is close to the enemy)
         */
        if(GrabEnemies && Enemies != null)
        {
            foreach(GameObject enemy in Enemies)
            {
                if((Input.GetMouseButton(0) || GrabToggle) && Vector2.Distance(enemy.transform.position, m) < GrabDistance)
                {
                    Rigidbody2D e = enemy.GetComponent<Rigidbody2D>();
                    enemy.transform.position = new Vector2(m[0], m[1]);
                    e.velocity = (deltaV * 3f) + (deltaV  * e.mass * Time.deltaTime * 25f);
                    Debug.Log(deltaV);
                }
            }
        }


        // if player clicks and the box is close to the player
        if ((Input.GetMouseButton(0) || GrabToggle) && boxDistance < GrabDistance * 2f)
        {
            if (GlitchMode)
            {
                m_Angle = DegreesToRadians(Vector2.Angle(playerPos, m - playerPos));

                if(m.y > playerPos.y)
                {
                    m_Angle *= -1f;
                }

                Debug.Log(m_Angle);
                m_Angle = Mathf.Abs(m_Angle - DegreesToRadians(180));
                box.position = new Vector2(playerPos[0] - (GrabDistance * Mathf.Cos(m_Angle)), playerPos[1] - (GrabDistance * Mathf.Sin(m_Angle)));

            } else if (Vector2.Distance(box.position, new Vector2(m[0], m[1])) < GrabDistance) { box.position = new Vector2(m[0], m[1]); }
            // ^ if box is close to the cursor


            // apply momentum vector
            box.velocity = (deltaV * 3f) + (deltaV * box.mass * Time.deltaTime * 25f);
        }

        // if box falls below the map
        if(box.position.y < -10f)
        {
            ResetPosition();
        }
    }


    /* Andrew Greer
     * - takes a Vector3 of screenspace coordinates (pixel position) and converts them to worldspace coordinates */
    private Vector3 ScreenSpaceToWorldSpace(Vector3 coordinates)
    {
        //multiplying x coord by 3.5554 and y coordinate by 2 since the ratio is approx. 16:9
        return new Vector3((coordinates[0] / Screen.width) * cam.orthographicSize * 3.5554f, (coordinates[1] / Screen.height) * cam.orthographicSize * 2, 0);
    }


    /* Andrew Greer
     *  - simple degrees to radians conversion method */
    private float DegreesToRadians(float angle) { return angle * (Mathf.PI / 180); }


    /* Andrew Greer
     *  - resets the box position to where it was when the level started (level restart) */
    public void ResetPosition() { 
        box.position = originalPos;
        box.velocity = new Vector2(0f, 0f);
    }
}
