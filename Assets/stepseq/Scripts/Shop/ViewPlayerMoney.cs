using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public class ViewPlayerMoney : MonoBehaviour
    {
        [SerializeField]
        private TextMesh m_textMesh = null!;
        
        private void Awake()
        {
            PlayerState.Money
                .Subscribe(this, (money, state) => { state.m_textMesh.text = $"${money.ToString()}"; })
                .RegisterTo(destroyCancellationToken);
        }
    }
}