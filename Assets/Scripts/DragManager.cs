using System;
using UnityEngine;
using DG.Tweening;

namespace Clock
{
    public class DragManager : MonoBehaviour
    {
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private Transform _secondHand;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private ClockController _clockController;

        public bool _isDraggingHourHand { get; private set; }
        public bool _isDraggingMinuteHand { get; private set; }
        public bool _isDraggingSecondHand { get; private set; }
        
        private int _hourHandRevolutions = 0;
        private float _previousHourAngle = 0f;
        
        void Update()
        {
            if (_clockController.isEditingTime)
            {
                UpdateDraggLogic();
            }
            else
            {
                _clockController.UpdateClocks();
            }
        }

        private void UpdateDraggLogic()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleInput(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    HandleInput(Camera.main.ScreenToWorldPoint(touch.position));
                }
            }

            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                _isDraggingHourHand = false;
                _isDraggingMinuteHand = false;
                _isDraggingSecondHand = false;
            }

            if (_isDraggingHourHand || _isDraggingMinuteHand || _isDraggingSecondHand)
            {
                Vector2 inputPosition = (Input.touchCount > 0)
                    ? Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)
                    : Camera.main.ScreenToWorldPoint(Input.mousePosition);

                UpdateDraggedHand(inputPosition);
            }
        }

        private void HandleInput(Vector2 inputPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.transform == _hourHand) _isDraggingHourHand = true;
                else if (hit.transform == _minuteHand) _isDraggingMinuteHand = true;
                else if (hit.transform == _secondHand) _isDraggingSecondHand = true;
            }
        }
        
        private void UpdateDraggedHand(Vector2 inputPosition)
        {
            Vector2 direction = inputPosition - (Vector2)_hourHand.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    
            angle = (angle + 360) % 360;

            if (_isDraggingHourHand)
            {
                Debug.Log("Hour");
                _hourHand.DORotate(new Vector3(0, 0, angle), 0.2f);

                float currentHourAngle = _hourHand.localEulerAngles.z;

                float angleDelta = currentHourAngle - _previousHourAngle;

                if (angleDelta > 180)
                {
                    _hourHandRevolutions--;
                }
                else if (angleDelta < -180)
                {
                    _hourHandRevolutions++;
                }
                _previousHourAngle = currentHourAngle;
            }
            else if (_isDraggingMinuteHand)
            {
                _minuteHand.DORotate(new Vector3(0, 0, angle), 0.2f);
            }
            else if (_isDraggingSecondHand)
            {
                _secondHand.DORotate(new Vector3(0, 0, angle), 0.2f);
            }

            _timeManager.SetCurrentTime(GetCurrentTime());
            _clockController.UpdateDigitalClock();
        }
        
        private DateTime GetCurrentTime()
        {
            float hourAngle = _hourHand.localRotation.eulerAngles.z;
            float minuteAngle = _minuteHand.localRotation.eulerAngles.z;
            float secondAngle = _secondHand.localRotation.eulerAngles.z;

            int hours = Mathf.RoundToInt((-hourAngle / 30f - 45) + (_hourHandRevolutions * 12)) % 24;
            int minutes = Mathf.RoundToInt(-minuteAngle / 6f - 45) % 60;
            int seconds = Mathf.RoundToInt(-secondAngle / 6f - 45) % 60;

            if (minutes < 0) minutes += 60;
            if (seconds < 0) seconds += 60;
            if (hours < 0) hours += 24;

            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);
        }
    }
}