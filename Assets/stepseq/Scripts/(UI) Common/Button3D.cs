using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace stepseq
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public sealed class Button3D : MonoBehaviour, IClickable
    {
        [SerializeField]
        private Shader m_buttonShader = null!;
        
        [SerializeField]
        private Renderer m_buttonRenderer = null!;
        
        [SerializeField]
        private UnityEvent m_onClick = null!;
        
        [SerializeField]
        private TextMesh? m_textMesh;
        
        [SerializeField]
        [TextArea]
        private string m_hintText = string.Empty;
        
        private Material _material = null!;
        private bool     _onClickReserved;
        
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
        
        private void OnTriggerEnter(Collider other)
        {
            SetButtonColor(new Color(0.48f, 0.39f, 0.09f));
            if (m_hintText.Length > 0)
            {
                HintBox.SetText(m_hintText);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            _onClickReserved = false;
            SetButtonColor(Color.gray);
            if (m_hintText.Length > 0)
            {
                HintBox.ClearText();
            }
        }
        
        void IClickable.LeftClick(bool isPressed)
        {
            if (isPressed)
            {
                _onClickReserved = true;
                SetButtonColor(new Color(1f, 0.82f, 0.15f));
            }
            else
            {
                if (_onClickReserved)
                {
                    SetButtonColor(new Color(0.48f, 0.39f, 0.09f));
                    m_onClick.Invoke();
                    _onClickReserved = false;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetButtonColor(in Color color)
        {
            _material.color = color;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetText(string text)
        {
            if (m_textMesh == null)
            {
                return;
            }
            
            m_textMesh.text = text;
        }
    }
}