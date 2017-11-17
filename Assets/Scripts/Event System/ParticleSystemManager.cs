using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ParticleSystemManager : MonoBehaviour
{
    private ParticleSystem[] particles;
    public ParticleSystem[] footstepGround;
    public ParticleSystem[] footstepLeaves;
    int random;

    private UnityAction<Vector3> eventPlayerFootstepListener;

    private float rayOriginOffset = 1f;
    private float rayDepth = 5f;
    private float totalRayLen;
    private Ray ray;
    private RaycastHit hit;
    private GameObject collidedObject;

    void Awake()
    {
        eventPlayerFootstepListener = new UnityAction<Vector3>(FootstepEventHandler);
    }

    // Use this for initialization
    void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();

        totalRayLen = rayOriginOffset + rayDepth;
    }

    void OnEnable()
    {
        //EventManager.StartListening<EventPlayerFootstep, Vector3, string>(eventPlayerFootstepListener);
    }

    void OnDisable()
    {
        //EventManager.StopListening<EventPlayerFootstep, Vector3, string>(eventPlayerFootstepListener);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ParticleSystem particle in particles)
        {
            if (particle.transform.position != Vector3.zero && !particle.isPlaying)
            {
                particle.transform.position = Vector3.zero;
            }
        }
    }

    ParticleSystem GetAvailableParticles(Vector3 footPos)
    {
        ray = new Ray(footPos + Vector3.up * rayOriginOffset, Vector3.down);
        ParticleSystem[] selectedParticles = null;

        if (Physics.Raycast(ray, out hit, totalRayLen))
        {
            collidedObject = hit.collider.gameObject;

            if (collidedObject.CompareTag("Forest"))
            {
                selectedParticles = footstepLeaves;
            }
            if (collidedObject.CompareTag("Ground"))
            {
                selectedParticles = footstepGround;
            }

            if (selectedParticles == null)
            {
                return null;
            }

            foreach (ParticleSystem particle in selectedParticles)
            {
                if (particle.transform.position == Vector3.zero)
                {
                    return particle;
                }
            }
        }
        return null;
    }

    void FootstepEventHandler(Vector3 footPos)
    {
        ParticleSystem particle = GetAvailableParticles(footPos);

        if (particle == null)
        {
            return;
        }

        particle.transform.position = footPos;
        particle.Play();
    }
}
