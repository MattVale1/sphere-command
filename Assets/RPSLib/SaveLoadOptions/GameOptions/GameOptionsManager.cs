/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPSCore {

    public class GameOptionsManager : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region INSTANCING
        public static GameOptionsManager Instance;
        #endregion
        #region UI
        [Header("GameOptionsCanvas")]
        private Canvas mCanvas;
        #endregion
        #region SCENE CHECK
        public bool isMainMenu = false;
        #endregion
        #region SAVEABLE DATA
        [HideInInspector]
        public SettingsData data;
        #endregion
        #region AUDIO
        [Header("-----AUDIO")]
        public AudioMixer mMixer;
        public Slider audioSlider_MASTER;
        public Slider audioSlider_MUSIC;
        public Slider audioSlider_AMBIENCE;
        public Slider audioSlider_UI;
        public Slider audioSlider_SFX;
        #endregion
        #region GRAPHICS
        [Header("-----GRAPHICS")]
        // Window settings
        public Slider FPSSlider;
        public Toggle windowedToggle;
        public Toggle vSyncToggle;
        public Slider renderScaleSlider;
        // Post processing settings
        public VolumeProfile ppVolumeProfile;
        public Slider bloomSlider;
        public Slider fovSlider;
        public Toggle motionBlurToggle;
        public Toggle depthOfFieldToggle;
        public Toggle filmGrainToggle;
        #endregion
        #region CONTROLS
        [Header("-----CONTROLS")]
        public Toggle edgeScrollToggle;
        #endregion
        #region RESOLUTIONS
        [Header("-----GAME-RESOLUTION")]
        public TMP_Dropdown resolutionDropdown;
        private Resolution[] resolutions;
        //private Vector2 currentResolution;
        #endregion
        #region LOCALIZATION
        [Header("-----GAME-LOCALIZATION")]
        public TMP_Dropdown localizationDropdown;
        #endregion
        #region URP ASSET
        [Header("-----URP")]
        public UniversalRenderPipelineAsset rpa;
        #endregion
        #endregion

        #region SETUP
        protected void Awake() {
            Instance = this;
            mCanvas = GetComponent<Canvas>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected void Start() {
            InitResolutions();
            StartCoroutine(InitLocalizations());
        }

        protected void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            HideOptionsCanvas(); // Make sure options canvas is hidden if we switch scenes, otherwise it's state wont change.
            if (!File.Exists(Application.persistentDataPath + SaveManager.Instance.gameSettingsFolderName + ".json")) {
                File.Create(Application.persistentDataPath + SaveManager.Instance.gameSettingsFolderName + ".json");
                //Debug.LogError("gameoptions.json file created......");
                Invoke(nameof(SetDefaultsAndSave), 1f);
                return;
            } else {
                //Debug.LogError("Loading gameoptions from gameoptions.json......");
                SaveManager.Instance.LoadOptions();
            }
        }

        private void SetDefaultsAndSave() {
            #region AUDIO DEFAULTS
            mMixer.SetFloat("Master", 0);
            audioSlider_MASTER.value = 0f;
            mMixer.SetFloat("MusicVol", -5f);
            audioSlider_MUSIC.value = -5f;
            mMixer.SetFloat("AmbienceVol", -20f);
            audioSlider_AMBIENCE.value = -20f;
            mMixer.SetFloat("SFXVol", -10f);
            audioSlider_SFX.value = -10f;
            mMixer.SetFloat("UIVol", 0f);
            audioSlider_UI.value = 0f;
            #endregion
            #region GRAPHIC DEFAULTS
            Application.targetFrameRate = 60;
            FPSSlider.value = 3;
            QualitySettings.vSyncCount = 1;
            vSyncToggle.isOn = true;

            windowedToggle.SetIsOnWithoutNotify(false);
            Screen.fullScreen = true;

            bloomSlider.value = 1.5f;
            SetBloomStrength();
            fovSlider.value = 1.5f;
            SetFieldOfView();
            motionBlurToggle.isOn = true;
            ToggleMotionBlur();
            depthOfFieldToggle.isOn = true;
            ToggleDepthOfField();
            filmGrainToggle.isOn = true;
            ToggleFilmGrain();
            renderScaleSlider.value = 1f;
            SetRenderScale(true);
            #endregion
            #region CONTROLS
            edgeScrollToggle.isOn = false;
            ToggleEdgeScrolling();
            #endregion
            ApplyGameOptions();
        }
        #endregion

        #region RESOLUTION
        private void InitResolutions() {
            resolutions = Screen.resolutions;
            for (int i = 0; i < resolutions.Length; i++) {
                resolutionDropdown.options[i].text = ResToString(resolutions[i]);
                resolutionDropdown.value = i;
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionDropdown.options[i].text));
            }
            resolutionDropdown.onValueChanged.AddListener(delegate { SwitchResolution(resolutions[resolutionDropdown.value]); });
        }

        public void SwitchResolution(Resolution selectedRes) {
            Screen.SetResolution(selectedRes.width, selectedRes.height, FullScreenMode.MaximizedWindow);
        }

        public void ToggleWindowed() {
            if (Screen.fullScreen == false) {
                Screen.fullScreen = true;
                windowedToggle.SetIsOnWithoutNotify(false);
            } else {
                Screen.fullScreen = false;
                windowedToggle.SetIsOnWithoutNotify(true);
            }
        }

        private string ResToString(Resolution res) {
            return res.width + " x " + res.height + " @" + res.refreshRateRatio;
        }
        #endregion

        #region GRAPHICS
        public void UpdateFPS() {
            switch (FPSSlider.value) {
                case 1:
                    Application.targetFrameRate = 0;
                    break;
                case 2:
                    Application.targetFrameRate = 30;
                    break;
                case 3:
                    Application.targetFrameRate = 60;
                    break;
                case 4:
                    Application.targetFrameRate = 120;
                    break;
            }
        }

        public void ToggleVSync() {
            if (QualitySettings.vSyncCount == 0) {
                QualitySettings.vSyncCount = 1;
                if (!vSyncToggle.isOn)
                    vSyncToggle.targetGraphic.enabled = true;
            } else {
                QualitySettings.vSyncCount = 0;
                if (vSyncToggle.isOn)
                    vSyncToggle.targetGraphic.enabled = false;
            }
        }

        public void SetBloomStrength() {
            if (ppVolumeProfile.TryGet<Bloom>(out var effect)) {
                effect.intensity.value = bloomSlider.value;
            }
        }

        public void SetFieldOfView() {
            //CameraControllerV2.Instance.SetCameraFOV(fovSlider.value);
        }

        public void ToggleMotionBlur() {
            if (ppVolumeProfile.TryGet<MotionBlur>(out var effect)) {
                effect.active = motionBlurToggle.isOn;
            }
        }

        public void ToggleDepthOfField() {
            if (ppVolumeProfile.TryGet<DepthOfField>(out var effect)) {
                effect.active = depthOfFieldToggle.isOn;
            }
        }

        public void ToggleFilmGrain() {
            if (ppVolumeProfile.TryGet<FilmGrain>(out var effect)) {
                effect.active = filmGrainToggle.isOn;
            }
        }

        public void SetRenderScale() {
            float value = Mathf.Round(renderScaleSlider.value * 10f) * 0.1f;
            renderScaleSlider.value = value;
            rpa.renderScale = value;
        }

        public void SetRenderScale(bool forceToDefault) {
            SetRenderScale();
            if (forceToDefault)
                rpa.renderScale = 1f;
        }
        #endregion

        #region AUDIO VOLUMES
        public void SetAudio_MASTER() {
            mMixer.SetFloat("Master", audioSlider_MASTER.value);
            if (audioSlider_MASTER.value <= -40) {
                mMixer.SetFloat("Master", -80f);
            }
        }

        public void SetAudio_MUSIC() {
            mMixer.SetFloat("MusicVol", audioSlider_MUSIC.value);
            if (audioSlider_MUSIC.value <= -40) {
                mMixer.SetFloat("MusicVol", -80f);
            }
        }

        public void SetAudio_AMBIENCE() {
            mMixer.SetFloat("AmbienceVol", audioSlider_AMBIENCE.value);
            if (audioSlider_AMBIENCE.value <= -40) {
                mMixer.SetFloat("AmbienceVol", -80f);
            }
        }

        public void SetAudio_UI() {
            mMixer.SetFloat("UIVol", audioSlider_UI.value);
            if (audioSlider_UI.value <= -40) {
                mMixer.SetFloat("UIVol", -80f);
            }
        }

        public void SetAudio_SFX() {
            mMixer.SetFloat("SFXVol", audioSlider_SFX.value);
            if (audioSlider_SFX.value <= -40) {
                mMixer.SetFloat("SFXVol", -80f);
            }
        }
        #endregion

        #region CONTROLS
        public void ToggleEdgeScrolling() {
            if (edgeScrollToggle.isOn == true) {
                //CameraControllerV2.Instance.edgeScrolling = true;
                //edgeScrollToggle.isOn = true;
            } else {
                //CameraControllerV2.Instance.edgeScrolling = false;
                //edgeScrollToggle.isOn = false;
            }
            ApplyGameOptions();
        }
        #endregion

        #region LOCALIZATION
        private IEnumerator InitLocalizations() {
            // Wait for the localization system to initialize, loading Locales, preloading etc.
            yield return LocalizationSettings.InitializationOperation;

            // Generate list of available Locales
            var options = new List<TMP_Dropdown.OptionData>();
            int selected = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i) {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add(new TMP_Dropdown.OptionData(locale.name));
            }
            localizationDropdown.options = options;

            localizationDropdown.value = selected;
            localizationDropdown.onValueChanged.AddListener(LocaleSelected);
        }

        public void LocaleSelected(int index) {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }
        #endregion

        #region UI BUTTONS
        public void ApplyGameOptions() {
            AudioManager.Instance.PlayUISound(AudioManager.UIClipType.BUTTON_CLICK, true);
            SaveSettings();
            SaveManager.Instance.SaveOptions();
        }
        #endregion

        #region PUBLIC GETS
        public Canvas GetOptionsCanvas() {
            return mCanvas;
        }

        public void ShowOptionsCanvas() {
            mCanvas.enabled = true;
            if (GetComponent<UIAnimator>() != null)
                GetComponent<UIAnimator>().Play();
        }

        public void HideOptionsCanvas() {
            if (mCanvas.enabled == false)
                return;
            mCanvas.enabled = false;
        }
        #endregion

        #region SAVING/LOADING
        /// <summary>
        /// 
        /// </summary>
        public void SaveSettings() {
            #region AUDIO
            data.VOL_MASTER = audioSlider_MASTER.value;
            data.VOL_MUSIC = audioSlider_MUSIC.value;
            data.VOL_AMBIENCE = audioSlider_AMBIENCE.value;
            data.VOL_UI = audioSlider_UI.value;
            data.VOL_SFX = audioSlider_SFX.value;
            #endregion
            #region GRAPHICS
            data.BLOOM_STRENGTH = bloomSlider.value;
            data.MOTION_BLUR = motionBlurToggle.isOn;
            data.DOF = depthOfFieldToggle.isOn;
            data.FILM_GRAIN = filmGrainToggle.isOn;
            data.FOV = fovSlider.value;
            data.RESOLUTION_X = Screen.currentResolution.width;
            data.RESOLUTION_Y = Screen.currentResolution.height;
            data.WINDOW_X = Screen.width;
            data.WINDOW_Y = Screen.height;
            data.WINDOWED = windowedToggle.isOn;
            data.VSYNC = vSyncToggle.isOn;
            data.FPS = Application.targetFrameRate;
            data.RENDER_SCALE = renderScaleSlider.value;
            data.EDGE_SCROLLING = edgeScrollToggle.isOn;
            #endregion
            GameSettingsData.Instance.settingsData = data;
        }

        public void LoadSettings(SettingsData data) {
            ApplyAudioSettingsFromSave(data);
            ApplyGraphicsSettingsFromSave(data);
        }

        private void ApplyAudioSettingsFromSave(SettingsData data) {
            mMixer.SetFloat("Master", data.VOL_MASTER);
            audioSlider_MASTER.value = data.VOL_MASTER;

            mMixer.SetFloat("MusicVol", data.VOL_MUSIC);
            audioSlider_MUSIC.value = data.VOL_MUSIC;

            mMixer.SetFloat("AmbienceVol", data.VOL_AMBIENCE);
            audioSlider_AMBIENCE.value = data.VOL_AMBIENCE;

            mMixer.SetFloat("UIVol", data.VOL_UI);
            audioSlider_UI.value = data.VOL_UI;

            mMixer.SetFloat("SFXVol", data.VOL_SFX);
            audioSlider_SFX.value = data.VOL_SFX;
        }

        private void ApplyGraphicsSettingsFromSave(SettingsData data) {
            bloomSlider.value = data.BLOOM_STRENGTH;
            SetBloomStrength();

            motionBlurToggle.SetIsOnWithoutNotify(data.MOTION_BLUR);
            ToggleMotionBlur();

            depthOfFieldToggle.SetIsOnWithoutNotify(data.DOF);
            ToggleDepthOfField();

            filmGrainToggle.SetIsOnWithoutNotify(data.FILM_GRAIN);
            ToggleFilmGrain();

            fovSlider.value = data.FOV;
            SetFieldOfView();

            if (data.WINDOWED) {
                Screen.fullScreen = false;
                windowedToggle.SetIsOnWithoutNotify(true);
            } else {
                Screen.fullScreen = true;
                windowedToggle.SetIsOnWithoutNotify(false);
            }

            vSyncToggle.SetIsOnWithoutNotify(data.VSYNC);
            if (data.VSYNC)
                QualitySettings.vSyncCount = 1;
            if (!data.VSYNC)
                QualitySettings.vSyncCount = 0;

            Application.targetFrameRate = data.FPS;
            if (data.FPS == 0)
                FPSSlider.value = 1;
            if (data.FPS == 30)
                FPSSlider.value = 2;
            if (data.FPS == 60)
                FPSSlider.value = 3;
            if (data.FPS == 120)
                FPSSlider.value = 4;

            if (data.RENDER_SCALE == 0) {
                renderScaleSlider.value = 1f;
            } else {
                renderScaleSlider.value = data.RENDER_SCALE;
            }
            SetRenderScale();

            edgeScrollToggle.SetIsOnWithoutNotify(data.EDGE_SCROLLING);
            ToggleEdgeScrolling();
        }
        #endregion

    }

}