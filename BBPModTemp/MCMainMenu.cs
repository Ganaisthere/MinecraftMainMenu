using MTM101BaldAPI.UI;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MinecraftMainMenu
{
    public class MCMainMenu : MonoBehaviour
    {
        [SerializeField]
        internal bool ready = false;
        public Canvas McMenuCanvas;
        public static MCMainMenu Instance { get; internal set; }

        internal void Start()
        {
            McMenuCanvas = gameObject.GetComponent<Canvas>();

            SetupMainMenu();
            SetupAboutMenu();
            SetupModsMenu();

            MCMainMenuCursorInitiator = UIHelpers.AddCursorInitiatorToCanvas(McMenuCanvas);
            MCMainMenuCursorInitiator.enabled = false;

            UIHelpers.AddBordersToCanvas(McMenuCanvas);

            ready = true;
        }
        internal void Update()
        {
            if (!ready)
            {
                return;
            }

            if (!MCMainMenuCursorInitiator.enabled)
            {
                MCMainMenuCursorInitiator.enabled = true;
            }
            if (MCMainMenuCursor = null)
            {
                Transform CursorOrigin = gameObject.transform.Find("CursorOrigin(Clone)");
                if (CursorOrigin != null)
                {
                    MCMainMenuCursor = CursorOrigin.gameObject;
                }
            }

            Other.FindRootGameObject("Menu", gameObject).SetActive(false);

            MainMenuUpdate();
        }
        //MainMenu------------------------------------------------
        public RawImage MainMenuStuff;
        public Image Panorama;
        public float panoramaTimer = 0f;
        public Image Title;
        public TextMeshProUGUI Copyright;
        public TextMeshProUGUI OtherInfos;
        public string copyrightText = null;
        public TextMeshProUGUI SplashText;
        public float splashTextTimer = 0f;
        public CursorInitiator MCMainMenuCursorInitiator;
        public GameObject MCMainMenuCursor = null;
        public StandardMenuButton[] MCMainMenuButtons = new StandardMenuButton[5];

        internal void SetupMainMenu()
        {
            GameObject MainMenuStuff_Obj = new GameObject("MainMenuStuff");
            MainMenuStuff_Obj.transform.SetParent(McMenuCanvas.transform, false);
            MainMenuStuff = MainMenuStuff_Obj.AddComponent<RawImage>();
            MainMenuStuff.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            MainMenuStuff.rectTransform.sizeDelta = new Vector2(480f, 360f);
            MainMenuStuff.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            MainMenuStuff.color = Color.clear;

            GameObject Panorama_Obj = new GameObject("Panorama");
            Panorama_Obj.transform.SetParent(MainMenuStuff.transform, false);
            Panorama = Panorama_Obj.AddComponent<Image>();
            Panorama.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            Panorama.rectTransform.sizeDelta = new Vector2(2400f, 360f);
            Panorama.rectTransform.pivot = new Vector2(0.1f, 0.5f);
            Panorama.sprite = BasePlugin.AssetMan.Get<Sprite>("Panorama");

            GameObject Title_Obj = new GameObject("Title");
            Title_Obj.transform.SetParent(MainMenuStuff.transform, false);
            Title = Title_Obj.AddComponent<Image>();
            Title.rectTransform.anchoredPosition3D = new Vector3(0f, 90f, 0f);
            Title.rectTransform.sizeDelta = new Vector2(410f, 120f);
            Title.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Title.sprite = BasePlugin.AssetMan.Get<Sprite>("Title");

            GameObject SplashText_Obj = new GameObject("SplashText");
            SplashText_Obj.transform.SetParent(MainMenuStuff.transform, false);
            SplashText = SplashText_Obj.AddComponent<TextMeshProUGUI>();
            SplashText.rectTransform.anchoredPosition3D = new Vector3(140f, 60f, 0f);
            SplashText.rectTransform.sizeDelta = new Vector2(180f, 135f);
            SplashText.rectTransform.rotation = Quaternion.Euler(0f, 0f, 20f);
            SplashText.alignment = TextAlignmentOptions.Center;
            SplashText.color = Color.yellow;
            SplashText.font = UIExtensions.FontAsset(BaldiFonts.ComicSans18);
            SplashText.fontSize = 18;
            SplashText.text = BasePlugin.splashes[Random.Range(0, BasePlugin.splashes.Length - 1)];

            GameObject Copyright_Obj = new GameObject("Copyright");
            Copyright_Obj.transform.SetParent(MainMenuStuff.transform, false);
            Copyright = Copyright_Obj.AddComponent<TextMeshProUGUI>();
            Copyright.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            Copyright.rectTransform.sizeDelta = new Vector2(480f, 360f);
            Copyright.alignment = TextAlignmentOptions.BottomRight;
            Copyright.color = Color.white;
            Copyright.font = UIExtensions.FontAsset(BaldiFonts.ComicSans18);
            Copyright.fontSize = 18;
            Copyright.text = "@2026 Basiclly Games";
            if (copyrightText != null)
            {
                Copyright.text = copyrightText;
            }

            GameObject OtherInfos_Obj = new GameObject("OtherInfos");
            OtherInfos_Obj.transform.SetParent(MainMenuStuff.transform, false);
            OtherInfos = OtherInfos_Obj.AddComponent<TextMeshProUGUI>();
            OtherInfos.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            OtherInfos.rectTransform.sizeDelta = new Vector2(480f, 360f);
            OtherInfos.alignment = TextAlignmentOptions.BottomLeft;
            OtherInfos.color = Color.white;
            OtherInfos.font = UIExtensions.FontAsset(BaldiFonts.ComicSans18);
            OtherInfos.fontSize = 18;

            string otherInfosText = "";
            otherInfosText += "MTMAPI\n" + BasePlugin.AllLoadedPlugins["mtm101.rulerp.bbplus.baldidevapi"].Metadata.Version.ToString();
            otherInfosText += "\n" + "Baldi's Basics Plus " + Application.version;
            int loadedMods = 0;
            foreach (var info in BasePlugin.AllLoadedPlugins)
            {
                loadedMods += 1;
            }
            otherInfosText += "\n" + loadedMods.ToString() + " mods loaded";
            OtherInfos.text = otherInfosText;

            MCMainMenuButtons[0] = Other.CreateMCButton("SPButton", MainMenuStuff.transform, new Vector3(0f, -16f, 0f), new Vector2(288f, 40f), "SPButton_Off", "SPButton_On", SPButton);
            MCMainMenuButtons[1] = Other.CreateMCButton("ModsButton", MainMenuStuff.transform, new Vector3(-74f, -62f, 0f), new Vector2(140f, 40f), "ModsButton_Off", "ModsButton_On", ModsButton);
            MCMainMenuButtons[2] = Other.CreateMCButton("AbButton", MainMenuStuff.transform, new Vector3(74f, -62f, 0f), new Vector2(140f, 40f), "AbButton_Off", "AbButton_On", AboutButton);
            MCMainMenuButtons[3] = Other.CreateMCButton("OpButton", MainMenuStuff.transform, new Vector3(-74f, -108f, 0f), new Vector2(140f, 40f), "OpButton_Off", "OpButton_On", OptionsButton);
            MCMainMenuButtons[4] = Other.CreateMCButton("QButton", MainMenuStuff.transform, new Vector3(74f, -108f, 0f), new Vector2(140f, 40f), "QButton_Off", "QButton_On", QuitButton);

            panoramaTimer = 0f;
            splashTextTimer = 0f;
        }
        internal void MainMenuUpdate()
        {
            if (panoramaTimer < 60f)
            {
                panoramaTimer += Time.unscaledDeltaTime;
            }
            else
            {
                panoramaTimer = 0f;
            }
            Panorama.rectTransform.anchoredPosition3D = new Vector3(0f - panoramaTimer / 60f * 1920f, 0f, 0f);

            if (splashTextTimer < 0.75f)
            {
                splashTextTimer += Time.unscaledDeltaTime;
            }
            else
            {
                splashTextTimer = 0f;
            }
            float splashTextEdit = math.sin(splashTextTimer / 0.75f * math.PI) * 0.25f;
            SplashText.rectTransform.localScale = new Vector3(1f + splashTextEdit, 1f + splashTextEdit, 1f);
        }
        public void QuitButton()
        {
            Application.Quit();
        }
        public void AboutButton()
        {
            AboutMenuStuff.gameObject.SetActive(true);
            MainMenuStuff.gameObject.SetActive(false);
        }
        public void SPButton()
        {
            Other.FindRootGameObject("PickMode", gameObject).SetActive(true);
            gameObject.SetActive(false);
        }
        public void OptionsButton()
        {
            Other.FindRootGameObject("Options", gameObject).SetActive(true);
            gameObject.SetActive(false);
        }
        public void ModsButton()
        {
            ModsMenuStuff.gameObject.SetActive(true);
            MainMenuStuff.gameObject.SetActive(false);
        }
        //About------------------------------------------------
        public RawImage AboutMenuStuff;
        public Image DirtBackground0;
        public RawImage AboutBackground;
        public TextMeshProUGUI DevUpdateTitle;
        public string devUpdateTitleText = "";
        public TextMeshProUGUI DevUpdateText;
        public string devUpdateTextText = "";
        public CursorInitiator McAboutCursorInitiator;
        public GameObject McAboutCursor = null;
        public StandardMenuButton[] McAboutButtons = new StandardMenuButton[2];

        internal void SetupAboutMenu()
        {
            GameObject AboutMenuStuff_Obj = new GameObject("AboutMenuStuff");
            AboutMenuStuff_Obj.transform.SetParent(McMenuCanvas.transform, false);
            AboutMenuStuff = AboutMenuStuff_Obj.AddComponent<RawImage>();
            AboutMenuStuff.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            AboutMenuStuff.rectTransform.sizeDelta = new Vector2(480f, 360f);
            AboutMenuStuff.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            AboutMenuStuff.color = Color.clear;

            GameObject DirtBackground0_Obj = new GameObject("DirtBackground0");
            DirtBackground0_Obj.transform.SetParent(AboutMenuStuff.transform, false);
            DirtBackground0 = DirtBackground0_Obj.AddComponent<Image>();
            DirtBackground0.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            DirtBackground0.rectTransform.sizeDelta = new Vector2(480f, 360f);
            DirtBackground0.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            DirtBackground0.sprite = BasePlugin.AssetMan.Get<Sprite>("DirtBackground");

            GameObject AboutBackground_Obj = new GameObject("AboutBackground");
            AboutBackground_Obj.transform.SetParent(AboutMenuStuff.transform, false);
            AboutBackground = AboutBackground_Obj.AddComponent<RawImage>();
            AboutBackground.rectTransform.anchoredPosition3D = new Vector3(0f, 24f, 0f);
            AboutBackground.rectTransform.sizeDelta = new Vector2(472f, 304f);
            AboutBackground.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            AboutBackground.color = new Color(0f, 0f, 0f, 0.5f);

            GameObject DevUpdateTitle_Obj = new GameObject("DevUpdateTitle");
            DevUpdateTitle_Obj.transform.SetParent(AboutMenuStuff.transform, false);
            DevUpdateTitle = DevUpdateTitle_Obj.AddComponent<TextMeshProUGUI>();
            DevUpdateTitle.rectTransform.anchoredPosition3D = new Vector3(0f, 148f, 0f);
            DevUpdateTitle.rectTransform.sizeDelta = new Vector2(474f, 360f);
            DevUpdateTitle.alignment = TextAlignmentOptions.Center;
            DevUpdateTitle.color = Color.white;
            DevUpdateTitle.font = UIExtensions.FontAsset(BaldiFonts.ComicSans18);
            DevUpdateTitle.fontSize = 18;
            DevUpdateTitle.text = devUpdateTitleText;

            GameObject DevUpdateText_Obj = new GameObject("DevUpdateText");
            DevUpdateText_Obj.transform.SetParent(AboutMenuStuff.transform, false);
            DevUpdateText = DevUpdateText_Obj.AddComponent<TextMeshProUGUI>();
            DevUpdateText.rectTransform.anchoredPosition3D = new Vector3(0f, 28f, 0f);
            DevUpdateText.rectTransform.sizeDelta = new Vector2(474f, 360f);
            DevUpdateText.alignment = TextAlignmentOptions.Center;
            DevUpdateText.color = Color.white;
            DevUpdateText.font = UIExtensions.FontAsset(BaldiFonts.ComicSans12);
            DevUpdateText.fontSize = 12;
            DevUpdateText.text = devUpdateTextText;

            McAboutButtons[0] = Other.CreateMCButton("BackButton", AboutMenuStuff.transform, new Vector3(-166f, -156f, 0f), new Vector2(140f, 40f), "BackButton_Off", "BackButton_On", BackButton);
            McAboutButtons[1] = Other.CreateMCButton("CreditsButton", AboutMenuStuff.transform, new Vector3(166f, -156f, 0f), new Vector2(140f, 40f), "CreditsButton_Off", "CreditsButton_On", CreditsButton);

            AboutMenuStuff.gameObject.SetActive(false);
        }
        public void BackButton()
        {
            MainMenuStuff.gameObject.SetActive(true);
            AboutMenuStuff.gameObject.SetActive(false);
            ModsMenuStuff.gameObject.SetActive(false);
        }
        public void CreditsButton()
        {
            Singleton<AdditiveSceneManager>.Instance.LoadScene("Credits");
            Destroy(gameObject);
        }
        //Mods-------------------------------------------------------------

        [SerializeField]
        public int page = 1;
        public int pageMax = 1;
        public RawImage ModsMenuStuff;
        public Image DirtBackground1;
        public RawImage[] ModBackgrounds = new RawImage[2];
        public TextMeshProUGUI ModCountDisplay;
        public StandardMenuButton[] McModsButtons = new StandardMenuButton[3];
        public StandardMenuButton[] McModsSelects = new StandardMenuButton[4];
        public Image[] McModsIcons = new Image[4];
        public TextMeshProUGUI[] McModstexts = new TextMeshProUGUI[4];
        public TextMeshProUGUI McModsPage;
        public TextMeshProUGUI McModsInfo;

        internal void SetupModsMenu()
        {
            GameObject ModsMenuStuff_Obj = new GameObject("ModsMenuStuff");
            ModsMenuStuff_Obj.transform.SetParent(McMenuCanvas.transform, false);
            ModsMenuStuff = ModsMenuStuff_Obj.AddComponent<RawImage>();
            ModsMenuStuff.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            ModsMenuStuff.rectTransform.sizeDelta = new Vector2(480f, 360f);
            ModsMenuStuff.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            ModsMenuStuff.color = Color.clear;

            GameObject DirtBackground1_Obj = new GameObject("DirtBackground1");
            DirtBackground1_Obj.transform.SetParent(ModsMenuStuff.transform, false);
            DirtBackground1 = DirtBackground1_Obj.AddComponent<Image>();
            DirtBackground1.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            DirtBackground1.rectTransform.sizeDelta = new Vector2(480f, 360f);
            DirtBackground1.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            DirtBackground1.sprite = BasePlugin.AssetMan.Get<Sprite>("DirtBackground");

            GameObject ModBackgrounds0_Obj = new GameObject("ModBackgrounds0");
            ModBackgrounds0_Obj.transform.SetParent(ModsMenuStuff.transform, false);
            ModBackgrounds[0] = ModBackgrounds0_Obj.AddComponent<RawImage>();
            ModBackgrounds[0].rectTransform.anchoredPosition3D = new Vector3(-120f, 24f, 0f);
            ModBackgrounds[0].rectTransform.sizeDelta = new Vector2(232f, 304f);
            ModBackgrounds[0].rectTransform.pivot = new Vector2(0.5f, 0.5f);
            ModBackgrounds[0].color = new Color(0f, 0f, 0f, 0.5f);

            GameObject ModBackgrounds1_Obj = new GameObject("ModBackgrounds1");
            ModBackgrounds1_Obj.transform.SetParent(ModsMenuStuff.transform, false);
            ModBackgrounds[1] = ModBackgrounds1_Obj.AddComponent<RawImage>();
            ModBackgrounds[1].rectTransform.anchoredPosition3D = new Vector3(120f, 24f, 0f);
            ModBackgrounds[1].rectTransform.sizeDelta = new Vector2(232f, 304f);
            ModBackgrounds[1].rectTransform.pivot = new Vector2(0.5f, 0.5f);
            ModBackgrounds[1].color = new Color(0f, 0f, 0f, 0.5f);

            GameObject ModCountDisplay_Obj = new GameObject("ModCountDisplay");
            ModCountDisplay_Obj.transform.SetParent(ModsMenuStuff.transform, false);
            ModCountDisplay = ModCountDisplay_Obj.AddComponent<TextMeshProUGUI>();
            ModCountDisplay.rectTransform.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            ModCountDisplay.rectTransform.sizeDelta = new Vector2(480f, 360f);
            ModCountDisplay.alignment = TextAlignmentOptions.BottomRight;
            ModCountDisplay.color = Color.white;
            ModCountDisplay.font = UIExtensions.FontAsset(BaldiFonts.ComicSans18);
            ModCountDisplay.fontSize = 18;

            string modCountText = "";
            int loadedMods = 0;
            foreach (var info in BasePlugin.AllLoadedPlugins)
            {
                loadedMods += 1;
            }
            modCountText += loadedMods.ToString() + " mods loaded";
            ModCountDisplay.text = modCountText;

            McModsButtons[0] = Other.CreateMCButton("BackButton", ModsMenuStuff.transform, new Vector3(-166f, -156f, 0f), new Vector2(140f, 40f), "BackButton_Off", "BackButton_On", BackButton);
            McModsButtons[1] = Other.CreateMCButton("LButton", ModsMenuStuff.transform, new Vector3(-204f, -96f, 0f), new Vector2(36f, 36f), "LButton_Off", "LButton_On", LButton);
            McModsButtons[2] = Other.CreateMCButton("RButton", ModsMenuStuff.transform, new Vector3(-36f, -96f, 0f), new Vector2(36f, 36f), "RButton_Off", "RButton_On", RButton);

            GameObject McModsPage_Obj = new GameObject("McModsPage");
            McModsPage_Obj.transform.SetParent(ModsMenuStuff.transform, false);
            McModsPage = McModsPage_Obj.AddComponent<TextMeshProUGUI>();
            McModsPage.rectTransform.anchoredPosition3D = new Vector3(-120f, -96f, 0f);
            McModsPage.rectTransform.sizeDelta = new Vector2(224f, 296f);
            McModsPage.alignment = TextAlignmentOptions.Center;
            McModsPage.color = Color.white;
            McModsPage.font = UIExtensions.FontAsset(BaldiFonts.ComicSans18);
            McModsPage.fontSize = 18;
            McModsPage.text = "1/1";

            CreateModsSelect(0);
            CreateModsSelect(1);
            CreateModsSelect(2);
            CreateModsSelect(3);

            GameObject McModsInfo_Obj = new GameObject("McModsInfo");
            McModsInfo_Obj.transform.SetParent(ModsMenuStuff.transform, false);
            McModsInfo = McModsInfo_Obj.AddComponent<TextMeshProUGUI>();
            McModsInfo.rectTransform.anchoredPosition3D = new Vector3(120f, 24f, 0f);
            McModsInfo.rectTransform.sizeDelta = new Vector2(224f, 296f);
            McModsInfo.alignment = TextAlignmentOptions.TopLeft;
            McModsInfo.color = Color.white;
            McModsInfo.font = UIExtensions.FontAsset(BaldiFonts.ComicSans12);
            McModsInfo.fontSize = 12;
            McModsInfo.text = "Select a mod to see its info here.";

            UpdateModsMenu();
            ModsMenuStuff.gameObject.SetActive(false);

            page = 1;
        }
        public void LButton()
        {
            page -= 1;
            if (page < 1)
            {
                page = pageMax;
            }
            UpdateModsMenu();
        }
        public void RButton()
        {
            page += 1;
            if (page > pageMax)
            {
                page = 1;
            }
            UpdateModsMenu();
        }
        public void UpdateModsMenu()
        {
            int a = BasePlugin.AllIcons.Count % 4;
            pageMax = (BasePlugin.AllIcons.Count - a) / 4;
            if (a > 0)
            {
                pageMax += 1;
            }
            if (page > pageMax)
            {
                page = pageMax;
            }
            McModsPage.text = page.ToString() + "/" + pageMax.ToString();
            for (int i = 1; i < 5; i++)
            {
                StandardMenuButton button = McModsSelects[i - 1];
                Image ModIcon = McModsIcons[i - 1];
                TextMeshProUGUI ModText = McModstexts[i - 1];

                int b = (page - 1) * 4;
                int c = b + i;
                if (c <= BasePlugin.AllKeys.Count)
                {
                    ModIcon.color = Color.white;
                    ModText.color = Color.white;
                    button.GetComponent<Image>().color = Color.white;

                    ModIcon.sprite = BasePlugin.AllIcons[BasePlugin.AllKeys[c - 1]];

                    BasePlugin.ModMeta meta = BasePlugin.AllModMetas[BasePlugin.AllKeys[c - 1]];
                    ModText.text = meta.name + "\n" + BasePlugin.AllLoadedPlugins[BasePlugin.AllKeys[c - 1]].Metadata.Version;
                }
                else
                {
                    ModIcon.color = Color.clear;
                    ModText.color = Color.clear;
                    button.GetComponent<Image>().color = Color.clear;
                }
            }
        }
        public void CreateModsSelect(int i)
        {
            GameObject ImageButton_Obj = new GameObject("ModSelect" + i.ToString());
            ImageButton_Obj.transform.SetParent(ModsMenuStuff.transform, false);
            Image ImageButton = ImageButton_Obj.AddComponent<Image>();
            ImageButton.rectTransform.anchoredPosition3D = new Vector3(-120f, 144f - 60f * i, 0f);
            ImageButton.rectTransform.sizeDelta = new Vector2(224f, 56f);
            ImageButton.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            ImageButton.sprite = BasePlugin.AssetMan.Get<Sprite>("MSButton_Off");

            StandardMenuButton button = ImageButton_Obj.ConvertToButton<StandardMenuButton>();
            button.eventOnHigh = true;

            button.audConfirmOverride = BasePlugin.AssetMan.Get<SoundObject>("Click");

            button.OnHighlight.AddListener(() =>
            {
                ImageButton.sprite = BasePlugin.AssetMan.Get<Sprite>("MSButton_On");
            });

            button.OffHighlight.AddListener(() =>
            {
                ImageButton.sprite = BasePlugin.AssetMan.Get<Sprite>("MSButton_Off");
            });

            button.OnPress.AddListener(() =>
            {
                string text = "";
                int b = (page - 1) * 4;
                int c = b + i;
                if (c <= BasePlugin.AllKeys.Count - 1)
                {
                    BasePlugin.ModMeta meta = BasePlugin.AllModMetas[BasePlugin.AllKeys[c]];
                    text += meta.name + " " + BasePlugin.AllLoadedPlugins[BasePlugin.AllKeys[c]].Metadata.Version;
                    text += "\n" + "-" + BasePlugin.AllLoadedPlugins[BasePlugin.AllKeys[c]].Metadata.GUID + "-";
                    text += "\n\n" + "By: " + meta.author;
                    text += "\n\n" + meta.description;
                    McModsInfo.text = text;
                }
            });

            GameObject ModIcon_Obj = new GameObject("ModIcon");
            ModIcon_Obj.transform.SetParent(ImageButton.transform, false);
            Image Icon = ModIcon_Obj.AddComponent<Image>();
            Icon.rectTransform.anchoredPosition3D = new Vector3(-84f, 0f, 0f);
            Icon.rectTransform.sizeDelta = new Vector2(48f, 48f);
            Icon.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Icon.sprite = BasePlugin.AssetMan.Get<Sprite>("ModsIcon_NoImage");

            GameObject ModText_Obj = new GameObject("ModText");
            ModText_Obj.transform.SetParent(ImageButton.transform, false);
            TextMeshProUGUI ModText = ModText_Obj.AddComponent<TextMeshProUGUI>();
            ModText.rectTransform.anchoredPosition3D = new Vector3(26f, 0f, 0f);
            ModText.rectTransform.sizeDelta = new Vector2(164f, 48f);
            ModText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            ModText.alignment = TextAlignmentOptions.TopLeft;
            ModText.color = Color.white;
            ModText.font = UIExtensions.FontAsset(BaldiFonts.ComicSans12);
            ModText.fontSize = 12;
            ModText.text = "";

            McModsSelects[i] = button;
            McModsIcons[i] = Icon;
            McModstexts[i] = ModText;
        }
        //OtherVanillaStuff------------------------------------------------
        public void BackButtonForVanilla()
        {
            gameObject.SetActive(true);
            MainMenuStuff.gameObject.SetActive(true);
            AboutMenuStuff.gameObject.SetActive(false);
            ModsMenuStuff.gameObject.SetActive(false);
        }
    }
}
