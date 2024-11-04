using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("Light")]
    private Light fireLight;
    private float minIntensity = 1f;
    private float maxIntensity = 2.5f;
    private float flickRate = 0.5f;
    private float lastFlicked;

    [Header("Damage")]
    [SerializeField] private float damage;
    private float damageRate = 0.5f;
    private float lastDamage;
    private List<IDamagable> tooCloseEntities;

    private void Start()
    {
        fireLight = GetComponent<Light>();
        tooCloseEntities = new List<IDamagable>();
    }

    private void Update()
    {
        LightIntensityChange();
        Damage();
    }

    private void LightIntensityChange()
    {
        if (Time.time - lastFlicked > flickRate)
        {
            lastFlicked = Time.time;
            fireLight.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }

    private void Damage()
    {
        if(Time.time - lastDamage > damageRate)
        {
            lastDamage = Time.time;
            foreach (var damagable in tooCloseEntities)
            {
                damagable.TakeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            tooCloseEntities.Add(damagable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<IDamagable>(out IDamagable dammagable))
        {
            tooCloseEntities.Remove(dammagable);
        }
    }
}
