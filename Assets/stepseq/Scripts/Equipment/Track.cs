using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public class Track : MonoBehaviour
    {
        [SerializeField]
        private SampleSlot m_sampleSlot = null!;
        
        private void Awake()
        {
            var buttons = GetComponentsInChildren<TrackButton>();
            var id = 0;
            foreach (var button in buttons)
            {
                button.Init(id++);
            }
        }
    }
}