using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private AudioSource thoughts;
    private AudioSource music;
    private AudioSource dummySourcc;

	// Use this for initialization
	void Start () {
        PlayerRBMover playa = Component.FindObjectOfType<PlayerRBMover>();
        thoughts = playa.transform.Find("Main Camera").Find("thoughts").GetComponent<AudioSource>();
        music = playa.transform.Find("Main Camera").Find("music").GetComponent<AudioSource>();
        dummySourcc = new AudioSource();

        //keep me
        DontDestroyOnLoad(transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
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
}

