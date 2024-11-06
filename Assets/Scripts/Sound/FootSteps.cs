using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FootSteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] footSteps;

    private AudioSource audioSource;
    private Rigidbody rb;

    private float stepTime;
    public float stepRate;
    public float footstepThreshold;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        OnFootSteps();
    }

    private void OnFootSteps()
    {
        if(CharacterManager.Instance.player.controller.IsGround())
        {
            if(rb.velocity.magnitude > footstepThreshold)
            {
                if((Time.time - stepTime) > stepRate)
                {
                    stepTime = Time.time;
                    audioSource.PlayOneShot(footSteps[Random.Range(0, footSteps.Length)]);
                }
            }
        }
    }
}
