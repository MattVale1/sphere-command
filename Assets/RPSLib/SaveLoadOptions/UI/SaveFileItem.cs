/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace RPSCore {

    public class SaveFileItem : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region UI
        public TextMeshProUGUI fileName;
        public TextMeshProUGUI fileDateAndTime;
        public Sprite saveFileScreenshot;
        public Image saveFileImage;
        #endregion
        #endregion


        #region PUBLIC METHODS
        public void PassInFileNameAndDateTime(string saveGameName, DateTime saveGameDateTime) {
            fileName.text = saveGameName;
            fileDateAndTime.text = saveGameDateTime.ToShortDateString() + " - " + saveGameDateTime.ToShortTimeString();
        }

        public void PassScreenshot(Sprite screenshot) {
            saveFileScreenshot = screenshot;
            saveFileImage.sprite = saveFileScreenshot;
        }
        #endregion

    }

}