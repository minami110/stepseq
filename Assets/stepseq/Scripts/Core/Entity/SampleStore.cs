// Copyright Edanoue, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using Edanoue.Rx;

namespace stepseq
{
    public sealed class SampleStore : IDisposable
    {
        private readonly Dictionary<CategoryType, int> _categoryDict = new();
        
        private readonly Subject<Dictionary<CategoryType, int>> _onCategoryChangedSubject = new();
        private readonly HashSet<SampleBase>                    _samples                  = new(8);
        
        public Observable<Dictionary<CategoryType, int>> OnCategoryChanged => _onCategoryChangedSubject;
        
        public void Dispose()
        {
            _samples.Clear();
            _categoryDict.Clear();
            _onCategoryChangedSubject.Dispose();
        }
        
        public bool Add(SampleBase sample)
        {
            if (!_samples.Add(sample))
            {
                return false;
            }
            
            UpdateCategory();
            return true;
        }
        
        public bool Remove(SampleBase sample)
        {
            if (!_samples.Remove(sample))
            {
                return false;
            }
            
            UpdateCategory();
            return true;
        }
        
        public void Solve()
        {
            foreach (var (category, count) in _categoryDict)
            {
                // ここでカテゴリーの処理を行う
                switch (category)
                {
                    case CategoryType.None:
                    {
                        break;
                    }
                    case CategoryType.Fire:
                    {
                        switch (count)
                        {
                            case >= 5:
                                // Fire が 5 以上あれば 攻撃力を 100% 上昇させる
                                break;
                            case >= 2:
                                // Fire が 2 以上あれば 攻撃力を 10% 上昇させる
                                break;
                        }
                        
                        break;
                    }
                    case CategoryType.Fly:
                        switch (count)
                        {
                            case >= 3:
                                // Fly が 3 以上あれば 50% の確率で相手の直接攻撃を避ける, HP を 100 上昇
                                break;
                            case >= 1:
                                // Fly が 1 以上あれば 10% の確率で相手の直接攻撃を避ける, HP を 50 上昇
                                break;
                        }
                        
                        break;
                    case CategoryType.Nature:
                        switch (count)
                        {
                            case >= 3:
                                // Nature が 3 以上あれば 回復量を 100% 上昇させる
                                break;
                            case >= 1:
                                // Nature が 1 以上あれば 回復量を 20% 上昇させる
                                break;
                        }
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void UpdateCategory()
        {
            _categoryDict.Clear();
            
            var alreadyCheckTypes = new HashSet<Type>();
            
            foreach (var sample in _samples)
            {
                var type = sample.GetType();
                if (!alreadyCheckTypes.Add(type))
                {
                    continue;
                }
                
                foreach (var category in sample.Categories)
                {
                    if (!_categoryDict.TryAdd(category, 1))
                    {
                        _categoryDict[category]++;
                    }
                }
            }
            
            _onCategoryChangedSubject.OnNext(_categoryDict);
        }
    }
}