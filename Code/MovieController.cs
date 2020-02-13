using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

[RequireComponent(typeof(VideoPlayer))]
public class MovieController : MonoBehaviour {

    private VideoPlayer player;
    // Use this for initialization

    //public int videoIndex;
    public int currentIndex = 0;
    public List<UnityEvent> eventsAfterEveryClip;

    public string currentClipName;

    void Start() {
        player = GetComponent<VideoPlayer>();
        player.loopPointReached += CheckOver;
    }


    public void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        if (string.Equals(currentClipName, vp.clip.name, System.StringComparison.CurrentCultureIgnoreCase))
            return;

        currentClipName = vp.clip.name;

        eventsAfterEveryClip[currentIndex].Invoke();
        currentIndex++;

    }
    public void OnDestroy()
    {

        player.loopPointReached -= CheckOver;
    }


   

    public VideoClip Clip
    {
        get
        {
            return player.clip;
        }
        set
        {
           // player.loopPointReached += CheckOver;
            player.clip = value;
        }
    }

    public void PauseVideo()
    {
        if(player.isPlaying)
        {
            Debug.Log("Pause");
            player.Pause();
        }
    }

    public void PlayVideo()
    {
        if(!player.isPlaying)
        {
            Debug.Log("Play");
            player.Play();
        }
    }

    public void BackVideo()
    {
        Debug.Log("Back");
        player.time -= 10;
    }

    public void ForwardVideo()
    {
        Debug.Log("Forward");
        player.time += 10;
    }

    public void ReplayVideo()
    {
        player.gameObject.SetActive(true);
        player.Stop();
        player.Play();
    }
}
