using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace stepseq
{
    // スクリプトの実行順序を早める, 中心的なスクリプトでいろんなものから参照されるため
    [DefaultExecutionOrder(-100)]
    [DisallowMultipleComponent]
    public sealed class HintBox : MonoBehaviour
    {
        private static HintBox? _instance;
        
        [SerializeField]
        private TextMesh m_textMesh = null!;
        
        private void Awake()
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("HintBox is a singleton and should only be initialized once.");
            }
            
            _instance = this;
            ClearText();
        }
        
        private void OnDestroy()
        {
            _instance = null;
        }
        
        public static void SetText(string text)
        {
            GetInstance().m_textMesh.text = text;
        }
        
        public static void ClearText()
        {
            GetInstance().m_textMesh.text = string.Empty;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static HintBox GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            throw new InvalidOperationException("HintBox is not initialized.");
        }
    }
}