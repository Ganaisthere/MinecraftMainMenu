using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.Registers;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MinecraftMainMenu
{
    [BepInPlugin("ganaisthere.plus.minecraftmainmenu", "Minecraft Main Menu", "0.1.0.0")]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi")]

    public class BasePlugin : BaseUnityPlugin
    {
        public static BasePlugin Instance { get; internal set; }
        public static Harmony harmony = new Harmony("ganaisthere.plus.minecraftmainmenu");
        public static AssetManager AssetMan = new AssetManager();
        public static Dictionary<string, PluginInfo> AllLoadedPlugins = new Dictionary<string, PluginInfo>();
        public static Dictionary<string, ModMeta> AllModMetas = new Dictionary<string, ModMeta>();
        public static Dictionary<string, Sprite> AllIcons = new Dictionary<string, Sprite>();
        public static List<string> AllKeys = new List<string>();
        public static string[] splashes = new string[1] { "Where is splashes.txt ?" };

        //----------------------------------------

        public class ModMeta
        {
            [JsonProperty("Name")]
            public string name;

            [JsonProperty("Author")]
            public string author;

            [JsonProperty("Description")]
            public string description;

            public ModMeta()
            {
                name = "Unknown Mod";
                author = "IDK";
                description = "No description added yet.";
            }
        }

        //----------------------------------------

        public void Awake()
        {
            Instance = this;
            harmony.PatchAllConditionals();

            //AddLocalization("Subtitles_English.json");

            LoadingEvents.RegisterOnAssetsLoaded(Info, LoadAssets(), LoadingEventOrder.Start);
        }

        public void Update()
        {
            //MCMainMenu.Instance?.AnotherUpdate();
        }

        internal IEnumerator LoadAssets()
        {
            yield return 1225;
            yield return "Loading Assets...";

            AddTexture2DAndToSprite("Panorama.png", "Textures");
            AddTexture2DAndToSprite("Title.png", "Textures");
            AddTexture2DAndToSprite("SPButton_Off.png", "Textures");
            AddTexture2DAndToSprite("SPButton_On.png", "Textures");
            AddTexture2DAndToSprite("ModsButton_Off.png", "Textures");
            AddTexture2DAndToSprite("ModsButton_On.png", "Textures");
            AddTexture2DAndToSprite("AbButton_Off.png", "Textures");
            AddTexture2DAndToSprite("AbButton_On.png", "Textures");
            AddTexture2DAndToSprite("OpButton_Off.png", "Textures");
            AddTexture2DAndToSprite("OpButton_On.png", "Textures");
            AddTexture2DAndToSprite("QButton_Off.png", "Textures");
            AddTexture2DAndToSprite("QButton_On.png", "Textures");
            AddTexture2DAndToSprite("CreditsButton_Off.png", "Textures");
            AddTexture2DAndToSprite("CreditsButton_On.png", "Textures");
            AddTexture2DAndToSprite("ModsIcon_NoImage.png", "Textures");
            AddTexture2DAndToSprite("LButton_Off.png", "Textures");
            AddTexture2DAndToSprite("LButton_On.png", "Textures");
            AddTexture2DAndToSprite("RButton_Off.png", "Textures");
            AddTexture2DAndToSprite("RButton_On.png", "Textures");
            AddTexture2DAndToSprite("MSButton_Off.png", "Textures");
            AddTexture2DAndToSprite("MSButton_On.png", "Textures");

            AddTexture2DAndToSprite("DirtBackground.png", "Textures");
            AddTexture2DAndToSprite("BackButton_Off.png", "Textures");
            AddTexture2DAndToSprite("BackButton_On.png", "Textures");

            AddSoundObject("Click.ogg", "SoundObjects");

            string splashesFilePath = Path.Combine(AssetLoader.GetModPath(this), "splashes.txt");
            if (File.Exists(splashesFilePath))
            {
                splashes = File.ReadAllLines(splashesFilePath);
            }

            string modIconsFolderPath = Path.Combine(AssetLoader.GetModPath(this), "ModIcons");
            if (!Directory.Exists(modIconsFolderPath))
            {
                Directory.CreateDirectory(modIconsFolderPath);
            }

            AllLoadedPlugins.Clear();
            AllModMetas.Clear();
            AllIcons.Clear();
            AllKeys.Clear();
            foreach (var info in Chainloader.PluginInfos)
            {
                AllLoadedPlugins.Add(info.Key, info.Value);
                AllKeys.Add(info.Key);

                string jsonPath = Path.Combine(modIconsFolderPath, info.Key + ".json");
                ModMeta modMeta;
                if (File.Exists(jsonPath))
                {
                    modMeta = JsonConvert.DeserializeObject<ModMeta>(File.ReadAllText(jsonPath));
                }
                else
                {
                    modMeta = new ModMeta
                    {
                        name = info.Key
                    };
                }
                AllModMetas.Add(info.Key, modMeta);

                string iconPath = Path.Combine(modIconsFolderPath, info.Key + ".png");
                Sprite icon;
                if (File.Exists(iconPath))
                {
                    icon = AssetLoader.SpriteFromFile(iconPath, new Vector2(0.5f, 0.5f));
                }
                else
                {
                    icon = AssetMan.Get<Sprite>("ModsIcon_NoImage");
                }
                AllIcons.Add(info.Key, icon);
            }

            yield break;
        }

        internal void AddTexture2DAndToSprite(string fileNameWithExtension, string chlidPath = null, float pixelsPerUnit = 50f)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                Texture2D texture2D = AssetLoader.TextureFromFile(filePath);
                Sprite sprite = AssetLoader.SpriteFromTexture2D(texture2D, pixelsPerUnit);
                AssetMan.Add(Path.GetFileNameWithoutExtension(filePath), texture2D);
                AssetMan.Add(Path.GetFileNameWithoutExtension(filePath), sprite);
            }
            else
            {
                LogStatic("File Not Exists: " + filePath);
            }
        }
        internal void AddAudioClip(string fileNameWithExtension, string chlidPath = null)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                AudioClip audioClip = AssetLoader.AudioClipFromFile(filePath);
                AssetMan.Add(Path.GetFileNameWithoutExtension(filePath), audioClip);
            }
            else
            {
                LogStatic("File Not Exists: " + filePath);
            }
        }
        internal void AddSoundObject(string fileNameWithExtension, string chlidPath = null)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                AudioClip audioClip = AssetLoader.AudioClipFromFile(filePath);
                SoundObject soundObject = ObjectCreators.CreateSoundObject(audioClip, "Nothing", SoundType.Music, Color.white, 0f);
                soundObject.name = Path.GetFileNameWithoutExtension(filePath);
                AssetMan.Add(Path.GetFileNameWithoutExtension(filePath), soundObject);
            }
            else
            {
                LogStatic("File Not Exists: " + filePath);
            }
        }
        internal void AddMidi(string fileNameWithExtension, string chlidPath = null)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                AssetLoader.MidiFromFile(filePath, Path.GetFileNameWithoutExtension(filePath));
            }
            else
            {
                LogStatic("File Not Exists: " + filePath);
            }
        }
        internal void AddLocalization(string fileNameWithExtension, string chlidPath = null)
        {
            string filePath;
            if (chlidPath == null || chlidPath == "")
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), fileNameWithExtension);
            }
            else
            {
                filePath = Path.Combine(AssetLoader.GetModPath(this), chlidPath, fileNameWithExtension);
            }
            if (File.Exists(filePath))
            {
                AssetLoader.LocalizationFromFile(filePath, Language.English);
            }
            else
            {
                LogStatic("File Not Exists: " + filePath);
            }
        }

        internal static void LogStatic(object data, int level = 0)
        {
            if (level == 1)
            {
                Instance.Logger.LogWarning(data);
            }
            else if (level == 2)
            {
                Instance.Logger.LogError(data);
            }
            else
            {
                Instance.Logger.LogInfo(data);
            }
        }
        internal void Log(object data, int level = 0)
        {
            if (level == 1)
            {
                Logger.LogWarning(data);
            }
            else if (level == 2)
            {
                Logger.LogError(data);
            }
            else
            {
                Logger.LogInfo(data);
            }
        }
    }
}
