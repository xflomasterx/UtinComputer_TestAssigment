using UnityEngine;

public class VolumeAndLineManager : MonoBehaviour
{
    float volume;
    float diameter;
    [SerializeField]
    GameObject lineAim;
    [SerializeField]
    float bonusVolume=1.2f;

    Vector3 projectilespawnDir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        projectilespawnDir = (Manager.target.transform.position - this.transform.position).normalized;
    }
    public void SetVolume(float value)
    {
        volume = value* bonusVolume;
        RecalcSphere();
        RecalcLine();
    }
    public void ApllyBonusVolume()
    {
        volume = volume * bonusVolume;
        RecalcSphere();
        RecalcLine();
    }
    public float GetDiameter()
    {
        return diameter;
    }
    public float GetVolume()
    {
        return volume;
    }

    public void TryDecrVolume(float value)
    {
        if (volume > value)
        {
            volume -= value;
            RecalcSphere();
            RecalcLine();
        }
        else
            Manager.gameManager.Lose();

    }
    public void RecalcSphere()
    {
        diameter = SupportMath.SphereDiameter(volume);
        this.transform.localScale = new Vector3(diameter, diameter, diameter);
        this.transform.position = new Vector3(this.transform.position.x, diameter/2f, this.transform.position.z);
    }
    public Vector3 GetProjectileSpawnPoint(float projectileDiameter)
    {
        return this.transform.position + projectilespawnDir * (diameter + projectileDiameter) / 2f;
    }
    public void RecalcLine()
    {
        lineAim.transform.localScale = new Vector3(diameter, 1f,Vector3.Distance(this.transform.position, Manager.target.position))/10f;
        lineAim.transform.localPosition = new Vector3(0,0.01f, (this.transform.position.z + Manager.target.position.z)/2f);
    }
}
