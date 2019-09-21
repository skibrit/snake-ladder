using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [HideInInspector]
    public int moveForwardCount;
    [HideInInspector]
    public bool moveAllowed;
    [HideInInspector]
    public int totalWayPoint;
    public Transform[] wayPoints = new Transform[100];
    [HideInInspector]
    public int currentPathIndex;
    [HideInInspector]
    public int jumpIndex;
    Dictionary<int, int> specialPos = new Dictionary<int, int>();


    // Start is called before the first frame update
    void Start()
    {
        currentPathIndex = 0;
        moveAllowed = false;
        totalWayPoint = wayPoints.Length-1;
        transform.position = wayPoints[currentPathIndex].transform.position;
        jumpIndex = 0;

        specialPos.Add(2, 23);
        specialPos.Add(6, 45);
        specialPos.Add(20, 59);
        specialPos.Add(55, 96);
        specialPos.Add(52, 72);
        specialPos.Add(71, 92);
        specialPos.Add(43, 17);
        specialPos.Add(50, 5);
        specialPos.Add(56, 8);
        specialPos.Add(74, 15);
        specialPos.Add(87, 49);
        specialPos.Add(98, 40);
        specialPos.Add(84, 58);
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAllowed)
        {
            if (jumpIndex > 0)
            {
                jump();
            }
            else
            {
                move();
            }
        }
           
    }

    void move()
    {
      //  Debug.Log("Current Path Index " + currentPathIndex);
        if (currentPathIndex < totalWayPoint)
        {
            transform.position = Vector2.MoveTowards(transform.position,wayPoints[currentPathIndex+1].transform.position,
                moveSpeed*Time.deltaTime);
            if (transform.position == wayPoints[currentPathIndex + 1].transform.position)
            {
                currentPathIndex += 1;
                moveForwardCount -= 1;
            }

            if (moveForwardCount == 0 && specialPos.ContainsKey(currentPathIndex + 1))
            {
                jumpIndex = specialPos[currentPathIndex + 1] - 1;
            }
        }
    }

    public void jump()
    {
        Debug.Log("Special Index " + jumpIndex + "Time " + moveSpeed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position,
          wayPoints[jumpIndex].transform.position,
        Time.deltaTime*moveSpeed);

        if (transform.position == wayPoints[jumpIndex].transform.position)
        {
            currentPathIndex = jumpIndex;
            jumpIndex = 0;
        }
    }
}
