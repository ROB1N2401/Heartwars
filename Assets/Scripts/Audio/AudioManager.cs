using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    //How to use: find or make an approriate location in a script for a sound effect to be triggered.
    //After doing so, type in the following to play a sound: AudioManager.instance.PlaySound("Name of sound");
    //Or the following to play a song: AudioManager.instance.PlaySong("Name of sound");
    //"Name of sound" should of course be switched out with the name of the sound/song you want to be played. Not the file name, but the name given to it in the sounds array inside the AudioManager
    //object. It is important to distinguish between sounds and songs, as the music switching system is dependent on it.
    //To stop a sound/song, you instead write: AudioManager.instance.StopSound("Name of sound");
    //This is only useful for sounds that are played on a loop, as well as all songs.

    //If you want a sound effect to have a random pitch when it plays, you can use "AudioManager.instance.shouldRandomizePitch = true;" before calling the PlaySound function.
    //If you want to play a sound from inside this script instead of in another script, you can leave out the "AudioManager.instance." part of the code. Same thing goes for changing shouldRandomizePitch.

    //For fading between songs specifically, you want to use "AudioManager.instance.fadeIn("Name of song", (float value, duration in seconds))" or "fadeIn("Name of song", (float value, duration in
    //seconds))" if done within this script. Replace "fadeIn" with "fadeOut" when fading out, same arguments are taken in.
    //"PlaySong()" and "StopSound()" can still be used if no transition is desired.

    public Sound[] sounds;
    public static AudioManager instance;
    AudioSource[] allMyAudioSources;
    AudioSource audioSource;
    private string whatSongIsGonnaPlayNow;
    private bool timeToSwitchSong;
    private bool justEnteredScene;

    public string currentScene;
    private string sceneName;
    private string lastScene;
    private string currentSong;
    public bool shouldRandomizePitch;
    private bool firstSong = true; //Why this bool exists is explained in the StopSound function
    private bool firstTimeRunning; //This needs to exist as a separate bool
    private bool musicCanChange;
    private bool isPlayingTheGame;
    private int randomizeSounds;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mix;
        }

        allMyAudioSources = GetComponents<AudioSource>();

        musicCanChange = false;
        shouldRandomizePitch = false;
        isPlayingTheGame = false;
        firstTimeRunning = true;
        timeToSwitchSong = true;
        justEnteredScene = false;
    }

    public void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (sceneName == "MainMenu"/* || sceneName == "OtherMenuScene"*/) //Change these to reflect actual scenes later
        {
            this.currentScene = "Menu";
        }
        else if (sceneName == "TheMainScene"/* || sceneName == "OtherGameScene"*/) //This one as well
        {
            this.currentScene = "Heartwars";
        }
        else
        {
            this.currentScene = "Error";
        }


        if (lastScene != this.currentScene || lastScene == null)
        {
            musicCanChange = true;
        }

        if (firstTimeRunning)
        {
            PlaySong("PlaceIce1");
            StopSound("PlaceIce1");
            firstTimeRunning = false;
        }

        if (isPlayingTheGame == true && !firstTimeRunning)
        {
            if (!musicCanChange)
            {
                if (!timeToSwitchSong)
                {
                    if (audioSource == null)
                    {
                        audioSource = allMyAudioSources[29];
                        //Debug.Log("audioSource: " + audioSource.name);
                    }
                    if (!audioSource.isPlaying)
                    {
                        timeToSwitchSong = true;
                    }
                }
                else if (timeToSwitchSong)
                {
                    var randomizeSounds = Random.Range(0, 10);
                    switch (randomizeSounds)
                    {
                        case 0:
                            audioSource = allMyAudioSources[29];
                            //Debug.Log("audioSource: " + audioSource.name + "#1");
                            whatSongIsGonnaPlayNow = "Clash";
                            break;
                        case 1:
                            audioSource = allMyAudioSources[32];
                            //Debug.Log("audioSource: " + audioSource.name + "#2");
                            whatSongIsGonnaPlayNow = "StoneMagic";
                            break;
                        case 2:
                            audioSource = allMyAudioSources[33];
                            //Debug.Log("audioSource: " + audioSource.name + "#3");
                            whatSongIsGonnaPlayNow = "TheWatcher";
                            break;
                        case 3:
                            audioSource = allMyAudioSources[34];
                            //Debug.Log("audioSource: " + audioSource.name + "#4");
                            whatSongIsGonnaPlayNow = "VikingGodWisdom";
                            break;
                        case 4:
                            audioSource = allMyAudioSources[35];
                            //Debug.Log("audioSource: " + audioSource.name + "#5");
                            whatSongIsGonnaPlayNow = "VikingHillLegend";
                            break;
                        case 5:
                            audioSource = allMyAudioSources[36];
                            //Debug.Log("audioSource: " + audioSource.name + "#6");
                            whatSongIsGonnaPlayNow = "VikingLifeLesson";
                            break;
                        case 6:
                            audioSource = allMyAudioSources[37];
                            //Debug.Log("audioSource: " + audioSource.name + "#7");
                            whatSongIsGonnaPlayNow = "VikingMoonMelody";
                            break;
                        case 7:
                            audioSource = allMyAudioSources[38];
                            //Debug.Log("audioSource: " + audioSource.name + "#9");
                            whatSongIsGonnaPlayNow = "VikingShieldMaiden";
                            break;
                        case 8:
                            audioSource = allMyAudioSources[39];
                            //Debug.Log("audioSource: " + audioSource.name + "#9");
                            whatSongIsGonnaPlayNow = "VikingWolfHowls";
                            break;
                        case 9:
                            audioSource = allMyAudioSources[40];
                            //Debug.Log("audioSource: " + audioSource.name + "#10");
                            whatSongIsGonnaPlayNow = "WarriorsOfAvalon";
                            break;
                        case 10:
                            break;
                    }
                    if (justEnteredScene)
                    {
                        StartCoroutine(fadeIn(whatSongIsGonnaPlayNow, 1f));
                        justEnteredScene = false;
                    }
                    else
                    {
                        PlaySong(whatSongIsGonnaPlayNow);
                    }
                    //Debug.Log("Song '" + whatSongIsGonnaPlayNow + "' is currently playing");
                    timeToSwitchSong = false;
                }
            }
            else
            {
                isPlayingTheGame = false;
            }
        }

        if (musicCanChange && !firstTimeRunning)
        {
            if (this.currentScene == "Menu")
            {
                StartCoroutine(fadeOut(currentSong, 1f));
                StartCoroutine(fadeIn("LandsofMagic", 1f));
                musicCanChange = false;
            }
            else if (this.currentScene == "Heartwars")
            {
                StartCoroutine(fadeOut(currentSong, 1f));
                justEnteredScene = true;
                isPlayingTheGame = true;
                musicCanChange = false;
            }
            else if (this.currentScene == "Error")
            {
                musicCanChange = false;
            }
        }

        lastScene = this.currentScene;
    }

    IEnumerator fadeIn(string name, float fadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float startVolume = s.volume;

        s.source.volume = 0f;
        PlaySong(name);

        while (s.source.volume < startVolume)
        {
            s.source.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        s.source.volume = startVolume;
        currentSong = s.name;
    }

    IEnumerator fadeOut(string name, float fadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float startVolume = s.source.volume;

        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        StopSound(name);
        s.source.volume = startVolume;
    }

    public static void InvokeWalkingSound()
    {
        instance.shouldRandomizePitch = true;
        var randomizeSounds = Random.Range(0, 3);

        switch (randomizeSounds)
        {
            case 0:
                AudioManager.instance.PlaySound("MovePiece1");
                break;
            case 1:
                AudioManager.instance.PlaySound("MovePiece2");
                break;
            case 2:
                AudioManager.instance.PlaySound("MovePiece3");
                break;
            case 3:
                AudioManager.instance.PlaySound("MovePiece4");
                break;
            default:
                AudioManager.instance.PlaySound("MovePiece5");
                break;
        }
    }

    public static void InvokeDeathSound()
    {
        instance.shouldRandomizePitch = true;
        var randomizeSounds = Random.Range(0, 3);

        switch (randomizeSounds)
        {
            case 0:
                AudioManager.instance.PlaySound("Falling1");
                break;
            case 1:
                AudioManager.instance.PlaySound("Falling2");
                break;
            default:
                AudioManager.instance.PlaySound("Falling3");
                break;
        }
    }

    public static void InvokePlacementSound(ETileType type)
    {
        AudioManager.instance.shouldRandomizePitch = true;
        Debug.Log("Played sound " + type);
        switch (type)
        {
            case ETileType.Void:
                break;
            case ETileType.Floor:
                instance.randomizeSounds = Random.Range(0, 4);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("PlaceGround1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("PlaceGround2");
                        break;
                    case 2:
                        AudioManager.instance.PlaySound("PlaceGround3");
                        break;
                    case 3:
                        AudioManager.instance.PlaySound("PlaceGround4");
                        break;
                    case 4:
                        break;
                }
                break;
            case ETileType.Wall:
                instance.randomizeSounds = Random.Range(0, 2);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("PlaceStone1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("PlaceStone2");
                        break;
                    case 2:
                        break;
                }
                break;
            case ETileType.Ice:
                instance.randomizeSounds = Random.Range(0, 3);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("PlaceIce1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("PlaceIce2");
                        break;
                    case 2:
                        AudioManager.instance.PlaySound("PlaceIce3");
                        break;
                    case 3:
                        break;
                }
                break;
            case ETileType.Trampoline:
                instance.randomizeSounds = Random.Range(0, 3);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("PlaceTempoline1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("PlaceTempoline2");
                        break;
                    case 2:
                        AudioManager.instance.PlaySound("PlaceTempoline3");
                        break;
                    case 3:
                        break;
                }
                break;
            case ETileType.SideBlock:
                instance.randomizeSounds = Random.Range(0, 4);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("PlaceGround1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("PlaceGround2");
                        break;
                    case 2:
                        AudioManager.instance.PlaySound("PlaceGround3");
                        break;
                    case 3:
                        AudioManager.instance.PlaySound("PlaceGround4");
                        break;
                    case 4:
                        break;
                }
                break;
            case ETileType.Spawn:
                break;
            case ETileType.Bonus:
                break;
        }
    }

    public static void InvokeDestructionSound(ETileType type)
    {
        AudioManager.instance.shouldRandomizePitch = true;
        Debug.Log("Played sound " + type);
        switch (type)
        {
            case ETileType.Void:
                break;
            case ETileType.Floor:
                instance.randomizeSounds = Random.Range(0, 3);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("DestroyGround1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("DestroyGround2");
                        break;
                    case 2:
                        AudioManager.instance.PlaySound("DestroyGround3");
                        break;
                    case 3:
                        break;
                }
                break;
            case ETileType.Wall:
                instance.randomizeSounds = Random.Range(0, 2);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("DestroyStone1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("DestroyStone2");
                        break;
                    case 2:
                        break;
                }
                break;
            case ETileType.Ice:
                instance.randomizeSounds = Random.Range(0, 3);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("DestroyIce1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("DestroyIce2");
                        break;
                    case 2:
                        AudioManager.instance.PlaySound("DestroyIce3");
                        break;
                    case 3:
                        break;
                }
                break;
            case ETileType.Trampoline:
                instance.randomizeSounds = Random.Range(0, 2);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("DestroyTempoline1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("DestroyTempoline2");
                        break;
                    case 2:
                        break;
                }
                break;
            case ETileType.SideBlock:
                instance.randomizeSounds = Random.Range(0, 3);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("DestroyGround1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("DestroyGround2");
                        break;
                    case 2:
                        AudioManager.instance.PlaySound("DestroyGround3");
                        break;
                    case 3:
                        break;
                }
                break;
            case ETileType.Spawn:
                instance.randomizeSounds = Random.Range(0, 2);
                switch (instance.randomizeSounds)
                {
                    case 0:
                        AudioManager.instance.PlaySound("DestroyTempoline1");
                        break;
                    case 1:
                        AudioManager.instance.PlaySound("DestroyTempoline2");
                        break;
                    case 2:
                        break;
                }
                break;
            case ETileType.Bonus:
                break;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound '" + name + "' not found.");
            return;
        }

        if (shouldRandomizePitch)
        {
            s.source.pitch = Random.Range(0.90f, 1.1f);
            s.source.Play();
            shouldRandomizePitch = false;
        }
        else
        {
            s.source.pitch = 1f;
            s.source.Play();
        }

    }

    public void PlaySong(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Song '" + name + "' not found.");
            return;
        }
        currentSong = s.name;
        s.source.Play();
    }

    public void StopSound(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            if (firstSong) //This was added due to the fact that an error message would appear in Unity when the game is first launched since there is no sound to match "currentSong" until the first
                           //song has begun playing.
            {
                firstSong = false;
                return;
            }
            else
            {
                Debug.LogWarning("Sound '" + name + "' not found.");
                return;
            }
        }
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;

        s.source.Stop();
    }
}