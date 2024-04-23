using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public class Sample : MonoBehaviour, IDraggable
    {
        [SerializeField]
        private Shader m_buttonShader = null!;
        
        [SerializeField]
        private Renderer m_buttonRenderer = null!;
        
        [SerializeField]
        private TextMesh m_priceText = null!;
        
        [SerializeField]
        [Range(0, 100)]
        private int m_price = 5;
        
        [SerializeField]
        [TextArea]
        private string m_hintText = string.Empty;
        
        private Vector3? _dragStartPosition;
        
        private Material _material = null!;
        
        private void Awake()
        {
            _material = new Material(m_buttonShader);
            m_buttonRenderer.sharedMaterial = _material;
            SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
            m_priceText.text = $"${m_price.ToString()}";
        }
        
        private void Update()
        {
            if (_dragStartPosition.HasValue)
            {
                transform.position = PlayerState.Position;
            }
        }
        
        private void OnDestroy()
        {
            DestroyImmediate(_material);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_dragStartPosition.HasValue == false)
            {
                SetButtonColor(new Color(0.48f, 0.39f, 0.09f));
                HintBox.SetText(m_hintText);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (_dragStartPosition.HasValue == false)
            {
                SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
                HintBox.ClearText();
            }
        }
        
        void IClickable.LeftClick(bool isPressed)
        {
            if (isPressed)
            {
                if (_dragStartPosition.HasValue == false)
                {
                    _dragStartPosition = transform.position;
                    SetButtonColor(new Color(1f, 0.82f, 0.15f));
                    HintBox.ClearText();
                }
            }
            else
            {
                if (_dragStartPosition.HasValue)
                {
                    //購入確定エリア(インベントリ内) なら購入する
                    if (TryBuy())
                    {
                        _dragStartPosition = null;
                        SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
                    }
                    else
                    {
                        // 購入キャンセルなので元の場所に戻す
                        transform.position = _dragStartPosition.Value;
                        _dragStartPosition = null;
                        SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
                    }
                }
            }
        }
        
        private bool TryBuy()
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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetButtonColor(in Color color)
        {
            _material.color = color;
        }
    }
}