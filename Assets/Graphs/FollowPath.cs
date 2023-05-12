using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPath : MonoBehaviour
{
    public Transform goal;
    public float speed = 5.0f;
    public float accuracy = 1.0f;
    public float rotSpeed = 2.0f;
    public GameObject wpManager;
    public GameObject[] wps;
    public GameObject currentNode;
    public int currentWP = 0;
    public Graph g;

    private NavMeshAgent _agent;
    private Ray _ray;
    private RaycastHit _hit;
    private Camera _camera;
    private static readonly int ground = 1 << 6;

    void Start()
    {
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];
        _agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
    }
    public void GoToHeli()
    {
        g.AStar(currentNode, wps[1]);
        currentWP = 0;
    }
    public void GoToRuin()
    {
        g.AStar(currentNode, wps[6]);
        currentWP = 0;
    }
    public void GoToFab()
    {
        g.AStar(currentNode, wps[9]);
        currentWP = 0;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 1000f, ground))
            {
                _agent.destination = _hit.point;
            }
        }
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;
        //O nó que estará mais próximo neste momento
        currentNode = g.getPathPoint(currentWP);
        //se estivermos mais próximo bastante do nó o tanque se moverá para o próximo
        if (Vector3.Distance(
        g.getPathPoint(currentWP).transform.position,
        transform.position) < accuracy)
        {
            currentWP++;
        }
        if (currentWP < g.getPathLength())
        {
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x,
            this.transform.position.y,
            goal.position.z );
            Vector3 direction = lookAtGoal - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * rotSpeed);

            transform.position = Vector3.MoveTowards(transform.position, goal.position, speed * Time.deltaTime);
        }
    }
}
