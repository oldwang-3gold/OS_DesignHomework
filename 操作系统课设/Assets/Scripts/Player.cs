using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    public float moveSpeed=3f;
    public float rotateSpeed = 3f;
	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {   
        float v = Input.GetAxis("Vertical");
     //   float speed = v * Time.fixedDeltaTime * moveSpeed;
     //   Debug.Log(speed);
        anim.SetFloat("Speed",v);
        transform.Translate(Vector3.forward*v*Time.fixedDeltaTime*moveSpeed);
        float h = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up*h*Time.fixedDeltaTime*rotateSpeed,Space.Self);
    }
}
