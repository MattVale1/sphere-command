/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/

namespace RPSCore {

    [System.Serializable]
    public class SettingsData {

        // WINDOW
        public int FPS;
        public bool VSYNC;
        public bool WINDOWED;
        public float RENDER_SCALE;
        public int RESOLUTION_X;
        public int RESOLUTION_Y;
        public int WINDOW_X;
        public int WINDOW_Y;

        // GRAPHICS
        public float FOV;
        public float BLOOM_STRENGTH;
        public bool MOTION_BLUR;
        public bool DOF;
        public bool FILM_GRAIN;

        // AUDIO
        public float VOL_MASTER;
        public float VOL_MUSIC;
        public float VOL_AMBIENCE;
        public float VOL_UI;
        public float VOL_SFX;

        // CONTROLS
        public bool EDGE_SCROLLING;

    }

}