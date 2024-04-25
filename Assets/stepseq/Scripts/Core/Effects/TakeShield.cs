// Copyright Edanoue, Inc. All Rights Reserved.

namespace stepseq
{
    public sealed class TakeShield : EffectBase
    {
        internal override void Execute(IEntity target, object arg0)
        {
            target.AddStack(StackType.AddShield, (float)arg0);
        }
    }
}