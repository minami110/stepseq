// Copyright Edanoue, Inc. All Rights Reserved.

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace stepseq
{
    internal class MaterialWrapper : IDisposable
    {
        private readonly Material _material;
        
        public MaterialWrapper(Shader shader)
        {
            _material = new Material(shader);
        }
        
        public void Dispose()
        {
            Object.DestroyImmediate(_material);
        }
        
        public void SetColor(in Color color)
        {
            _material.color = color;
        }
        
        public static implicit operator Material(MaterialWrapper wrapper)
        {
            return wrapper._material;
        }
    }
}