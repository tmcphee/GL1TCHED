using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrabObject : MonoBehaviour
{
    public Rigidbody2D player;
    public Camera cam;

    private Vector3 m;
    private Vector3 cameraPos;
    private Rigidbody2D box;

    // Start is called before the first frame update
    void Start()
    {
        cameraPos = cam.GetComponent<Transform>().position;
        cameraPos[2] = 0;
        box = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, 0f);
        float m_Angle = Vector3.Angle(new Vector3(0, 0, 0), relativeMousePosition(playerPos, cameraPos));
        Debug.Log((m_Angle, player.position, playerPos - relativeMousePosition(playerPos, cameraPos)));
    }

    private Vector3 relativeMousePosition(Vector3 playerPos, Vector3 cameraPos)
    {
        m = cameraPos - Input.mousePosition;
        m = new Vector3(playerPos.x - (m[0] * 16 / Screen.width), playerPos.y - (m[1] * 9 / Screen.height), 0);
        
        Vector3 relativeMousePos = playerPos - (cameraPos - m);
        relativeMousePos[0] -= 0.4f;
        relativeMousePos[1] -= 0.6f;
        return relativeMousePos;
    }
}
