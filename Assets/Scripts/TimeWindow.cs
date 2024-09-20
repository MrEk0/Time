using System;
using Configs;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeWindow : MonoBehaviour, IGameUpdatable, IDisposable
{
    [SerializeField] private RectTransform _minutesArrow;
    [SerializeField] private RectTransform _hoursArrow;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _editButton;
    [SerializeField] private TMP_Text _timeText;

    private DateTime _time = DateTime.UtcNow;
    private TimeDataManager _timeDataManager;

    private void Awake()
    {
        _inputField.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _editButton.onClick.AddListener(OnEditButtonClicked);
    }

    private void OnDisable()
    {
        _editButton.onClick.RemoveListener(OnEditButtonClicked);
    }

    public void Init(TimeDataManager timeDataManager, TimeController timeController)
    {
        _timeDataManager = timeDataManager;
        
        _time = timeController.LocalTime();
        UpdateTime(_time);

        _timeDataManager.UpdateTimeEvent += UpdateTime;
    }
    
    public void Dispose()
    {
        _timeDataManager.UpdateTimeEvent -= UpdateTime;
    }

    public void OnUpdate(float deltaTime)
    {
        _time = _time.AddSeconds(Time.deltaTime);
        UpdateTime(_time);
    }

    private void UpdateTime(DateTime dateTime)
    {
        _timeText.text = string.Format($"{dateTime.Hour:00} : {dateTime.Minute:00} : {dateTime.Second:00}");

        var daySeconds = (float)(dateTime - dateTime.Date).TotalSeconds;

        _minutesArrow.rotation = Quaternion.Euler(0f, 0f, -SettingsData.GetMinutesAngle(daySeconds));
        _hoursArrow.rotation = Quaternion.Euler(0f, 0f, -SettingsData.GetHourAngle(daySeconds));
    }
    
    private void OnEditButtonClicked()
    {
        _inputField.gameObject.SetActive(true);
    }
}
