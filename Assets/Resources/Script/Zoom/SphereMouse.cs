using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMouse : MonoBehaviour
{
    private Vector3 pointposition;

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;
        pointposition.x = mouseX;
        pointposition.y = mouseY;
        pointposition.z  =  3f;
        this.transform.localPosition = pointposition;
    }
}
