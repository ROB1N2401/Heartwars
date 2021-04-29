using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class AudioManager : MonoBehaviour
{
    //How to use: find or make an approriate location in a script for a sound effect to be triggered.
    //After doing so, type in the following to play a sound: AudioManager.instance.PlaySound("Name of sound");
    //Or the following to play a song: AudioManager.instance.PlaySong("Name of sound");
    //"Name of sound" should of course be switched out with the name of the sound/song you want to be played. Not the file name, but the name given to it in the sounds array inside the AudioManager
    //object. It is important to distinguish between sounds and songs, as the music switching system is dependent on it.
    //To stop a sound/song, you instead write: AudioManager.instance.StopSound("Name of sound");
    //This is only useful for sounds that are played on a loop, as well as all songs.

    //If you want to play a sound from inside this script instead of in another script, you can leave out the "AudioManager.instance." part of the code.

    public Sound[] sounds;
    public static AudioManager instance;
    private string sceneName;
    public string currentScene;
    private string lastScene;
    private string currentSong;
    private bool musicCanChange;
    private bool firstSong = true; //Why this bool exists is explained in the StopSound function

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

        musicCanChange = false;
    }

    public void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        if (sceneName == "MenuScene"/* || sceneName == "OtherMenuScene"*/) //Change these to reflect actual scenes later
        {
            this.currentScene = "Menu";
        }
        else if (sceneName == "GameScene"/* || sceneName == "OtherGameScene"*/) //This one as well
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

        if (musicCanChange == true)
        {
            if (this.currentScene == "Menu")
            {
                StopSound(currentSong);
                PlaySong("ScatmansWorld");
                musicCanChange = false;
            }
            else if (this.currentScene == "Heartwars")
            {
                StopSound(currentSong);
                PlaySong("CountryRoads");
                musicCanChange = false;
            }
            else if (this.currentScene == "Error")
            {
                StopSound(currentSong);
                PlaySong("UraniumFever");
                musicCanChange = false;
            }
        }
        lastScene = this.currentScene;
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound '" + name + "' not found.");
            return;
        }
        s.source.Play();
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