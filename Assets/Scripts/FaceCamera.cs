using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Transform myCamera;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = Player.WORLD_PLAYER.transform.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(myCamera);
    }
}
