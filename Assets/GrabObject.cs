using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrabObject : MonoBehaviour
{
    public Rigidbody2D player;
    public Camera cam;
    public float GrabDistance;
    public bool GlitchMode;

    private Vector3 m;
    private Vector3 cameraPos;
    private Rigidbody2D box;
    private float m_Angle;
    private float boxDistance;

    // instantiates some variables like the camera position and this box object
    void Start()
    {
        cameraPos = cam.GetComponent<Transform>().position;
        cameraPos[2] = 0;
        box = GetComponent<Rigidbody2D>();
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


        // if player clicks, the box is close to the player, and the box is close to the cursor
        if (Input.GetMouseButton(0) && boxDistance < GrabDistance * 2f && Vector2.Distance(box.position, new Vector2(m[0], m[1])) < GrabDistance * 2f)
        {
            if (GlitchMode)
            {
                m_Angle = Vector3.SignedAngle(playerPos, m - playerPos, Vector3.up);
                box.position = new Vector2(playerPos[0] + (GrabDistance * Mathf.Cos(m_Angle)), playerPos[1] + (GrabDistance * Mathf.Sin(m_Angle)));

            } else box.position = new Vector2(m[0], m[1]);
        }

        // if box falls below the map
        if(box.position.y < -10f)
        {
            box.position = new Vector3(7f, 2f, 0f);
        }

        //Debug.Log((m, playerPos, DegreesToRadians(m_Angle)));
    }


    /* Andrew Greer
     * - takes a Vector3 of screenspace coordinates (pixel position) and converts them to worldspace coordinates
     */
    Vector3 ScreenSpaceToWorldSpace(Vector3 coordinates)
    {
        return new Vector3((coordinates[0] / Screen.width) * cam.orthographicSize * 4, (coordinates[1] / Screen.height) * cam.orthographicSize * 2, 0);
    }


    /* Andrew Greer
     *  - simple degrees to radians conversion method
     */
    float DegreesToRadians(float angle) { return angle * (Mathf.PI / 180); }
}
