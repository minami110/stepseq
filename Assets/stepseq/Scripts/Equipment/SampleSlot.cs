using System.Runtime.CompilerServices;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public class SampleSlot : MonoBehaviour
    {
        private SampleBase? _sample;
        
        // ToDO: 適当
        internal int PlayerIndex = -1;
        
        private bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _sample == null;
        }
        
        internal void ReleaseMe(SampleBase sample)
        {
            if (_sample == sample)
            {
                _sample = null;
            }
        }
        
        public bool AssignSample(SampleBase sample)
        {
            if (!IsEmpty)
            {
                return false;
            }
            
            _sample = sample;
            _sample.OnAssignedSampleSlot(this);
            return true;
        }
        
        public bool Execute()
        {
            if (IsEmpty)
            {
                return false;
            }
            
            switch (PlayerIndex)
            {
                case 0:
                    _sample!.Execute(PlayerMockManager.GetPlayerMock(0), PlayerMockManager.GetPlayerMock(1));
                    break;
                case 1:
                    _sample!.Execute(PlayerMockManager.GetPlayerMock(1), PlayerMockManager.GetPlayerMock(0));
                    break;
                default:
                    throw new System.Exception("Invalid PlayerIndex");
            }
            return true;
        }
    }
}