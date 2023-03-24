using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour
{
    [SerializeField] Material unlitMat;
    [SerializeField] Material litMat;
    private Rigidbody rb;
    private CapsuleCollider col;
    private ParticleSystem particles;
    private MeshRenderer headRenderer;
    private Light lightSource;
    private bool isGrounded = false;
    private bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        headRenderer = transform.Find("Head").GetComponent<MeshRenderer>();
        col = transform.Find("Body").GetComponent<CapsuleCollider>();
        lightSource = transform.Find("Light").GetComponent<Light>();
        particles = transform.Find("Particles").GetComponent<ParticleSystem>();
        LeanTween.delayedCall(0.5f, e => {
            if (!isGrounded)
                col.enabled = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (hasExploded)
            VaryLightSource();
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGrounded && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Ground();
            if (!hasExploded)
                Detonate();
        }
    }

    private void Ground()
    {
        rb.isKinematic = true;
        col.enabled = false;
        isGrounded = true;
    }

    private void VaryLightSource()
    {
        lightSource.intensity = 3 * Mathf.Sin(5 * Mathf.Cos(Time.time / 2)) + 10;
    }

    public void Detonate()
    {
        tag = "Untagged";
        headRenderer.material = litMat;
        lightSource.enabled = true;
        particles.Play();
        hasExploded = true;
    }
}
