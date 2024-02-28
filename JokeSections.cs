using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class JokeSections : MonoBehaviour
{
    #region Scriptable Objects
    [Header("Scriptable Objects")]
    public Joke[] jokes;
    public Joke selectedJoke;
    #endregion



    #region UI Elements
    [Header("UI Elements")]
    public TMP_Text setUpText;
    public TMP_Text darkEnd;
    public TMP_Text slapstickEnd;
    public TMP_Text deadpanEnd;
    public TMP_Text timer;
    public Slider slider;
    public GameObject panel;
    public GameObject deathUI;
    public GameObject passUI;
    #endregion


    #region Post Processing
    [Header("Post Processing")]

    public PostProcessVolume volume;
    public ChromaticAberration chromaticAberration = null;
    public Vignette vignette;
    #endregion

    #region Variables
    [Header("Numeric Variables")]

    float AudienceScore;
    float MaxAudienceScore = 100;
    int jokeNum;
    [SerializeField] int thisLevel;
    int levelCompleted;

    [SerializeField] float MaxJokeTime;
    float jokeTime;
    bool isRunning;
    #endregion

    #region PlayerReferences
    [Header("Player References")]
    public GameObject ragdoll;
    public GameObject player;
    public GameObject playerHead;
    public GameObject tomato;
    public Transform origin;
    #endregion


    #region Audio
    public AudioSource audioSource;
    public AudioClip crowdTalk;
    public AudioClip crowdBoo;
    public AudioClip crowdCheer;
    #endregion



    void Start()
    {
        volume.profile.TryGetSettings(out chromaticAberration);
        volume.profile.TryGetSettings(out vignette);
        selectedJoke = jokes[0];
        AudienceScore = 50;
        Invoke("initialize", 3.9f);
        jokeTime = MaxJokeTime;

        //This just sets everything up properly, sets the joke that we're telling to the first one, the audience score to be neutral, etc.
    }

    // Update is called once per frame
    void Update()
    {
        setUpText.text = selectedJoke.setUp;
        darkEnd.text = selectedJoke.darkPunchline;
        slapstickEnd.text = selectedJoke.slapstickPunchline;
        deadpanEnd.text = selectedJoke.deadpanPunchline;
        //This sets all the text in the UI to be correct for the new joke.


        slider.value = AudienceScore;
        

        if((jokeTime > 0) && (isRunning))
            jokeTime -= Time.deltaTime;
        
        timer.text = jokeTime.ToString("0");

        if(jokeTime < 0)
        {
            StartCoroutine(Death());
        }

        if(AudienceScore <= 0)
        {
            StartCoroutine(Death());
        }


        #region Post Processing Applied
        if (AudienceScore < 25)
        {
            chromaticAberration.intensity.value = 0.2f;
            vignette.intensity.value = 0.2f;
        }
        else
        {
            chromaticAberration.intensity.value = 0.1f;
            vignette.intensity.value = 0;
        }
        //This sets all the post processing effects!
        #endregion 
    }

    public void initialize()
    {
        panel.SetActive(false);
        isRunning = true;

    }

    public void DarkResponse()
    {
        if(selectedJoke.goodPunchline == "DarkPunchline")
        {
            AudienceScore += 25;
            audioSource.clip = crowdCheer;
            audioSource.Play();

            //If the player says the good punchline, they gain audience score and the audience laugh.
        }
        else if(selectedJoke.badPunchline == "DarkPunchline")
        {
            AudienceScore -= 25;
            audioSource.clip = crowdBoo;
            audioSource.Play();

            //If the player says the bad punchline, they lose audience score and get booed.
        }
        else
        {
            AudienceScore += 5;            //If this punchline isn't good or bad, the audience will slightly like you more.

        }

        NewJoke();
    }

    public void SlapstickResponse()
    {
        if (selectedJoke.goodPunchline == "SlapstickPunchline")
        {
            AudienceScore += 25;
            audioSource.clip = crowdCheer;
            audioSource.Play();

            //If the player says the good punchline, they gain audience score and the audience laugh.

        }
        else if (selectedJoke.badPunchline == "SlapstickPunchline")
        {
            AudienceScore -= 25;
            audioSource.clip = crowdBoo;
            audioSource.Play();

            //If the player says the bad punchline, they lose audience score and get booed.
        }
        else
        {
            AudienceScore += 5;            //If this punchline isn't good or bad, the audience will slightly like you more.

        }
        NewJoke();
    }

    public void DeadpanResponse()
    {
        if (selectedJoke.goodPunchline == "DeadpanPunchline")
        {
            AudienceScore += 25;
            audioSource.clip = crowdCheer;
            audioSource.Play();

            //If the player says the good punchline, they gain audience score and the audience laugh.

        }
        else if (selectedJoke.badPunchline == "DeadpanPunchline")
        {
            AudienceScore -= 25;
            audioSource.clip = crowdBoo;
            audioSource.Play();

            //If the player says the bad punchline, they lose audience score and get booed.

        }
        else
        {
            AudienceScore += 5;            //If this punchline isn't good or bad, the audience will slightly like you more.

        }

        NewJoke();
    }

    public void NewJoke()
    {
        if(jokeNum != jokes.Length - 1)
        {
            jokeNum++;
            selectedJoke = jokes[jokeNum];
            jokeTime = MaxJokeTime;
            //If there is another joke in this set.
        }
        else if(jokeNum == jokes.Length - 1)
        {
            //If there isn't any other jokes.
            if(AudienceScore > 80)
            {
                //If the audience like you.
                if(levelCompleted > thisLevel)
                {
                    //If you have already passed this level, it won't change your save.
                    passUI.SetActive(true);
                    isRunning = false;

                }
                else
                {
                    //If you haven't passed this level, it will update your save.
                    if(thisLevel == 1)
                    {
                        levelCompleted++;
                    }
                    levelCompleted++;
                    PlayerPrefs.SetInt("level", levelCompleted);
                    PlayerPrefs.Save();
                    Debug.Log("Game Saved!");
                    passUI.SetActive(true);
                    isRunning = false;


                }
            }
            else
            {
                deathUI.SetActive(true);
            }
        }
       
    }


    public void Dead()
    {
        ragdoll.SetActive(true);
        player.SetActive(false);
        //I have 2 versions of the character, one of which is a ragdoll, the other is animated. This disables the animated and enables the ragdoll.
    }

    IEnumerator Death()
    {
        GameObject projectile = Instantiate(tomato, origin);
        Vector3 direction = playerHead.transform.position - projectile.transform.position;
        direction.Normalize();
        projectile.GetComponent<Rigidbody>().AddForce(direction);
        //This throws tomatoes at the player.
        yield return new WaitForSeconds(1);
        Dead();
        //Toggles the ragdoll
        yield return new WaitForSeconds(5);
        deathUI.SetActive(true);
    }
}
