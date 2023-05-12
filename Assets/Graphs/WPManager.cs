using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Link
{
    //Declarando as primeiras variáveis
    public enum direction { UNI, BI }
    public GameObject node1;
    public GameObject node2;
    public direction dir;
}

public class WPManager : MonoBehaviour
{
    //Declarando as variáveis de pontos
    public GameObject[] waypoints;
    public Link[] links;
    public Graph graph = new Graph();

    void Start()
    {
        //Se os pontos for mais que zero
        if (waypoints.Length > 0)
        {
            //Para cada ponto na cena, adiciona um node
            foreach (GameObject wp in waypoints)
            {
                graph.AddNode(wp);
            }
            //Adiciona um gráfico para cada ponto
            foreach (Link l in links)
            {
                graph.AddEdge(l.node1, l.node2);
                if (l.dir == Link.direction.BI)
                    graph.AddEdge(l.node2, l.node1);
            }
        }
    }

    void Update()
    {
        //Desenha o gráfico
        graph.debugDraw();
    }
}
