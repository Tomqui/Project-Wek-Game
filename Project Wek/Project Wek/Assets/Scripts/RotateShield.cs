using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShield : MonoBehaviour
{
    public float rotateSpeed = 0.0f;
    public Projectile p1;
    public Projectile p2;
    public Projectile p3;


    GameObject player;

    private void Start()
    {
        player = transform.parent.gameObject;
        transform.parent = null;

    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position;
        //transform.Rotate(new Vector3(0,0, transform.rotation.z + rotateSpeed * Time.fixedDeltaTime));
        transform.RotateAround(player.transform.position, Vector3.forward, rotateSpeed * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        p1.transform.eulerAngles = new Vector3(
            p1.transform.eulerAngles.x,
            p1.transform.eulerAngles.y,
            -gameObject.transform.rotation.z);
        p2.transform.eulerAngles = new Vector3(
            p1.transform.eulerAngles.x,
            p1.transform.eulerAngles.y,
            -gameObject.transform.rotation.z);
        p3.transform.eulerAngles = new Vector3(
            p1.transform.eulerAngles.x,
            p1.transform.eulerAngles.y,
            -gameObject.transform.rotation.z);
        /*
        p4.transform.eulerAngles = new Vector3(
            p1.transform.eulerAngles.x,
            p1.transform.eulerAngles.y,
            -gameObject.transform.rotation.z);*/
    }

}
