using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire_Shot_Shell : MonoBehaviour
{
    Rigidbody Shell;
    public float speed=30;
    public Vector3 targetPosition;
    Vector3 StartPoint;
    bool Wire_Landing;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        StartPoint=this.transform.position;
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position= Vector3.MoveTowards(transform.position, targetPosition, speed*Time.deltaTime);
        

        if (transform.position==targetPosition)
        {
            Player.GetComponent<Player>().Wire_Go=true;
        }
        else
        {
            Player.GetComponent<Player>().Wire_Go=false;
        }
    
        


    }
}
