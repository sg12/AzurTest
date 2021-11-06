using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AnimalFactory;

public class Food : MonoBehaviour
{
    private ParticleSystem particle;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer.enabled = true;
        particle.Stop();
    }

    public void StartDestroy()
    {
        boxCollider.enabled = false;
        particle.Play();
        meshRenderer.enabled = false;
    }
}
