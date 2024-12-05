/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class CurrencyUIItem : MonoBehaviour {

        #region Public Properties
        public Image CurrencyImage;
        public TextMeshProUGUI AmountText;

        public CurrencySO Currency { get; set; }
        public int Amount;
        #endregion


        #region Public Methods
        public void SetData(CurrencySO currencySO, int amount) {
            Currency = currencySO;

            CurrencyImage.sprite = Currency.CurrencyIcon;
            AmountText.color = currencySO.CurrencyColor;

            UpdateValue(amount);
        }

        public void UpdateValue(int amount) {
            if (amount <= 0 && !Currency.CanBeInDebt) {
                gameObject.SetActive(false);
            } else {
                gameObject.SetActive(true);
            }

            Amount = amount;
            AmountText.text = $"{Amount:n0}"; // 1,234,456 - No decimals
        }
        #endregion

    }

}