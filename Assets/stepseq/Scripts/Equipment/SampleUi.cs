using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    internal sealed class SampleUi : MonoBehaviour, IDraggable
    {
        [SerializeField]
        private Shader m_buttonShader = null!;
        
        [SerializeField]
        private Renderer m_buttonRenderer = null!;
        
        [SerializeField]
        private TextMesh m_priceText = null!;
        
        [SerializeField]
        private LayerMask m_searchSampleSlotLayerMask = 0;
        
        private Vector3? _dragStartPosition;
        private Material _material = null!;
        
        private SampleBase               _sampleBase = null!;
        private CancellationTokenSource? _trackMouseCts;
        
        private void Awake()
        {
            // Get sibling component
            _sampleBase = GetComponent<SampleBase>();
            
            // Init Material
            _material = new Material(m_buttonShader);
            m_buttonRenderer.sharedMaterial = _material;
            
            // Init Look
            SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
            
            // Init TextMesh
            m_priceText.text = $"${_sampleBase.Price.ToString()}";
        }
        
        private void OnDestroy()
        {
            DestroyImmediate(_material);
            _trackMouseCts?.Cancel();
            _trackMouseCts?.Dispose();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_dragStartPosition.HasValue == false)
            {
                SetButtonColor(new Color(0.48f, 0.39f, 0.09f));
                HintBox.SetText(_sampleBase.HintText);
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
                if (_dragStartPosition.HasValue)
                {
                    return;
                }
                
                // 初回クリック時
                _dragStartPosition = transform.position;
                SetButtonColor(new Color(1f, 0.82f, 0.15f));
                HintBox.ClearText();
                
                // マウス位置を追従する Task を開始する
                _trackMouseCts = new CancellationTokenSource();
                _ = TrackMousePositionLoopAsync(_trackMouseCts.Token);
            }
            else
            {
                if (!_dragStartPosition.HasValue)
                {
                    return;
                }
                
                // マウス位置の追従タスクを終了する
                _trackMouseCts?.Cancel();
                _trackMouseCts?.Dispose();
                _trackMouseCts = null;
                
                // ドラッグ終了時の処理
                var restPosition = _dragStartPosition.Value;
                _dragStartPosition = null;
                SetButtonColor(new Color(0.18f, 0.5f, 0.22f));
                
                //購入確定エリア(インベントリ内) なら購入する, 購入に失敗したら元の場所に戻す
                if (_sampleBase.TryBuy() == false)
                {
                    transform.position = restPosition;
                }
            }
        }
        
        private async Awaitable TrackMousePositionLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                transform.position = MousePlayerState.Position;
                await Awaitable.NextFrameAsync(token);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetButtonColor(in Color color)
        {
            _material.color = color;
        }
    }
}