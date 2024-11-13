using UnityEngine;
using System;
using System.Collections.Generic;

public class ObstaclesAccounter : MonoBehaviour
{

    List<Transform> allObstacles;
    List<Transform> potentialObstacles;
    float aimLineWidth;
    float searchCenterAxis;
    float minProjectileVolume;
    float obstacleWidth;
    private static int CompareObsteclesByZCoord(Transform a, Transform b)
    { return a.position.z == b.position.z ? 0 : (a.position.z > b.position.z ? -1 : 1); }   
    public void Init(float playerPositionCoordX, float volumeTransferSpeed)
    { 
        minProjectileVolume = volumeTransferSpeed;
        float volume = 0;
        searchCenterAxis = playerPositionCoordX;
        allObstacles = new List<Transform>();
        foreach(Transform child in this.transform)
        {
            allObstacles.Add(child);
        }
        obstacleWidth = allObstacles[0].GetComponent<CapsuleCollider>().radius*Mathf.Max(allObstacles[0].transform.lossyScale.x, allObstacles[0].transform.lossyScale.z)*2f;
        allObstacles.Sort(CompareObsteclesByZCoord);
        aimLineWidth = 0;
        int a = 0;
        foreach (Transform obst in FindNextObstacle())
        {
            a++;
            volume += minProjectileVolume;
            Manager.volumeAndLineManager.SetVolume(volume);
            aimLineWidth = Manager.volumeAndLineManager.GetDiameter();
        }
        Debug.Log("Calculated obst " + a);
        Debug.Log("Calculated volume per obst " + minProjectileVolume);
        Debug.Log("Calculated Volume " + Manager.volumeAndLineManager.GetVolume());
        Debug.Log("Calculated Diameter " + Manager.volumeAndLineManager.GetDiameter());
    }
    public bool CheckObstInLine(float width)
    {
        bool pathIsClear=true;
        if (potentialObstacles == null)
        {
            potentialObstacles = new List<Transform>();
            foreach (Transform obstacle in allObstacles)
            {
                if (Mathf.Abs(obstacle.position.x - searchCenterAxis) - obstacleWidth / 2f <= width / 2f)
                {
                    pathIsClear = false;
                    potentialObstacles.Add(obstacle);
                }
                else
                {
                    CapsuleCollider cc = obstacle.GetComponent<CapsuleCollider>();
                    Destroy(cc);
                }
            }
        }
        else
        {
            List<Transform> tmpObstacles = new List<Transform>();
            foreach (Transform obstacle in potentialObstacles)
            {
                if (Mathf.Abs(obstacle.position.x - searchCenterAxis) - obstacleWidth / 2f <= width / 2f)
                {
                    pathIsClear = false;
                    tmpObstacles.Add(obstacle);
                }
                else
                {
                    CapsuleCollider cc = obstacle.GetComponent<CapsuleCollider>();
                    Destroy(cc);
                }
            }
            potentialObstacles = tmpObstacles;
        }
            return pathIsClear;
    }
    public List<Transform> GetAllObstacles()
    { return allObstacles; }
    public float GetObstRadius()
    { return obstacleWidth; }
    public void RemoveObst(Transform obst)
    {
        allObstacles.Remove(obst);
        potentialObstacles.Remove(obst);
    }
    IEnumerable<Transform> FindNextObstacle()
    {
        foreach(Transform obstacle in allObstacles)
            if (Mathf.Abs(obstacle.position.x - searchCenterAxis) - obstacleWidth / 2f <= aimLineWidth )
                yield return obstacle;
    }
}
