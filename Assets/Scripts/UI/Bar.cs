using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI
{
    public class Bar:MonoBehaviour
    {
        private Image _valueBar;
        private Text _valueText;

        public int Max;
        public int Value;

        private void Start()
        {
            _valueBar = transform.Find("ValueBar").GetComponent<Image>();
            _valueText = transform.Find("Text").GetComponent<Text>();
        }

        private void Update()
        {
            _valueBar.fillAmount = (float) Value / Max;
            _valueText.text = Value + "/" + Max;
        }
    }
}