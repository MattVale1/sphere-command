/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections;
using UnityEngine;

namespace RPSCore {

    public class AudioManager : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region INSTANCING
        public static AudioManager Instance;
        #endregion
        #region CACHE
        public AudioSource source_MUSIC;
        public AudioSource source_AMBIENCE;
        public AudioSource source_WEATHER;
        public AudioSource source_SFX;
        public AudioSource source_UI;
        #endregion
        #region TIMINGS
        [Header("TIMINGS")]
        public float musicClipGapTime = 5f; // How much silence do we want inbetween tracks?
        public float musicFadeSpeed = 4f;
        #endregion
        #region MUSIC CLIPS
        [Header("MUSIC CLIPS")]
        public AudioClip[] regularMusic_Clips;
        public AudioClip[] intenseMusic_Clips;
        #endregion
        #region AMBIENCE CLIPS
        [Header("AMBIENCE CLIPS")]
        public AudioClip[] regularAmbience_Clips;
        #endregion
        #region WEATHER CLIPS
        [Header("WEATHER CLIPS")]
        public AudioClip[] rain_Clips;
        #endregion
        #region SFX CLIPS
        [Header("SFX CLIPS")]
        public AudioClip SFX_01_Clip;
        public AudioClip SFX_02_Clip;
        public AudioClip SFX_03_Clip;
        #endregion
        #region UI CLIPS
        public enum UIClipType {
            BUTTON_CLICK, BUTTON_HOVER
        }
        [Header("UI CLIPS")]
        public AudioClip UIButtonClick_Clip;
        public AudioClip UIButtonHover_Clip;
        #endregion
        #region PITCH OPTIONS
        [Header("PITCHING OPTIONS")]
        [Range(0.5f, 1f)]
        public float minRandomPitch = 1f;
        [Range(1f, 1.5f)]
        public float maxRandomPitch = 1f;
        #endregion
        #endregion


        #region SETUP
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            Init();
        }

        private void Init() {

            if (regularMusic_Clips.Length == 0) {
                RPSLib.Debug.Log("AudioManager :: No music clips found.", RPSLib.Debug.Style.Warning);
                return;
            }

            source_MUSIC.clip = regularMusic_Clips[Random.Range(0, regularMusic_Clips.Length)];
            source_MUSIC.Play();
            StartCoroutine(FadeMusic(1f));
            Invoke(nameof(Init), source_MUSIC.clip.length + musicClipGapTime);

        }
        #endregion

        #region MUSIC METHODS
        private void SwitchMusicTrack() {
            source_MUSIC.clip = regularMusic_Clips[Random.Range(0, regularMusic_Clips.Length)];
            source_MUSIC.Play();
            StartCoroutine(FadeMusic(1f));
        }

        private IEnumerator FadeMusic(float targetVol) {
            float curTime = 0;
            float curVol = source_MUSIC.volume;
            while (curTime < musicFadeSpeed) {
                curTime += Time.deltaTime;
                source_MUSIC.volume = Mathf.Lerp(curVol, targetVol, curTime / musicFadeSpeed);
                yield return null;
            }
            yield break;
        }
        #endregion

        #region AMBIENCE METHODS
        private void SwitchAmbienceTrack() {
            source_AMBIENCE.clip = regularAmbience_Clips[Random.Range(0, regularAmbience_Clips.Length)];
            source_AMBIENCE.Play();
            StartCoroutine(FadeAmbience(1f));
        }

        private IEnumerator FadeAmbience(float targetVol) {
            float curTime = 0;
            float curVol = source_AMBIENCE.volume;
            while (curTime < musicFadeSpeed) {
                curTime += Time.deltaTime;
                source_AMBIENCE.volume = Mathf.Lerp(curVol, targetVol, curTime / musicFadeSpeed);
                yield return null;
            }
            yield break;
        }
        #endregion

        #region WEATHER METHOD
        public void PlayRainAmbience() {
            source_WEATHER.clip = rain_Clips[Random.Range(0, rain_Clips.Length)];
            source_WEATHER.Play();
            StartCoroutine(FadeWeather(1f));
        }

        public void FadeWeatherAmbience() {
            StopCoroutine(FadeMusic(0));
            StartCoroutine(FadeWeather(0f));
        }

        private IEnumerator FadeWeather(float targetVol) {
            float curTime = 0;
            float curVol = source_WEATHER.volume;
            while (curTime < musicFadeSpeed) {
                curTime += Time.deltaTime;
                source_WEATHER.volume = Mathf.Lerp(curVol, targetVol, curTime / musicFadeSpeed);
                yield return null;
            }
            yield break;
        }
        #endregion

        #region SFX METHODS
        public void PlaySound(AudioClip clip, bool randomPitch = false) {
            if (randomPitch)
                source_SFX.pitch = Random.Range(minRandomPitch, maxRandomPitch);

            source_SFX.PlayOneShot(clip);
        }
        #endregion

        #region UI METHODS
        public void PlayUISound(UIClipType uiClipType, bool randomPitch = false) {
            if (randomPitch)
                source_UI.pitch = Random.Range(minRandomPitch, maxRandomPitch);

            switch (uiClipType) {
                case UIClipType.BUTTON_CLICK:
                    source_UI.PlayOneShot(UIButtonClick_Clip);
                    break;
                case UIClipType.BUTTON_HOVER:
                    source_UI.PlayOneShot(UIButtonHover_Clip);
                    break;
            }
        }
        #endregion

    }

}