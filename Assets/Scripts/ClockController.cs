using UnityEngine;

namespace Clock
{
    public class ClockController : MonoBehaviour
    {
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private WallClockManager _wallClockManager;
        [SerializeField] private DigitalClockManager _digitalClockManager;
        [SerializeField] private DragManager _dragManager;
        
        public bool isEditingTime { get; private set; } = false;
        
        private void Update()
        {
            if (!isEditingTime)
            {
                _timeManager.UpdateTime();
            }
        }
        
        public void ToggleEditing(bool isEditing)
        {
            isEditingTime = isEditing;
        }

        public void UpdateClocks()
        {
            UpdateDigitalClock();
            UpdateWallClock();
        }

        public void UpdateDigitalClock()
        {
            _digitalClockManager.UpdateDigitalClock(_timeManager.currentTime);
        }

        public void UpdateWallClock()
        {
            _wallClockManager.UpdateWallClock(_timeManager.currentTime);
        }
    }
}