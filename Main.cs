using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using static BTD_Mod_Helper.Api.ModContent;
using UnityEngine.SceneManagement;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.Stats;
using BTD_Mod_Helper.Api.ModOptions;
using Harmony;
using HarmonyLib;
using System.Runtime.Serialization.Formatters.Binary;
using MelonLoader.Utils;
using static _1_Player_BTDB.EcoData;
using static _1_Player_BTDB.Main;
using System.Globalization;

[assembly: MelonInfo(typeof(_1_Player_BTDB.Main), "1 Player BTDB", "1.0.0", "00diggity000")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace _1_Player_BTDB
{
    
    public class Main : BloonsTD6Mod
    {
        AssetBundle bloonsUIBundle;
        UnityEngine.Object BloonsUI;
        GameObject BloonsMenu;
        Transform BloonParent;
        GameObject EcoText;
        public float ECO = 0;

        public static readonly ModSettingHotkey ToggleBloonsMenu = new(KeyCode.F1)
        {
            requiresRestart = true,
            icon = VanillaSprites.Red
        };
        bool showingMenu = true;
        public override void OnInitialize()
        {
            base.OnInitialize();
            bloonsUIBundle ??= GetBundle<Main>("bloons");
            if (!Directory.Exists(MelonEnvironment.UserDataDirectory + "/ECO/"))
            {
                Directory.CreateDirectory(MelonEnvironment.UserDataDirectory + "/ECO/");
            }
        }
        public override void OnMatchStart()
        {
            
            
            foreach (Canvas c in GameObject.FindObjectsOfType<Canvas>())
            {
                
                if (c.gameObject.name == "InGame")
                {
                    foreach (RectTransform r in c.gameObject.GetComponentsInChildren<RectTransform>())
                    {
                        if(r.name == "UIRect")
                        {
                            BloonParent = r.gameObject.transform;
                            foreach(NK_TextMeshProUGUI g in r.gameObject.GetComponentsInChildren<NK_TextMeshProUGUI>())
                            {
                                if(g.name == "Cash")
                                {
                                    EcoText = g.gameObject.Duplicate();
                                    EcoText.name = "ECO";
                                    EcoText.transform.parent = g.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform;
                                    string path = MelonEnvironment.UserDataDirectory + "/ECO/" + InGame.instance.GameId + ".eco";
                                    if (File.Exists(path))
                                    {
                                        BinaryFormatter formatter = new BinaryFormatter();
                                        FileStream stream = new FileStream(path, FileMode.Open);
#pragma warning disable SYSLIB0011
                                        EcoData? data = formatter.Deserialize(stream) as EcoData;
#pragma warning restore SYSLIB0011
                                        ECO = data.Eco;
                                        stream.Close();
                                        EcoText.GetComponent<NK_TextMeshProUGUI>().text = ECO.ToString("0");

                                    }
                                    else
                                    {
                                        ECO = 0;
                                        EcoText.GetComponent<NK_TextMeshProUGUI>().text = ECO.ToString("0");
                                    }
                                    EcoText.transform.position = g.gameObject.transform.parent.transform.position;
                                    EcoText.transform.position = new Vector3(EcoText.transform.position.x + 530, EcoText.transform.position.y, EcoText.transform.position.z);
                                    EcoText.transform.localScale = new Vector2(1f, 1f);
                                    EcoText.GetComponent<NK_TextMeshProUGUI>().fontSize = g.fontSize;
                                    EcoText.GetComponent<NK_TextMeshProUGUI>().color = Color.green;
                                    EcoText.RemoveComponent<CashDisplay>();
                                    //EcoText.GetComponent<NK_TextMeshProUGUI>().colorGradient = 
                                }
                            }
                        }
                        else if(r.name == "bloons(clone)")
                        {
                            BloonsMenu = r.gameObject;
                            r.position = new Vector3(0, 0, r.position.z);
                        }
                    }
                }
            }
            BloonsUI = UnityEngine.Object.Instantiate(bloonsUIBundle?.LoadAsset("bloons"), BloonParent);
            ECO = float.Parse(InGame.instance.uiRect.FindChild("MainHudLeftAlign(Clone)").FindChild("LeftGroup").FindChild("ECO").gameObject.GetComponent<NK_TextMeshProUGUI>().text, CultureInfo.InvariantCulture.NumberFormat);

            foreach (Button b in BloonParent.GetComponentsInChildren<Button>())
            {
                if (b.gameObject.name == "red")
                {
                    b.onClick.AddListener(delegate { SendBloon("Red", 20, 8, 1); });
                }else if (b.gameObject.name == "blue")
                {
                    b.onClick.AddListener(delegate { SendBloon("Blue", 24, 6, 1); });
                }
                else if (b.gameObject.name == "green")
                {
                    b.onClick.AddListener(delegate { SendBloon("Green", 18, 5, 0.9f); });
                }
                else if (b.gameObject.name == "yellow")
                {
                    b.onClick.AddListener(delegate { SendBloon("Yellow", 24, 5, 1.2f); });
                }
                else if (b.gameObject.name == "pink")
                {
                    b.onClick.AddListener(delegate { SendBloon("Pink", 28, 3, 1.4f); });
                }
                else if (b.gameObject.name == "white")
                {
                    b.onClick.AddListener(delegate { SendBloon("White", 30, 3, 1.5f); });
                }
                else if (b.gameObject.name == "black")
                {
                    b.onClick.AddListener(delegate { SendBloon("Black", 33, 3, 1.6f); });
                }
                else if (b.gameObject.name == "rainbow")
                {
                    b.onClick.AddListener(delegate { SendBloon("Rainbow", 33, 1, 3f); });
                }
                else if (b.gameObject.name == "purple")
                {
                    b.onClick.AddListener(delegate { SendBloon("Purple", 33, 5, 2.5f); });
                }
                else if (b.gameObject.name == "zebra")
                {
                    b.onClick.AddListener(delegate { SendBloon("Zebra", 33, 3, 2.5f); });
                }
                else if (b.gameObject.name == "lead")
                {
                    b.onClick.AddListener(delegate { SendBloon("Lead", 33, 4, 4f); });
                }
                else if (b.gameObject.name == "ceramic")
                {
                    b.onClick.AddListener(delegate { SendBloon("Ceramic", 150, 1, 5f); });
                }
                else if (b.gameObject.name == "moab")
                {
                    b.onClick.AddListener(delegate { SendBloon("Moab", 1000, 1, 11f); });
                }
                else if (b.gameObject.name == "bfb")
                {
                    b.onClick.AddListener(delegate { SendBloon("Bfb", 1000, 1, 25f); });
                }
                else if (b.gameObject.name == "zomg")
                {
                    b.onClick.AddListener(delegate { SendBloon("Zomg", 1000, 1, 55f); });
                }
                else if (b.gameObject.name == "bad")
                {
                    b.onClick.AddListener(delegate { SendBloon("Bad", 1000, 1, 125f); });
                }
            }
    }
        public override void OnRoundEnd()
        {
            InGame.instance.SetCash(InGame.instance.GetCash() + ECO);
        }
        void SendBloon(string bloon, float cost, int count, float eco)
        {
            if (InGame.instance.bridge.AreRoundsActive() && InGame.instance.bridge.GetCash() > cost - 1)
            {
                InGame.instance.SetCash(InGame.instance.GetCash() - cost);
                InGame.instance.SpawnBloons(bloon, count, 13);
                ECO += eco;
                EcoText.GetComponent<NK_TextMeshProUGUI>().text = ECO.ToString("0");
            }
        }
        public override void OnUpdate()
        {
            if(ToggleBloonsMenu.JustPressed())
            {
                showingMenu = !showingMenu;
                InGame.instance.uiRect.FindChild("bloons(Clone)").gameObject.SetActive(showingMenu);
            }
        }
        //public override void OnMatchEnd()
        //{
            //if (File.Exists(MelonEnvironment.UserDataDirectory + "/ECO/" + InGame.instance.GameId + ".eco"))
            //{
                //File.Delete(MelonEnvironment.UserDataDirectory + "/ECO/" + InGame.instance.GameId + ".eco");

            //}
        //}

    }
    [System.Serializable]
    public class EcoData
    {
        public float Eco;

        public EcoData(float eco)
        {
            Eco = eco;
        }
    }


    
    [HarmonyLib.HarmonyPatch(typeof(InGame), nameof(InGame.instance.Quit))]
    public class OnSaved
    {
        [HarmonyLib.HarmonyPrefix]
        public static void Prefix()
        {
            string big_numbers = InGame.instance.uiRect.FindChild("MainHudLeftAlign(Clone)").FindChild("LeftGroup").FindChild("ECO").gameObject.GetComponent<NK_TextMeshProUGUI>().text;
            float ECO = float.Parse(big_numbers, CultureInfo.InvariantCulture.NumberFormat);

            if (File.Exists(MelonEnvironment.UserDataDirectory + "/ECO/" + InGame.instance.GameId + ".eco"))
            {
                File.Delete(MelonEnvironment.UserDataDirectory + "/ECO/" + InGame.instance.GameId + ".eco");
            }
            BinaryFormatter formatter = new BinaryFormatter();
            string path = MelonEnvironment.UserDataDirectory + "/ECO/" + InGame.instance.GameId + ".eco";
            FileStream stream = new FileStream(path, FileMode.Create);
            EcoData data = new EcoData(ECO);
            #pragma warning disable SYSLIB0011
            formatter.Serialize(stream, data);
            #pragma warning restore SYSLIB0011
            stream.Close();
        }
    }
}