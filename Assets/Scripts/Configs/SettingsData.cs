using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/GameSettingsData")]
    public class SettingsData : ScriptableObject
    {
        private const int SECONDS_IN_MINUTE = 60;
        private const int SECONDS_IN_HOUR = 3600;
        private const int CLOCK_NUMBERS = 12;
        private const float CLOCK_DEGREES = 360f;

        [SerializeField] private string _serverURL = "https://yandex.com/time/sync.json";
        [Min(0f)] [SerializeField] private float serverRequestUpdateTime = 3600;

        public string ServerURL => _serverURL;
        public float ServerRequestUpdateTime => serverRequestUpdateTime;

        public static float GetHourAngle(float secondsElapsed)
        {
            return secondsElapsed * (CLOCK_DEGREES / CLOCK_NUMBERS / SECONDS_IN_HOUR);
        }

        public static float GetMinutesAngle(float secondsElapsed)
        {
            return secondsElapsed * (CLOCK_DEGREES / SECONDS_IN_MINUTE / SECONDS_IN_MINUTE);
        }

        public static float GetSeconds(float hoursAngle, float minutesAngle)
        {
            var hAngle = hoursAngle < 0 ? Mathf.Abs(hoursAngle) : CLOCK_DEGREES - hoursAngle;
            var mAngle = minutesAngle < 0 ? Mathf.Abs(minutesAngle) : CLOCK_DEGREES - minutesAngle;

            return Mathf.FloorToInt(hAngle / (CLOCK_DEGREES / CLOCK_NUMBERS)) * SECONDS_IN_HOUR +
                   mAngle / (CLOCK_DEGREES / SECONDS_IN_MINUTE) * SECONDS_IN_MINUTE;
        }
    }
}