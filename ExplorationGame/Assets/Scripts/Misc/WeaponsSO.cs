using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon Stats")]
public class WeaponsSO : ScriptableObject {

    [Header("Weapon Name")]
    public string wepName;

    [Header("Weapon Variables")]
    public int wepDamage;
    public float wepBulletSpeed;
    public float wepFireRate;
    public float wepBulletSpread;
    public float wepRange;

    [Header("Weapon VFX")]
    public ParticleSystem wepTrailParticle;
    public ParticleSystem wepExplosionParticle;
}
