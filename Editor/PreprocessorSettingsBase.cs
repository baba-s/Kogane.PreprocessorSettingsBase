using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Kogane
{
    /// <summary>
    /// テクスチャや SpriteAtlas の設定の基底クラス
    /// </summary>
    [Serializable]
    public abstract class PreprocessorSettingsBase<TPreprocessorSettings, TPreprocessorSetting> :
        ScriptableObject,
        IEnumerable<TPreprocessorSetting>
        where TPreprocessorSettings : ScriptableObject
    {
        //================================================================================
        // 変数(SerializeField)
        //================================================================================
        [SerializeField] private TPreprocessorSetting[] m_array;

        //================================================================================
        // 変数(static)
        //================================================================================
        private static TPreprocessorSettings m_instance;

        //================================================================================
        // 関数
        //================================================================================
        protected void SaveToJson( string path )
        {
            File.WriteAllText
            (
                path: path,
                contents: JsonUtility.ToJson( m_instance, true ),
                encoding: Encoding.UTF8
            );
        }

        public IEnumerator<TPreprocessorSetting> GetEnumerator()
        {
            return ( ( IEnumerable<TPreprocessorSetting> )m_array ).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //================================================================================
        // 関数(static)
        //================================================================================
        protected static TPreprocessorSettings GetInstance( string path )
        {
            if ( m_instance != null ) return m_instance;

            m_instance = CreateInstance<TPreprocessorSettings>();

            if ( !File.Exists( path ) ) return m_instance;

            var json = File.ReadAllText( path, Encoding.UTF8 );

            if ( string.IsNullOrWhiteSpace( json ) ) return m_instance;

            JsonUtility.FromJsonOverwrite( json, m_instance );

            if ( m_instance != null ) return m_instance;

            m_instance = CreateInstance<TPreprocessorSettings>();

            return m_instance;
        }
    }
}