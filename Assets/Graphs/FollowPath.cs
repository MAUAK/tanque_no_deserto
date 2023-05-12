using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    //Declarando as variáveis e setando os valores delas
    public Transform goal;
    public float speed = 5.0f;
    public float accuracy = 1.0f;
    public float rotSpeed = 2.0f;
    public GameObject wpManager;
    public GameObject[] wps;
    public GameObject currentNode;
    public int currentWP = 0;
    public Graph g;

    void Start()
    {
        //Pegando os componentes para as veriáveis
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        currentNode = wps[0];
    }
    //métodos para ir até determinados pontos
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
        //Se o ponto atual for igual aos pontos que faltam, retorna
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;
        currentNode = g.getPathPoint(currentWP);
        if (Vector3.Distance(
        g.getPathPoint(currentWP).transform.position,
        transform.position) < accuracy)
        {
            currentWP++;
        }
        //Se for menos que a quantidade de pontos, o tanque vira e vai para o node
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
