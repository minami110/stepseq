// Copyright Edanoue, Inc. All Rights Reserved.

using System;

namespace stepseq
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum CategoryType: int
    {
        None = 0,
        Fire = 1 << 0,
        Fly  = 1 << 1,
        Tree = 1 << 2
    }
}