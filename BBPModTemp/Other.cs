using MTM101BaldAPI.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MinecraftMainMenu
{
    public class Other
    {
        public static Color32 ColorToColor32(Color c)
        {
            return new Color32((byte)Mathf.Round(Mathf.Clamp01(c.r) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.g) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.b) * 255f), (byte)Mathf.Round(Mathf.Clamp01(c.a) * 255f));
        }

        public static Color Color32ToColor(Color32 c)
        {
            return new Color((float)(int)c.r / 255f, (float)(int)c.g / 255f, (float)(int)c.b / 255f, (float)(int)c.a / 255f);
        }

        public static StandardMenuButton CreateMCButton(string name, Transform transform, Vector3 anchoredPosition3D, Vector2 sizeDelta, string off, string on = null, UnityAction action = null)
        {
            if (on == null)
            {
                return CreateMCButton(name, transform, anchoredPosition3D, sizeDelta, BasePlugin.AssetMan.Get<Sprite>(off), null, action);
            }
            else
            {
                return CreateMCButton(name, transform, anchoredPosition3D, sizeDelta, BasePlugin.AssetMan.Get<Sprite>(off), BasePlugin.AssetMan.Get<Sprite>(on), action);
            }
        }
        public static StandardMenuButton CreateMCButton(string name, Transform transform, Vector3 anchoredPosition3D, Vector2 sizeDelta, Sprite off, Sprite on = null, UnityAction action = null)
        {
            GameObject ImageButton_Obj = new GameObject(name);
            ImageButton_Obj.transform.SetParent(transform, false);
            Image ImageButton = ImageButton_Obj.AddComponent<Image>();
            ImageButton.rectTransform.anchoredPosition3D = anchoredPosition3D;
            ImageButton.rectTransform.sizeDelta = sizeDelta;
            ImageButton.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            ImageButton.sprite = off;

            StandardMenuButton button = ImageButton_Obj.ConvertToButton<StandardMenuButton>();
            button.eventOnHigh = true;

            button.audConfirmOverride = BasePlugin.AssetMan.Get<SoundObject>("Click");

            button.OnHighlight.AddListener(() =>
            {
                if (on == null)
                {
                    ImageButton.sprite = off;
                }
                else
                {
                    ImageButton.sprite = on;
                }
            });

            button.OffHighlight.AddListener(() =>
            {
                ImageButton.sprite = off;
            });

            if (action != null)
            {
                button.OnPress.AddListener(action);
            }

            return button;
        }

        internal static GameObject FindRootGameObject(string name, GameObject rootGameObject = null)
        {
            GameObject[] AllGameObjects;
            if (rootGameObject == null)
            {
                AllGameObjects = Object.FindObjectsOfType<GameObject>();
            }
            else
            {
                AllGameObjects = rootGameObject.scene.GetRootGameObjects();
            }
            foreach (GameObject @object in AllGameObjects)
            {
                if (@object.name == name)
                {
                    return @object;
                }
            }
            BasePlugin.LogStatic("GameObject Not Found: " + name, 2);
            return null;
        }
    }
}
