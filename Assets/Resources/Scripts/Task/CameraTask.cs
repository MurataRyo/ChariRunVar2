using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTask : MonoBehaviour
{
    [HideInInspector] public GameObject cameraObject;
    [HideInInspector] public float whideSize;           //横の距離
    [HideInInspector] public Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        cameraObject = Camera.main.gameObject;
        mainCamera = Camera.main;
        whideSize =
            mainCamera.orthographicSize * ((float)Screen.width / (float)Screen.height) * 2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //cameraObject.transform.position += new Vector3(3f * Time.fixedDeltaTime, 0f, 0f);
        cameraObject.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0f,0f,-10f);
    }
}
