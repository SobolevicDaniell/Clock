using System;
using UnityEngine;
using UnityEngine.Events;

namespace Clock
{
    public class TimeManager : MonoBehaviour
    {
        public DateTime currentTime { get; private set; }
        
        [SerializeField] private GetTimeService _getTimeService;
        [SerializeField] private ClockController _clockController;
        [SerializeField] private int timezoneOffsetHours = 3;

        private float _updateInterval = 3600f;
        private float _timeSinceLastUpdate = 0f;

        private void Start()
        {
            StartCoroutine(_getTimeService.FetchTime(OnTimeFetched, OnError));
        }

        public void UpdateTime()
        {
            _timeSinceLastUpdate += Time.deltaTime;

            if (_timeSinceLastUpdate >= _updateInterval)
            {
                StartCoroutine(_getTimeService.FetchTime(OnTimeFetched, OnError));
                _timeSinceLastUpdate = 0f;
            }

            currentTime = currentTime.AddSeconds(Time.deltaTime);
        }

        public void SetCurrentTime(DateTime newTime)
        {
            currentTime = newTime;
        }

        private void OnTimeFetched(long timestamp)
        {
            currentTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp)
                .ToOffset(TimeSpan.FromHours(timezoneOffsetHours))
                .DateTime;
        }

        private void OnError()
        {
            // Debug.LogError($"Ошибка получения времени!");
        }
    }
}