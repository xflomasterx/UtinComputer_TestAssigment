using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float infectionRadiusMult = 1f;
    [SerializeField]
    Material infectedMat;
    bool collided =false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        StartCoroutine(Move());
    } 
    IEnumerator Move()
    {
        while(!collided)
        {
            this.transform.position += new Vector3(0,0,speed*Time.deltaTime);
            yield return null;
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (!collided)
        {
       
            if (collider.tag == "Obstacle")
            {
                Debug.Log("collided obst");
                collided = true;
                Infect(this.transform.localScale.x / 2f * infectionRadiusMult, collider.ClosestPoint(new Vector3(this.transform.position.y, collider.transform.position.y, this.transform.position.z)));
                Destroy(this.gameObject);
            }
            else if (collider.tag == "Target")
            {
                collided = true;
                Manager.clickZone.SetActive(true);
                if (Manager.obstaclesAccounter.CheckObstInLine(Manager.volumeAndLineManager.GetDiameter()))
                    Manager.gameManager.Win();
                Destroy(this.gameObject);
            }
            else
                Debug.Log("collided " + collider.gameObject.name);
        }
    }
    void Infect(float infectionRadius,Vector3 impactPosition)
    {
        List<Transform> potentialObstacles= Manager.obstaclesAccounter.GetAllObstacles();
        float obstacleRadius = Manager.obstaclesAccounter.GetObstRadius();
        potentialObstacles = potentialObstacles.Where(o => (Mathf.Abs(o.position.x - impactPosition.x) - obstacleRadius) < infectionRadius && (Mathf.Abs(o.position.z - impactPosition.z) - obstacleRadius) < infectionRadius).ToList();
        foreach (Transform obst in potentialObstacles)
            if (Vector3.Distance(obst.position, impactPosition) < infectionRadius + obstacleRadius)
            {
                obst.GetComponent<MeshRenderer>().material = infectedMat;
                Manager.RegisterObst(obst.GetComponent<ObstacleDestroyer>());
                obst.GetComponent<Animator>().Play("Blast");
            }

    }

}
