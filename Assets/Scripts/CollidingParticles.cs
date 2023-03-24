using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingParticles : MonoBehaviour
{
    public bool active = true;
    private ParticleSystem particles;
    [SerializeField] float destroyDelay = 5f;
    [SerializeField] bool automaticallyDecay = false;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        particles.collision.AddPlane(Environment.INSTANCE.transform.Find("Terrain"));
        if (automaticallyDecay)
        {
            LeanTween.delayedCall(destroyDelay, SafeDelete);
            this.enabled = false;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            particles.Stop();
            LeanTween.delayedCall(destroyDelay, SafeDelete);
            this.enabled = false;
        }
    }

    void SafeDelete()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
