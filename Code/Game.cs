//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Video;
//using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;
//using FrameSynthesis.VR;

//[RequireComponent(typeof(AudioSource))]
//public class Game : MonoBehaviour {

//    public static Game instance = null;

//    private AudioSource audioSource;
//    private bool nodIsDetected = false;
//    private bool isTreeReady = false;

//    private int previousSceneIndex = -1;
//    private Vector3 cameraStartPosition;

//    [Header("Camerarig")]
//    public GameObject cameraRig;

//    [Header("VideoClips Scene 1")]
//    public VideoClip welcomeVideo1;
//    public VideoClip welcomeVideo2;
//    public VideoClip welcomeVideo3;
//    public VideoClip mindfulnessVideo;
//    public VideoClip reviewVideo;

//    [Header("AudioClip Scene 2")]
//    public AudioClip exerciseAudioClip;

//    public enum State
//    {
//        LISTENING,
//        INPUT
//    }

//    [Header("States")]
//    public State state;

//    public enum ActionState
//    {
//        FIRE_GAZE,
//        TV_GAZE,
//        NOD,
//        PLAY_GAZE,
//        SKY_GAZE,
//        END
//    }

//    public ActionState actionState;

//    [Header("References")]
//    public GameObject firePlaceUI;
//    public GameObject tvUI;
//    public GameObject playUI;
//    public Button playButton;
//    public MovieController movieController;
//    public GameObject videoUI;
//    public GameObject movieControlUI;
//    private VideoPlayer videoPlayer;
//    private LoadingCircleController loadingCirclecontroller;

//    private GameObject treeCanvas;

//    public bool fallingLeavesEnabled = false;
//    private bool inGarden = false;
//    private float activateTreeTimer = 35.0f;

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//        }
//        else if (instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        DontDestroyOnLoad(gameObject);
//        //DontDestroyOnLoad(cameraRig);

//        Debug.Log("Adding OnSceneLoaded");
//        SceneManager.sceneLoaded += OnSceneLoaded;
//        audioSource = GetComponent<AudioSource>();
//        videoPlayer = movieController.GetComponent<VideoPlayer>();
//    }

//    // Use this for initialization
//    void Start () {
//        playUI.SetActive(false);
//        videoPlayer.loopPointReached += EndOfVideoReached;
//        videoPlayer.SetTargetAudioSource(0, audioSource);
//    }
	
//	// Update is called once per frame
//	void Update () {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            Debug.Log("Pressed space");
//        }
//        if (Input.GetKeyDown(KeyCode.S))
//        {
//            StopAllCoroutines();
//            SkipIntro();
//        }
		
//        if (inGarden)
//        {
//            activateTreeTimer -= Time.deltaTime;
//            if (activateTreeTimer <= 0)
//            {
//                fallingLeavesEnabled = true;
//            }
//        }
//	}

//    public void SwitchState( State newState)
//    {
//        Debug.Log("Switching to new State: " + newState.ToString());
//        state = newState;
//        if (newState == State.INPUT)
//        {
//            if(loadingCirclecontroller.isGazing)
//            {
//                loadingCirclecontroller.ActivateLoad();
//            }
//        }
//    }

//    IEnumerator SwitchStateAfterTime(float delay, State newState)
//    {
//        yield return new WaitForSeconds(delay);
//        SwitchState(newState);
//    }

//    private void SwitchStateAfterAudioClip(float startDelay, State newState)
//    {
//        float audioLength = audioSource.clip.length + startDelay;
//        StartCoroutine(SwitchStateAfterTime(audioLength, newState));
//    }

//    public void SwitchActionState( ActionState newActionState)
//    {
//        Debug.Log("Switching to new ActionState: " + newActionState);
//        actionState = newActionState;
//        StartActionState(actionState);
//    }

//    IEnumerator SwitchActionStateAfterTime(float delay, ActionState newActionState)
//    {
//        yield return new WaitForSeconds(delay);
//        SwitchActionState(newActionState);
//    }

//    private void StartActionState(ActionState newActionState)
//    {
//        float waitBeforeAudio = 0f;
//        if (videoPlayer != null)
//        {
//            videoPlayer.gameObject.SetActive(true);
//        }
//        switch(newActionState)
//        {
//            case ActionState.FIRE_GAZE:
//                Debug.Log("Fire gaze phase started");
//                videoPlayer.clip = welcomeVideo1;

//                waitBeforeAudio = 5.0f;
//                Invoke("PlayVideo", waitBeforeAudio);

//                firePlaceUI.SetActive(true);
//                break;
//            case ActionState.TV_GAZE:
//                Debug.Log("TV gaze phase started");
//                videoPlayer.clip = welcomeVideo2;

//                PlayVideo();

//                tvUI.SetActive(true);
//                break;
//            case ActionState.NOD:
//                Debug.Log("Nod phase started");
//                videoPlayer.clip = welcomeVideo3;

//                PlayVideo();

//                VRGestureRecognizer.Current.NodHandler += OnNod;
//                break;
//            case ActionState.PLAY_GAZE:
//                Debug.Log("Mindfulness intro started");
//                videoPlayer.clip = mindfulnessVideo;

//                PlayVideo();

//                playUI.SetActive(true);
//                break;
//            case ActionState.SKY_GAZE:
//                Debug.Log("Skye gaze phase started");
//                audioSource.clip = exerciseAudioClip;

//                waitBeforeAudio = 5.0f;
//                Invoke("PlayAudio", waitBeforeAudio);

//                treeCanvas.transform.Find("TreeButton").gameObject.SetActive(true);
//                SwitchStateAfterAudioClip(waitBeforeAudio, State.INPUT);
//                break;
//            case ActionState.END:
//                Debug.Log("Ending phase started");
//                videoPlayer.clip = reviewVideo;

//                waitBeforeAudio = 5.0f;
//                Invoke("PlayVideo", waitBeforeAudio);

//                playUI.SetActive(true);
//                break;
//        }
//    }

//    private void EndOfVideoReached(VideoPlayer vp)
//    {
//        Debug.Log("End reached");
//        SwitchState(State.INPUT);
//        videoPlayer.gameObject.SetActive(false);
//    }

//    private void PlayAudio()
//    {
//        Debug.Log("Play audio");
//        audioSource.Play();
//    }

//    private void OnNod()
//    {
//        if (actionState == ActionState.NOD && state == State.INPUT)
//        {
//            SwitchState(State.LISTENING);
//            Debug.Log("Nod Detected");
//            nodIsDetected = true;
//            VRGestureRecognizer.Current.NodHandler -= OnNod;
//            Debug.Log("Ready to switch state");
//            SwitchActionState(ActionState.PLAY_GAZE);
//        }
//    }

//    public void ActivateFirePlace()
//    {
//        Debug.Log("Click fireplace!!!");
//        if(state == State.INPUT)
//        {
//            SwitchState(State.LISTENING);
//            firePlaceUI.SetActive(false);
//            SwitchActionState(ActionState.TV_GAZE);
//        }
//    }

//    public void ActivateTV()
//    {
//        if(state == State.INPUT)
//        {
//            SwitchState(State.LISTENING);
//            tvUI.SetActive(false);
//            movieControlUI.SetActive(true);
//            SwitchActionState(ActionState.NOD);
//        }
//    }

//    public void ActivateExercise()
//    {
//        if(state == State.INPUT)
//        {
//            playButton.GetComponent<Image>().color = new Color(0f, 255f, 0f);
//            SwitchState(State.LISTENING);
//            FadeToNextScene(1);
//        }
//    }

//    public void SkipIntro()
//    {
//        SwitchState(State.LISTENING);
//        FadeToNextScene(1);
//    }

//    public void ActivateSky()
//    {
//        if(state == State.INPUT)
//        {
//            SwitchState(State.LISTENING);
//            FadeToNextScene(0);
//        }
//    }

//    public void ReplayVideo()
//    {
//        movieController.ReplayVideo();
//    }

//    public void PlayVideo()
//    {
//        movieController.PlayVideo();
//    }

//    public void ActivateLoadCircleIfInput()
//    {
//        if(state == State.INPUT)
//        {
//            ActivateLoadCircle();
//        }
//    }

//    public void ActivateLoadCircle()
//    {
//        Debug.Log("Call loading circle start");
//        loadingCirclecontroller.ActivateLoad();
//    }

//    public void ActivateTreeLoadCircle(float _loadTime)
//    {
//        Debug.Log("Call loading circle start");
//        loadingCirclecontroller.ActivateTreeLoad(_loadTime);
//    }

//    public void DeactivateLoadCircle()
//    {
//        Debug.Log("Call loading circle stop");
//        loadingCirclecontroller.DeactivateLoad();
//    }

//    public void DeactivateLoadCircleIfInput()
//    {
//        if(state == State.INPUT)
//        {
//            Debug.Log("Call loading circle stop");
//            loadingCirclecontroller.DeactivateLoad();
//        }
//    }

//    private void FadeToNextScene(int nextSceneIndex)
//    {
//        OVRScreenFade fader = Camera.main.GetComponent<OVRScreenFade>();
//        float _fadeTime = fader.fadeTime;
//        fader.FadeOut();
//        StartCoroutine(LoadSceneAfterTime(_fadeTime, nextSceneIndex));
//    }

//    IEnumerator LoadSceneAfterTime(float delay, int sceneIndex)
//    {
//        yield return new WaitForSeconds(delay);
//        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
//        SceneManager.LoadScene(sceneIndex);
//    }

//    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//        loadingCirclecontroller = FindObjectOfType<LoadingCircleController>();
//        Debug.Log("Running OnSceneloaded");
//        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

//        if ( activeSceneIndex == 0)
//        {
//            inGarden = false;
//            if(previousSceneIndex == -1)
//            {
//                SwitchState(State.LISTENING);
//                SwitchActionState(ActionState.FIRE_GAZE);
//            }
//            else
//            {
//                movieController = FindObjectOfType<MovieController>();
//                videoPlayer = movieController.GetComponent<VideoPlayer>();
//                videoPlayer.loopPointReached += EndOfVideoReached;
//                videoPlayer.SetTargetAudioSource(0, audioSource);

//                playUI = GameObject.Find("PlayCanvas");
//                playButton = playUI.GetComponentInChildren<Button>();

//                SwitchState(State.LISTENING);
//                SwitchActionState(ActionState.END);

                
//            }
//        }
//        else if (activeSceneIndex == 1)
//        {
//            inGarden = true;
//            activateTreeTimer = 35.0f;
//            fallingLeavesEnabled = false;
//            treeCanvas = GameObject.Find("TreeCanvas");
//            SwitchState(State.LISTENING);
//            SwitchActionState(ActionState.SKY_GAZE);
            
//        }
//    }

//    public static bool ContainsSky(RaycastHit[] hits)
//    {
//        foreach (RaycastHit hit in hits)
//        {
//            if (hit.collider.name == "SkyCanvas")
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//}
