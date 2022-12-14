using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    /// <summary>
    /// PreprocessorSettingBase の Inspector を管理するエディタ拡張
    /// </summary>
    [CustomPropertyDrawer( typeof( PreprocessorSettingBase<> ), true )]
    internal sealed class PreprocessorSettingBaseDrawer : PropertyDrawer
    {
        //==============================================================================
        // 関数
        //==============================================================================
        /// <summary>
        /// GUI を表示する時に呼び出されます
        /// </summary>
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            using ( new EditorGUI.PropertyScope( position, label, property ) )
            {
                position.height = EditorGUIUtility.singleLineHeight;

                var labelWidth        = 112;
                var widthRate         = 0.575f;
                var pathLabelRect     = new Rect( position ) { width = labelWidth, };
                var pathRect          = new Rect( position ) { x     = labelWidth, width = position.width - labelWidth * widthRate, };
                var settingLabelsRect = new Rect( position ) { width = labelWidth, y     = pathRect.yMax + 2, };
                var settingsRect      = new Rect( position ) { x     = labelWidth, y     = pathRect.yMax + 2, width = position.width - labelWidth * widthRate, };

                var pathProperty = property.FindPropertyRelative( "m_path" );
                var guidProperty = property.FindPropertyRelative( "m_guid" );

                var guid      = guidProperty.stringValue;
                var assetPath = AssetDatabase.GUIDToAssetPath( guid );
                var asset     = AssetDatabase.LoadAssetAtPath<ScriptableObject>( assetPath );

                EditorGUI.PrefixLabel( pathLabelRect, new( "Path" ) );
                EditorGUI.PropertyField( pathRect, pathProperty, GUIContent.none );
                EditorGUI.PrefixLabel( settingLabelsRect, new( "Settings" ) );

                var newAsset = EditorGUI.ObjectField( settingsRect, asset, typeof( ScriptableObject ), false );

                if ( asset != newAsset )
                {
                    var newAssetPath = AssetDatabase.GetAssetPath( newAsset );
                    var newGuid      = AssetDatabase.GUIDFromAssetPath( newAssetPath );

                    guidProperty.stringValue = newGuid.ToString();
                }

                // アセットパスのドラッグ & ドロップ対応
                if ( GetDragAndDropAssetPath( pathRect, out var newPath ) )
                {
                    pathProperty.stringValue = newPath;
                }
            }
        }

        /// <summary>
        /// 高さを返します
        /// </summary>
        public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
        {
            return 40;
        }

        /// <summary>
        /// 指定された矩形にドラッグ & ドロップされたアセットのパスを返します
        /// </summary>
        private static bool GetDragAndDropAssetPath( Rect rect, out string assetPath )
        {
            var current   = Event.current;
            var controlId = GUIUtility.GetControlID( FocusType.Passive );

            assetPath = string.Empty;

            switch ( current.type )
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:

                    if ( !rect.Contains( current.mousePosition ) ) break;

                    DragAndDrop.visualMode      = DragAndDropVisualMode.Copy;
                    DragAndDrop.activeControlID = controlId;

                    if ( current.type == EventType.DragPerform )
                    {
                        DragAndDrop.AcceptDrag();

                        foreach ( var draggedObject in DragAndDrop.objectReferences )
                        {
                            assetPath = AssetDatabase.GetAssetPath( draggedObject );
                        }

                        DragAndDrop.activeControlID = 0;
                    }

                    current.Use();
                    break;
            }

            return !string.IsNullOrWhiteSpace( assetPath );
        }
    }
}