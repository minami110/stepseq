using System.Runtime.CompilerServices;
using UnityEngine;

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
        private TextMesh m_textMesh = null!;
        
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
            HintBox.SetText(m_hintText);
        }
        
        private void OnTriggerExit(Collider other)
        {
            SetButtonColor(Color.gray);
            HintBox.ClearText();
        }
        
        void IClickable.LeftClick(bool isPressed)
        {
            if (isPressed)
            {
                SetButtonColor(new Color(1f, 0.82f, 0.15f));
            }
            // ToDo: なにか処理を..
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetButtonColor(in Color color)
        {
            _material.color = color;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetText(string text)
        {
            m_textMesh.text = text;
        }
    }
}