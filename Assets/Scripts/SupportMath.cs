using UnityEngine;

public static class SupportMath
{
    static public float SphereVolume(float diameter)
    {
        return Mathf.PI * Mathf.Pow(diameter, 3f) / 6f;
    }
    static public float SphereDiameter(float volume)
    {
        return  Mathf.Pow(6f* volume / Mathf.PI, 1f/3f);
    }
}
