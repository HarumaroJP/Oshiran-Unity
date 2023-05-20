using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteImage;

    [SerializeField, HideInInspector] Sprite[] spriteTextures;

    [SerializeField] float animationFrameSeconds;

    public bool loop;

    public void SpriteAnimeStart(CancellationToken cancellationToken)
    {
        SpriteAnimeCoroutine(cancellationToken).Forget();
    }


    public void ResetSprite()
    {
        spriteImage.sprite = null;
    }

    async UniTaskVoid SpriteAnimeCoroutine(CancellationToken cancellationToken)
    {
        do
        {
            foreach (Sprite sprite in spriteTextures)
            {
                spriteImage.sprite = sprite;
                await UniTask.Delay(TimeSpan.FromSeconds(animationFrameSeconds), cancellationToken: cancellationToken);
            }
        }
        while (loop);
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(SpriteAnimation))]
    public class SpriteAnimationEditor : Editor
    {
        ReorderableList reorderableList;

        void OnEnable()
        {
            SerializedProperty prop = serializedObject.FindProperty("spriteTextures");

            reorderableList = new ReorderableList(serializedObject, prop);

            reorderableList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "アニメーションに使用する画像");
            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty element = prop.GetArrayElementAtIndex(index);
                rect.height -= 4;
                rect.y += 2;
                EditorGUI.PropertyField(rect, element, new GUIContent("フレーム" + index));
            };
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}