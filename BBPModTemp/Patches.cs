using HarmonyLib;
using MTM101BaldAPI.UI;
using System.Collections;
using TMPro;
using UnityEngine;

namespace MinecraftMainMenu
{
    [HarmonyPatch(typeof(MainMenu))]
    public class MainMenuPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPrefix]
        public static void StartPostfix(MainMenu __instance)
        {
            __instance.StartCoroutine(MaybeSetup(__instance));
        }

        public static IEnumerator MaybeSetup(MainMenu mainMenu)
        {
            yield return null;

            if (Other.FindRootGameObject("MCMainMenu", mainMenu.gameObject) == null)
            {
                /*GameObject MCMainMenu_Obj = new GameObject("MCMainMenu");
                Canvas MCMainMenuCanvas = MCMainMenu_Obj.AddComponent<Canvas>();
                MCMainMenuCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                MCMainMenuCanvas.worldCamera = Singleton<GlobalCam>.Instance.Cam;
                MCMainMenuCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                MCMainMenuCanvas.planeDistance = 0.31f;

                CanvasScaler MCMainMenuCanvasScaler = MCMainMenu_Obj.AddComponent<CanvasScaler>();
                MCMainMenuCanvasScaler.referenceResolution = new Vector2(480f, 360f);
                MCMainMenuCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                MCMainMenuCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

                GraphicRaycaster graphicRaycaster = MCMainMenu_Obj.AddComponent<GraphicRaycaster>();
                graphicRaycaster.blockingMask = -1;
                graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;*/

                Canvas MCMainMenuCanvas = UIHelpers.CreateBlankUIScreen("MCMainMenu", true, true);
                MCMainMenuCanvas.worldCamera = Singleton<GlobalCam>.Instance.Cam;
                MCMainMenuCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                MCMainMenuCanvas.planeDistance = 0.31f;
                MCMainMenu mcMain = MCMainMenuCanvas.gameObject.AddComponent<MCMainMenu>();

                Transform CopyrightTransform = mainMenu.transform.Find("Copyright");
                mcMain.copyrightText = CopyrightTransform.gameObject.GetComponent<TextMeshProUGUI>().text;

                Transform DevUpdateTitleTransform = Other.FindRootGameObject("About", mainMenu.gameObject).transform.Find("DevUpdateTitle");
                mcMain.devUpdateTitleText = DevUpdateTitleTransform.gameObject.GetComponent<TextMeshProUGUI>().text;
                Transform DevUpdateTextTransform = Other.FindRootGameObject("About", mainMenu.gameObject).transform.Find("DevUpdateText");
                mcMain.devUpdateTextText = DevUpdateTextTransform.gameObject.GetComponent<TextMeshProUGUI>().text;

                Transform BaseTransform = Other.FindRootGameObject("Options", mainMenu.gameObject).transform.Find("Base");
                Transform OptionsBackButtonTransform = BaseTransform.Find("BackButton");
                StandardMenuButton OptionsBackButton = OptionsBackButtonTransform.gameObject.GetComponent<StandardMenuButton>();
                OptionsBackButton.OnPress.RemoveAllListeners();
                OptionsBackButton.OnPress.AddListener(mcMain.BackButtonForVanilla);

                Transform PickModeBackButtonTransform = Other.FindRootGameObject("PickMode", mainMenu.gameObject).transform.Find("BackButton");
                StandardMenuButton PickModeBackButton = PickModeBackButtonTransform.gameObject.GetComponent<StandardMenuButton>();
                PickModeBackButton.OnPress.RemoveAllListeners();
                PickModeBackButton.OnPress.AddListener(mcMain.BackButtonForVanilla);
            }

            Singleton<MusicManager>.Instance.StopMidi();
            mainMenu.gameObject.SetActive(false);

            yield break;
        }
    }
}
