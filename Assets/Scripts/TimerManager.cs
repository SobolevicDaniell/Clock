using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Clock
{
    public class TimerManager : MonoBehaviour
    {
        public DateTime timerTime { get; private set; }
        [SerializeField] private TMP_Text _hoursText;
        [SerializeField] private TMP_Text _minutesText;
        [SerializeField] private TMP_Text _secondsText;
        [SerializeField] public Toggle _toggle;

        public bool isTimerOn => _toggle.isOn;

        public event Action timerCompleted;

        private void Start()
        {
            timerTime = DateTime.Now;
            _hoursText.GetComponent<Button>().onClick.AddListener(() => OnTimeFieldClicked(_hoursText));
            _minutesText.GetComponent<Button>().onClick.AddListener(() => OnTimeFieldClicked(_minutesText));
            _secondsText.GetComponent<Button>().onClick.AddListener(() => OnTimeFieldClicked(_secondsText));
        }

        public void UpdateTimerTime()
        {
            int hours = Mathf.Clamp(int.Parse(_hoursText.text), 0, 11);
            int minutes = Mathf.Clamp(int.Parse(_minutesText.text), 0, 59);
            int seconds = Mathf.Clamp(int.Parse(_secondsText.text), 0, 59);

            timerTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);
        }

        public void UpdateTimerFromClockHands(DateTime time)
        {
            timerTime = time;
            UpdateTimeFields();
        }

        private void UpdateTimeFields()
        {
            _hoursText.text = timerTime.Hour.ToString("00");
            _minutesText.text = timerTime.Minute.ToString("00");
            _secondsText.text = timerTime.Second.ToString("00");
        }

        public bool CheckTimer(DateTime time)
        {
            if (_toggle.isOn && time.Hour == timerTime.Hour &&
                time.Minute == timerTime.Minute &&
                time.Second == timerTime.Second)
            {
                timerCompleted?.Invoke();
                _toggle.isOn = false;
                return true;
            }
            return false;
        }

        private void OnTimeFieldClicked(TMP_Text timeField)
        {
            TouchScreenKeyboard keyboard = TouchScreenKeyboard.Open(timeField.text, TouchScreenKeyboardType.NumberPad);

            StartCoroutine(WaitForInput(keyboard, timeField));
        }

        private IEnumerator WaitForInput(TouchScreenKeyboard keyboard, TMP_Text timeField)
        {
            while (keyboard != null && !keyboard.done && !keyboard.wasCanceled)
            {
                yield return null;
            }

            if (keyboard != null && !keyboard.wasCanceled)
            {
                int newValue;
                if (int.TryParse(keyboard.text, out newValue))
                {
                    if (timeField == _hoursText && (newValue < 0 || newValue > 23))
                        newValue = Mathf.Clamp(newValue, 0, 23);
                    else if ((timeField == (_minutesText) || timeField == _secondsText) &&
                             (newValue < 0 || newValue > 59))
                        newValue = Mathf.Clamp(newValue, 0, 59);

                    timeField.text = newValue.ToString("00");
                    UpdateTimerTime();
                }
            }
        }
    }
}
