using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public abstract class SampleBase : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 100)]
        private int m_price = 5;
        
        [SerializeField]
        [TextArea]
        private string m_hintText = string.Empty;
        
        private SampleSlot? _assignedSlot;
        
        /// <summary>
        /// Gets the price of the sample.
        /// </summary>
        public int Price
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_price;
        }
        
        public string HintText
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_hintText;
        }
        
        internal bool TryBuy()
        {
            var currentMoney = PlayerState.Money.CurrentValue;
            if (currentMoney < m_price)
            {
                return false;
            }
            
            var success = false;
            var results = ArrayPool<Collider>.Shared.Rent(16);
            var count = Physics.OverlapSphereNonAlloc(transform.position, 0.1f, results,
                1 << LayerMask.NameToLayer("Default"),
                QueryTriggerInteraction.Collide);
            var resultsAsSpan = results.AsSpan(0, count);
            foreach (var col in resultsAsSpan)
            {
                if (col.TryGetComponent(out SampleSlot slot))
                {
                    if (slot.AssignSample(this))
                    {
                        PlayerState.AddMoney(-m_price);
                        success = true;
                        break;
                    }
                }
            }
            
            ArrayPool<Collider>.Shared.Return(results);
            return success;
        }
        
        internal void OnAssignedSampleSlot(SampleSlot slot)
        {
            if (_assignedSlot != null)
            {
                _assignedSlot.ReleaseMe(this);
            }
            
            _assignedSlot = slot;
            transform.position = _assignedSlot.transform.position;
        }
        
        public abstract void Execute();
    }
}