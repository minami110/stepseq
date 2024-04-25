using System;
using Edanoue.Rx;
using UnityEngine;

namespace stepseq
{
    public class CategoryVizUi : MonoBehaviour
    {
        [SerializeField]
        private TextMesh m_categoryText = null!;
        
        internal void Init(int playerIndex)
        {
            var player = PlayerMockManager.GetPlayerMock(playerIndex);
            player.SampleStore.OnCategoryChanged
                .Subscribe(this, (dict, state) =>
                {
                    // カテゴリーの変更を受け取る
                    var text = "";
                    
                    foreach (var (category, count) in dict)
                    {
                        switch (category)
                        {
                            case CategoryType.None:
                                break;
                            case CategoryType.Fire:
                                text += count switch
                                {
                                    >= 5 => $"Fire: x{count} (攻撃力を 100% 上昇)\n",
                                    >= 2 => $"Fire: x{count} (攻撃力を 10% 上昇)\n",
                                    _ => $"Fire: x{count}\n"
                                };
                                break;
                            case CategoryType.Fly:
                                text += count switch
                                {
                                    >= 3 => $"Fly: x{count} (50% の確率で相手の直接攻撃を回避, HP を 100 上昇)\n",
                                    >= 1 => $"Fly: x{count} (10% の確率で相手の直接攻撃を回避, HP を 50 上昇)\n",
                                    _ => $"Fly: x{count}\n"
                                };
                                break;
                            case CategoryType.Nature:
                                text += count switch
                                {
                                    >= 3 => $"Nature: x{count} (回復量を 100% 上昇)\n",
                                    >= 1 => $"Nature: x{count} (回復量を 20% 上昇)\n",
                                    _ => $"Nature: x{count}\n"
                                };
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    
                    state.m_categoryText.text = text;
                })
                .RegisterTo(destroyCancellationToken);
        }
    }
}