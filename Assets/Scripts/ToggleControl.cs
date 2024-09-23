using UnityEngine;
using UnityEngine.UI;

namespace Clock
{
    public class ToggleControl : MonoBehaviour
    {
        [SerializeField] private Toggle _editToggle;
        [SerializeField] private ClockController _clockController;

        private void OnEnable()
        {
            _editToggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void OnDisable()
        {
            _editToggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool isEditing)
        {
            _clockController.ToggleEditing(isEditing);
        }
    }
}