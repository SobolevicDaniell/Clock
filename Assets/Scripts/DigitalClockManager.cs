using System;
using UnityEngine;
using TMPro;

namespace Clock
{
    public class DigitalClockManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _hoursInput;
        [SerializeField] private TMP_InputField _minutesInput;
        [SerializeField] private TMP_InputField _secondsInput;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private ClockController _clockController;

        private void OnEnable()
        {
            _hoursInput.onEndEdit.AddListener(OnTimeInputChanged);
            _minutesInput.onEndEdit.AddListener(OnTimeInputChanged);
            _secondsInput.onEndEdit.AddListener(OnTimeInputChanged);
        }

        private void OnDisable()
        {
            _hoursInput.onEndEdit.RemoveListener(OnTimeInputChanged);
            _minutesInput.onEndEdit.RemoveListener(OnTimeInputChanged);
            _secondsInput.onEndEdit.RemoveListener(OnTimeInputChanged);
        }

        public void UpdateDigitalClock(DateTime time)
        {
            _hoursInput.text = time.Hour.ToString("00");
            _minutesInput.text = time.Minute.ToString("00");
            _secondsInput.text = time.Second.ToString("00");
        }

        private void OnTimeInputChanged(string value)
        {
            bool hoursValid = int.TryParse(_hoursInput.text, out int hours);
            bool minutesValid = int.TryParse(_minutesInput.text, out int minutes);
            bool secondsValid = int.TryParse(_secondsInput.text, out int seconds);

            if (!hoursValid || hours < 0 || hours > 23)
            {
                hours = Mathf.Clamp(hours, 0, 23);
                _hoursInput.text = hours.ToString("00");
            }
            if (!minutesValid || minutes < 0 || minutes > 59)
            {
                minutes = Mathf.Clamp(minutes, 0, 59);
                _minutesInput.text = minutes.ToString("00");
            }
            if (!secondsValid || seconds < 0 || seconds > 59)
            {
                seconds = Mathf.Clamp(seconds, 0, 59);
                _secondsInput.text = seconds.ToString("00");
            }
            
            DateTime newTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);
            
            _timeManager.SetCurrentTime(newTime);
            _clockController.UpdateWallClock();
            // Debug.Log("Clock updated");
        }
    }
}
