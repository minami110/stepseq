using System;
using UnityEngine;

namespace stepseq
{
    [DisallowMultipleComponent]
    public class DummyEnemy : MonoBehaviour, IEntity
    {
        [SerializeField]
        private Shader m_shader;
        
        
        // Update is called once per frame
        private void Update()
        {
            // 普段はゆるく 回転し続ける
            transform.Rotate(Vector3.right, 10f * Time.deltaTime);
            transform.Rotate(Vector3.up, 3f * Time.deltaTime);
            transform.Rotate(Vector3.forward, 1f * Time.deltaTime);
        }
        
        public static void TakeDamage(float damage)
        {
            // ToDo:
            Debug.Log($"Enemy took {damage} damage!");
        }
        
        void IEntity.TakeDamage(float damage)
        {
            throw new NotImplementedException();
        }
    }
}