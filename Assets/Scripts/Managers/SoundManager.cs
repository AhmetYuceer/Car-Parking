using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance;
   
   [SerializeField] private AudioSource _musicSource;
   [SerializeField] private AudioSource _sfxSource;

   [SerializeField] private AudioClip _musicClip;

   [SerializeField] private AudioClip _crashClip;
   [SerializeField] private AudioClip _levelCompleteClip;
   [SerializeField] private AudioClip _levelFailedClip;
   [SerializeField] private AudioClip _carStartingClip;

   private void Awake()
   {
      if (Instance == null)
         Instance = this;
      else
         Destroy(gameObject);
   }

   private void Start()
   {
      _musicSource.loop = true;
      _sfxSource.loop = false;
   }

   public void PlayBackgroundMusic()
   {
      _musicSource.clip = _musicClip;
      _musicSource.Play();
   }

   public void PlayCarStartingSound()
   {
      if (_sfxSource.isPlaying)
         _sfxSource.Stop();
      
      _sfxSource.clip = _carStartingClip;
      _sfxSource.Play();
   }
   
   public void PlayCrashSfx()
   {
      if (_sfxSource.isPlaying)
         _sfxSource.Stop();
      
      _sfxSource.clip = _crashClip;
      _sfxSource.Play();
   }
   
   public void PlayLevelComplateSfx()
   {
      if (_sfxSource.isPlaying)
         _sfxSource.Stop();
      
      _sfxSource.clip = _levelCompleteClip;
      _sfxSource.Play();
   }
   
   public void PlayLevelFailedSfx()
   {
      if (_sfxSource.isPlaying)
         _sfxSource.Stop();
      
      _sfxSource.clip = _levelFailedClip;
      _sfxSource.Play();
   }
}
