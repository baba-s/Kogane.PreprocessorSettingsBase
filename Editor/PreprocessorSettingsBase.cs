using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kogane
{
    /// <summary>
    /// テクスチャや SpriteAtlas の設定の基底クラス
    /// </summary>
    public abstract class PreprocessorSettingsBase<TPreprocessorSettings, TPreprocessorSetting> :
        ScriptableSingleton<TPreprocessorSettings>,
        IEnumerable<TPreprocessorSetting>
        where TPreprocessorSettings : ScriptableObject
    {
        //================================================================================
        // 変数(SerializeField)
        //================================================================================
        [SerializeField] private TPreprocessorSetting[] m_array;

        //================================================================================
        // 関数
        //================================================================================
        public void Save()
        {
            Save( true );
        }

        public IEnumerator<TPreprocessorSetting> GetEnumerator()
        {
            return ( ( IEnumerable<TPreprocessorSetting> )m_array ).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}