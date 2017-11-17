using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AudioEventManager : MonoBehaviour
{
    // List of audio sources
    private AudioSource[] audioSources;
    // Private variable for the current set of audio clips to play
    private AudioClip[] currentAudioClips;
    // List of Unity events
    private UnityAction<Vector3, string> eventListener;

    // Create a public class for grouping audio clips
    public AudioClipGroup[] audioClipGroups;

    [System.Serializable]
    public class AudioClipGroup
    {
        public string tag;
        public AudioClip[] audioClips;

        public AudioClipGroup(string tag, AudioClip[] audioClips)
        {
            this.tag = tag;
            this.audioClips = audioClips;
        }
    }

    void Awake()
    {
        eventListener = new UnityAction<Vector3, string>(GenericAudioEventHandler);
    }

    void Start()
    {
        audioSources = GetComponentsInChildren<AudioSource>();
    }

    void OnEnable()
    {
        EventManager.StartListening<EventPlayerFootstep, Vector3, string>(eventListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<EventPlayerFootstep, Vector3, string>(eventListener);
    }

    void Update()
    {
        foreach (AudioSource source in audioSources)
        {
            // If the source is done playing
            if (source.transform.position != Vector3.zero && !source.isPlaying)
            {
                // Reset the audio source location
                source.transform.position = Vector3.zero;
            }
        }
    }

    // Attempt to return an available audio source
    AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.transform.position == Vector3.zero)
            {
                return source;
            }
        }
        return null;
    }

    AudioClip[] GetAudioClips(string tag)
    {
        foreach (AudioClipGroup audioGroup in audioClipGroups)
        {
            if (tag.Equals(audioGroup.tag))
            {
                return audioGroup.audioClips;
            }
        }
        return null;
    }

    void GenericAudioEventHandler(Vector3 position, string tag)
    {
        // Get available audio source
        AudioSource source = GetAvailableAudioSource();

        if (source != null)
        {
            // Get pool of audio clips
            currentAudioClips = GetAudioClips(tag);

            if (currentAudioClips != null)
            {
                // Move the audio source to target position
                source.transform.position = position;
                // Get a random number from the audio clip array size
                int random = Random.Range(0, currentAudioClips.Length);
                // Play the audio
                source.PlayOneShot(currentAudioClips[random], 1f);
            }
        }
    }

    //void CollisionEventHandler(Vector3 worldPos, GameObject currentObject, Collision collision)
    //{
    //    // Get pool of audio clips
    //    currentAudioClips = GetCollisionAudioClips(currentObject);

    //    if (currentAudioClips == null)
    //    {
    //        return;
    //    }

    //    // Get a random number from array of clips
    //    int random = Random.Range(0, currentAudioClips.Length);

    //    // Get available audio source
    //    AudioSource source = getAvailableAudioSource();

    //    // Move the audio source to foot
    //    source.transform.position = worldPos;

    //    // Play the audio
    //    volume = collision.impulse.magnitude * 0.03f;
    //    source.pitch = 1 + (collision.impulse.magnitude * 0.0004f);
    //    source.PlayOneShot(currentAudioClips[random], volume);
    //}
}
