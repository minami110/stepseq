// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public static class EffectManager
    {
        public static void AddEffect<T>(object arg0)
            where T : EffectBase, new()
        {
            var effect = new T();
            effect.Execute(DummyEnemy.GetInstance(), arg0);
        }
    }
}