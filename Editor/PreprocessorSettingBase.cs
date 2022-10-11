using System;
using UnityEditor;
using UnityEngine;

namespace Kogane
{
    /// <summary>
    /// テクスチャや SpriteAtlas の個別の設定の基底クラス
    /// </summary>
    [Serializable]
    public abstract class PreprocessorSettingBase<T> where T : ScriptableObject
    {
        //================================================================================
        // 変数(SerializeField)
        //================================================================================
        [SerializeField] private string m_path = "";
        [SerializeField] private string m_guid = "";

        //================================================================================
        // プロパティ
        //================================================================================
        public string Path     => m_path;
        public T      Settings => AssetDatabase.LoadAssetAtPath<T>( AssetDatabase.GUIDToAssetPath( m_guid ) );
    }
}