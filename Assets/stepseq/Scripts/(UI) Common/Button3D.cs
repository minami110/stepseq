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
                SetButtonColor(new Color(1f, 0.82f, 0.15f));
            }
            
            m_onClick.Invoke();
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