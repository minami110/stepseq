// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public static class EffectManager
    {
        public static void AddEffect<T>(IEntity target, object arg0)
            where T : EffectBase, new()
        {
            var effect = new T();
            effect.Execute(target, arg0);
        }
    }
}