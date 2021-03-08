using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/* NOTE:
 * In order for this script to work properly, the bottom left of the camera's view should be near (0, 0, 0) in the level's worldspace */
public class GrabObject : MonoBehaviour
{
    public Rigidbody2D player;
    public Camera cam;
    public float GrabDistance;
    public bool GlitchMode;

    private Vector3 m;
    private Vector3 cameraPos;
    private Vector3 originalPos;
    private Rigidbody2D box;
    private float m_Angle;
    private float boxDistance;
    private bool GrabToggle;


    // instantiates some variables like the camera position and this box object
    void Start()
    {
        cameraPos = cam.GetComponent<Transform>().position;
        cameraPos[2] = 0;
        box = GetComponent<Rigidbody2D>();
        originalPos = box.position;
        GrabToggle = false;
    }
    

    /* Andrew Greer 
     *  - for every frame, calculates the mouse position in worldspace coordinates
     *  - if the player clicks on the box, it grabs the box using one of 2 different modes
     *      - normal mode attaches the box to the cursor
     *      - glitchmode snaps the box to a fixed radius circle around the player based on mouse angle (incredibly glitchy, but in a good way)
     *  - respawns the box if it falls below the map
     */
    void Update()
    {
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, 0f);
        m = ScreenSpaceToWorldSpace(Input.mousePosition);
        boxDistance = Vector2.Distance(new Vector2(playerPos[0], playerPos[1]), box.position);

        if (Input.GetKey(KeyCode.E))
        {
            GrabToggle = !GrabToggle;
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

            Debug.Log((m, box.position));
        }

        // if box falls below the map
        if(box.position.y < -10f)
        {
            ResetPosition();
        }
    }


    /* Andrew Greer
     * - takes a Vector3 of screenspace coordinates (pixel position) and converts them to worldspace coordinates */
    Vector3 ScreenSpaceToWorldSpace(Vector3 coordinates)
    {
        return new Vector3((coordinates[0] / Screen.width) * cam.orthographicSize * 4 - GrabDistance, (coordinates[1] / Screen.height) * cam.orthographicSize * 2 - GrabDistance, 0);
    }


    /* Andrew Greer
     *  - simple degrees to radians conversion method */
    float DegreesToRadians(float angle) { return angle * (Mathf.PI / 180); }


    /* Andrew Greer
     *  - resets the box position to where it was when the level started (level restart) */
    public void ResetPosition() { 
        box.position = originalPos;
        box.velocity = new Vector2(0f, 0f);
    }
}
