using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrephab;
    [SerializeField]
    [Range(0f, 1f)]
    float volumeGainSpeed;
    [SerializeField]
    [Range(0f, 1f)]
    float minProjectileVolume;
    [SerializeField]
    [Range(0f, 1f)]
    float humanReactionTime;
    GameObject actualProjectile;
    float playerSphereVolume;//=SupportMath.SphereVolume(1f);
    float projectileVolume;
    float projectileDiameter;
    bool isVolumeGaining =false;

    public void Init()
    {
        Manager.volumeAndLineManager.Init();
        Manager.obstaclesAccounter.Init(this.transform.position.x, volumeGainSpeed*humanReactionTime);
        Manager.volumeAndLineManager.ApllyBonusVolume();
        SetPlayerSphereVolume(Manager.volumeAndLineManager.GetVolume());
        Manager.obstaclesAccounter.CheckObstInLine(Manager.volumeAndLineManager.GetDiameter());
    }
    public void SetPlayerSphereVolume(float volume)
    {
        playerSphereVolume = volume;
    }
    public void StartVolumeGain()
    {
        isVolumeGaining = true;
        actualProjectile = GameObject.Instantiate(projectilePrephab, this.transform.position, Quaternion.identity, this.transform.parent);
    }
    public void TryTransferVolume(float transferedVolume)
    {
        if (playerSphereVolume < transferedVolume)
        {
            Manager.gameManager.Lose();
            return;
        }
        projectileVolume += transferedVolume;
        projectileDiameter = SupportMath.SphereDiameter(projectileVolume);
        this.playerSphereVolume -= transferedVolume;
        Manager.volumeAndLineManager.TryDecrVolume(transferedVolume);
        actualProjectile.transform.position = Manager.volumeAndLineManager.GetProjectileSpawnPoint(projectileDiameter);
        actualProjectile.transform.localScale = new Vector3(projectileDiameter, projectileDiameter, projectileDiameter);
      
    }
    public void StopVolumeGain()
    {
        isVolumeGaining = false;
        projectileVolume = 0;
        projectileDiameter = 0;
        actualProjectile.GetComponent<Projectile>().Init();
        Manager.clickZone.SetActive(false);
        actualProjectile = null;
    }
    // Update is called once per frame
    void Update()
    {
       
        if(isVolumeGaining)
        {
            float transferredVolume = volumeGainSpeed * Time.deltaTime;
            TryTransferVolume(transferredVolume);
        }
    }
}
