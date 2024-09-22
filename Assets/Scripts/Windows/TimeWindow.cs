using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
    public class TimeWindow : MonoBehaviour
    {
        public event Action<string> ConfirmTextEvent = delegate { };
        public event Action<Quaternion, Quaternion> ConfirmArrowsEvent = delegate { };
        public event Action EditEvent = delegate { };

        [SerializeField] private RectTransform _minutesArrow;
        [SerializeField] private RectTransform _hoursArrow;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _editButton;
        [SerializeField] private Button _confirmTextFieldButton;
        [SerializeField] private Button _confirmArrowsButton;
        [SerializeField] private TMP_Text _timeText;

        private void Awake()
        {
            _inputField.gameObject.SetActive(false);
            _confirmTextFieldButton.gameObject.SetActive(false);
            _confirmArrowsButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _editButton.onClick.AddListener(OnEditButtonClicked);
            _confirmTextFieldButton.onClick.AddListener(OnConfirmTextFieldButtonClicked);
            _confirmArrowsButton.onClick.AddListener(OnConfirmArrowsButtonClicked);
        }

        private void OnDisable()
        {
            _editButton.onClick.RemoveListener(OnEditButtonClicked);
            _confirmTextFieldButton.onClick.RemoveListener(OnConfirmTextFieldButtonClicked);
            _confirmArrowsButton.onClick.RemoveListener(OnConfirmArrowsButtonClicked);
        }

        public void UpdateTime(DateTime dateTime, float minutesAngle, float hourAngle)
        {
            _timeText.text = string.Format($"{dateTime.Hour:00} : {dateTime.Minute:00} : {dateTime.Second:00}");

            _minutesArrow.rotation = Quaternion.Euler(0f, 0f, -minutesAngle);
            _hoursArrow.rotation = Quaternion.Euler(0f, 0f, -hourAngle);
        }

        private void OnEditButtonClicked()
        {
            EditEvent();
        }

        private void OnConfirmArrowsButtonClicked()
        {
            ConfirmArrowsEvent(_hoursArrow.rotation, _minutesArrow.rotation);
        }
        
        private void OnConfirmTextFieldButtonClicked()
        {
            ConfirmTextEvent(_inputField.text);
        }
    }
}
