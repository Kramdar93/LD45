using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private AudioSource thoughts;
    private AudioSource music;
    private AudioSource feet;

    public AudioSource dummySourcc;

    public AudioClip[] tracks;

    private int trackNum = -1;
    private int currentTrack = -1;
    private bool nextLoop;

	// Use this for initialization
	void Start () {
        PlayerRBMover playa = Component.FindObjectOfType<PlayerRBMover>();
        thoughts = playa.transform.Find("Main Camera").Find("thoughts").GetComponent<AudioSource>();
        music = playa.transform.Find("Main Camera").Find("music").GetComponent<AudioSource>();
        feet = playa.transform.Find("Main Camera").Find("feet").GetComponent<AudioSource>();

        //keep me
        DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (currentTrack != trackNum && trackNum < tracks.Length-1)
        {
            music.loop = false;
            if (!music.isPlaying)
            {
                currentTrack = trackNum;
                music.clip = tracks[currentTrack];
                music.loop = nextLoop;
                music.Play();
            }
        }
        else if (!nextLoop && !music.isPlaying && currentTrack >= 0 && trackNum < tracks.Length - 1)
        {
            trackNum += 1;
            currentTrack += 1;
            music.clip = tracks[currentTrack];
            music.loop = nextLoop;
            music.Play();
        } 
	}

    public void PlayAt(AudioClip sfx, Vector3 position)
    {
        AudioSource source = Instantiate<AudioSource>(dummySourcc,null,true);
        source.transform.position = position;
        source.clip = sfx;
        source.Play();
    }

    public void PlayFrom(AudioClip sfx, AudioSource source, bool loop)
    {
        source.Stop();
        source.loop = loop;
        source.clip = sfx;
        source.Play();
    }

    public void PlayThought(AudioClip track)
    {
        thoughts.Stop();
        thoughts.loop = false;
        thoughts.clip = track;
        thoughts.Play();
    }
    
    public void PlayMusic(AudioClip track, bool loop)
    {
        music.Stop();
        music.clip = track;
        music.loop = loop;
        music.Play();
    }

    public void ToggleFootsteps(bool should)
    {
        if (should && !feet.isPlaying)
        {
            feet.Play();
        }
        else if(!should && feet.isPlaying)
        {
            feet.Stop();
        }
    }


    public void AdvanceTrack(bool shouldLoop)
    {
        trackNum += 1;
        music.loop = false;
        nextLoop = shouldLoop;
    }
}

