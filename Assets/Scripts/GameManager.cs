using UnityEngine;
using System.Collections.Generic;

public static class Manager
{
    public static GameManager gameManager;
    public static ObstaclesAccounter obstaclesAccounter;
    public static ProjectileSpawner projectileSpawner;
    public static Transform target;
    public static VolumeAndLineManager volumeAndLineManager;
    public static CameraAngleFitWithBounds cameraAngleFitWithBounds;
    public static PlayerMove playerMove;
    public static GameObject blastParticle;
    public static GameObject clickZone;
    public static GameObject loseScreen;
    public static GameObject winScreen;
    static List<ObstacleDestroyer> animatedObstacles;
    public static void RegisterObst(ObstacleDestroyer obst)
    {

        if (animatedObstacles == null)
            animatedObstacles = new List<ObstacleDestroyer>();
        animatedObstacles.Add(obst);
        Debug.Log("added obst " + animatedObstacles.Count);
    }
    public static void UnRegisterObst(ObstacleDestroyer obst)
    {
        animatedObstacles.Remove(obst);
        Debug.Log("left obst "+ animatedObstacles.Count);
        if (animatedObstacles.Count == 0)
        {
            clickZone.SetActive(true);
            if(obstaclesAccounter.CheckObstInLine(volumeAndLineManager.GetDiameter()))
                Manager.gameManager.Win();
        }
    }
}


public class GameManager : MonoBehaviour
{
    [SerializeField]
    ObstaclesAccounter obstAccounter;
    [SerializeField]
    ProjectileSpawner spawner;
    [SerializeField]
    Transform target;
    [SerializeField]
    VolumeAndLineManager volumeManager;
    [SerializeField]
    CameraAngleFitWithBounds cameraFitter;
    [SerializeField]
    GameObject blastParticle;
    [SerializeField]
    PlayerMove playerMove;
    [SerializeField]
    GameObject clickZone;
    [SerializeField]
    GameObject loseScreen;
    [SerializeField]
    GameObject winScreen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Manager.gameManager = this;
        Manager.obstaclesAccounter = obstAccounter;
        Manager.projectileSpawner = spawner;
        Manager.target = target;
        Manager.volumeAndLineManager = volumeManager;
        Manager.cameraAngleFitWithBounds = cameraFitter;
        Manager.playerMove = playerMove;
        Manager.blastParticle = blastParticle;
        Manager.clickZone = clickZone;
        Manager.loseScreen = loseScreen;
        Manager.winScreen = winScreen;

        Manager.cameraAngleFitWithBounds.Init();
        Manager.projectileSpawner.Init();
    }
    public void Win()
    {
        Manager.clickZone.SetActive(false);
        Manager.playerMove.StartMove();
    }
    public void Lose()
    {
        Manager.clickZone.SetActive(false);
        Manager.loseScreen.SetActive(true);
    }
    public void WinFinal()
    {
        Manager.winScreen.SetActive(true);
    }
}
