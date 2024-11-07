using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGrowl : MonoBehaviour
{
    [SerializeField] private AudioClip[] growls;
    private AudioSource audioSource;
    public float targetVolume;
    public float maxVolume;
    public float fadeTime;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        targetVolume = 0;
        audioSource.volume = targetVolume;
    }

    private void Update()
    {
        if(!Mathf.Approximately(audioSource.volume, targetVolume))
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, (maxVolume/fadeTime) * Time.deltaTime);
        }

        if(!audioSource.isPlaying)
        audioSource.PlayOneShot (growls[Random.Range(0, growls.Length)]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = maxVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = 0.0f;
        }
    }


}
