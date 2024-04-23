using System.Runtime.CompilerServices;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public class TrackButton : MonoBehaviour, IClickable
    {
        [SerializeField]
        private Shader m_buttonShader = null!;
        
        [SerializeField]
        private Renderer m_buttonRenderer = null!;
        
        private bool _enabledTrack;
        private int  _id;
        
        private Material _material = null!;
        private Track    _track    = null!;
        
        private void Awake()
        {
            _material = new Material(m_buttonShader);
            m_buttonRenderer.sharedMaterial = _material;
            SetButtonColor(Color.gray);
        }
        
        private void OnDestroy()
        {
            DestroyImmediate(_material);
        }
        
        void IClickable.LeftClick(bool isPressed)
        {
            if (isPressed == false)
            {
                _enabledTrack = !_enabledTrack;
                if (_enabledTrack)
                {
                    SetButtonColor(new Color(0.45f, 0.12f, 0.16f));
                }
                else
                {
                    SetButtonColor(Color.gray);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetButtonColor(in Color color)
        {
            _material.color = color;
        }
        
        public void Init(int id, Track track)
        {
            _id = id;
            _track = track;
            TimeManager.ActiveTrack
                .Subscribe(this, (i, state) => { state.OnChangedActiveTrack(i); })
                .RegisterTo(destroyCancellationToken);
        }
        
        private void OnChangedActiveTrack(int activeTrack)
        {
            if (_enabledTrack)
            {
                if (activeTrack == _id)
                {
                    // トラックがアクティブになったので効果を適用する
                    SetButtonColor(Color.white);
                    _track.Execute();
                }
                else
                {
                    SetButtonColor(new Color(0.45f, 0.12f, 0.16f));
                }
            }
            else
            {
                if (activeTrack == _id)
                {
                    // トラックがアクティブになったので効果を適用する
                    SetButtonColor(new Color(0.6f, 0.6f, 0.6f));
                }
                else
                {
                    SetButtonColor(Color.gray);
                }
            }
        }
    }
}