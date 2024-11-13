using UnityEngine;

public class ObstacleDestroyer: MonoBehaviour
{
    public void AddParticles()
    {
        Instantiate(Manager.blastParticle, this.transform);
    }
    public void Hide()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }
    public void Suicide()
    {
        Manager.obstaclesAccounter.RemoveObst(this.transform);
        Manager.UnRegisterObst(this);
        Destroy(this.gameObject);
    }
}
