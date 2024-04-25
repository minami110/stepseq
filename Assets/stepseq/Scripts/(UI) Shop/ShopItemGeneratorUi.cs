using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace stepseq
{
    [DisallowMultipleComponent]
    public sealed class ShopItemGeneratorUi : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> m_shopItemPrefabs = new();
        
        [SerializeField]
        private Transform m_itemRootTransform = null!;
        
        private void Start()
        {
            // 初回時は リロールしておく
            Generate();
        }
        
        /// <summary>
        /// ショップにアイテムを生成 (リロール) 
        /// </summary>
        /// <param name="count"></param>
        public void Generate(int count = 9)
        {
            // すでに存在するアイテムをすべて削除する
            foreach (Transform child in m_itemRootTransform)
            {
                Destroy(child.gameObject);
            }
            
            // 指定された個数のアイテムをショップ内に生成する
            float span = 1.7f;
            for (var i = 0; i < count; i++)
            {
                var prefab = m_shopItemPrefabs[Random.Range(0, m_shopItemPrefabs.Count)];
                var go = Instantiate(prefab, m_itemRootTransform);
                
                // 3つごとに Y を 下げる
                go.transform.localPosition = new Vector3((i % 3) * span, -Mathf.Floor(i / 3) * span, 0);
            }
        }
    }
}