using BepInEx;
using GorillaLocomotion;
using HarmonyLib;
using StupidTemplate.Classes;
using StupidTemplate.Mods;
using StupidTemplate.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;
using static StupidTemplate.Menu.Buttons;
using static StupidTemplate.Settings;

/*
 * Hello, current and future developers!
 * This is ii's Stupid Template, a base mod menu template for Gorilla Tag.
 * 
 * Comments are placed around the code showing you how certain classes work, such as the settings, buttons, and notifications.
 * 
 * If you need help with the template, you may join my Discord server: https://discord.gg/iidk
 * It's full of talented developers that can show you the way and how things work.
 * 
 * If you want to support my, check out my Patreon: https://patreon.com/iiDk
 * Any support is appreciated, and it helps me make more free content for you all!
 * 
 * Thank you, and enjoy the template!
 */

namespace StupidTemplate.Menu
{
    [HarmonyPatch(typeof(GTPlayer), "LateUpdate")]
    public class Main : MonoBehaviour
    {
        // Constant
        public static void Prefix()
        {
            // Initialize Menu
                try
                {
                    bool toOpen = (!rightHanded && ControllerInputPoller.instance.leftControllerSecondaryButton) || (rightHanded && ControllerInputPoller.instance.rightControllerSecondaryButton);
                    bool keyboardOpen = UnityInput.Current.GetKey(keyboardButton);

                    if (menu == null)
                    {
                        if (toOpen || keyboardOpen)
                        {
                            CreateMenu();
                            RecenterMenu(rightHanded, keyboardOpen);
                            if (reference == null)
                                CreateReference(rightHanded);
                        }
                    }
                    else
                    {
                        if (toOpen || keyboardOpen)
                            RecenterMenu(rightHanded, keyboardOpen);
                        else
                        {
                            GameObject.Find("Shoulder Camera").transform.Find("CM vcam1").gameObject.SetActive(true);

                            Rigidbody comp = menu.AddComponent(typeof(Rigidbody)) as Rigidbody;
                            comp.linearVelocity = (rightHanded ? GTPlayer.Instance.LeftHand.velocityTracker : GTPlayer.Instance.RightHand.velocityTracker).GetAverageVelocity(true, 0);

                            Destroy(menu, 2f);
                            menu = null;

                            Destroy(reference);
                            reference = null;
                        }
                    }
                }
                catch (Exception exc)
                {
                    Debug.LogError(string.Format("{0} // Error initializing at {1}: {2}", PluginInfo.Name, exc.StackTrace, exc.Message));
                }

            // Cleanup Gun
                try
                {
                    if (GunPointer != null)
                    {
                        if (!GunPointer.activeSelf)
                            Destroy(GunPointer);
                        else
                            GunPointer.SetActive(false);
                    }

                    if (GunLine != null)
                    {
                        if (!GunLine.gameObject.activeSelf)
                        {
                            Destroy(GunLine.gameObject);
                            GunLine = null;
                        }
                        else
                            GunLine.gameObject.SetActive(false);
                    }
                } catch { }

            // Constant
                try
                {
                    // Pre-Execution
                        if (fpsObject != null)
                            fpsObject.text = "FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();

                    // Execute Enabled Mods
                        foreach (ButtonInfo button in buttons
                            .SelectMany(list => list)
                            .Where(button => button.enabled && button.method != null))
                        {
                            try
                            {
                                button.method.Invoke();
                            }
                            catch (Exception exc)
                            {
                                Debug.LogError(string.Format("{0} // Error with mod {1} at {2}: {3}", PluginInfo.Name, button.buttonText, exc.StackTrace, exc.Message));
                            }
                        }
                } catch (Exception exc)
                {
                    Debug.LogError(string.Format("{0} // Error with executing mods at {1}: {2}", PluginInfo.Name, exc.StackTrace, exc.Message));
                }
        }

        // Functions
        public static void CreateMenu()
        {
            // Menu Holder
                menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(menu.GetComponent<Rigidbody>());
                Destroy(menu.GetComponent<BoxCollider>());
                Destroy(menu.GetComponent<Renderer>());
                if (thickmenu)
                    menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);
                else
                    menu.transform.localScale = new Vector3(0.1f, 0.2f, 0.3825f);


            // Menu Background
                menuBackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Destroy(menuBackground.GetComponent<Rigidbody>());
                Destroy(menuBackground.GetComponent<BoxCollider>());
                menuBackground.transform.parent = menu.transform;
                menuBackground.transform.rotation = Quaternion.identity;
                menuBackground.transform.localScale = menuSize;
                menuBackground.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
                menuBackground.transform.position = new Vector3(0.05f, 0f, 0f);

                ColorChanger colorChanger = menuBackground.AddComponent<ColorChanger>();
                colorChanger.colors = backgroundColor;

            // Canvas
                canvasObject = new GameObject();
                canvasObject.transform.parent = menu.transform;
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvasScaler.dynamicPixelsPerUnit = 1000f;

            // Title and FPS and Version
            Text version = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();

            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            version.font = currentFont;
            version.text = PluginInfo.Version;
            version.fontSize = 1;
            version.color = Color.white;
            version.supportRichText = true;
            version.fontStyle = FontStyle.Italic;
            version.alignment = TextAnchor.LowerLeft;
            version.resizeTextForBestFit = true;
            version.resizeTextMinSize = 0;

            RectTransform versioncomponent = version.GetComponent<RectTransform>();
            versioncomponent.sizeDelta = new Vector2(0.2f, 0.04f);
            versioncomponent.localPosition = new Vector3(-0.04f, 0f, -0.0001f);
            versioncomponent.rotation = Quaternion.Euler(0f, 90f, 90f);

            text.font = currentFont;
            text.text = PluginInfo.Name + " <color=grey>[</color><color=white>" +
                        (pageNumber + 1).ToString() +
                        "</color><color=grey>]</color>";
            text.fontSize = 1;
            text.color = textColors[0];
            text.supportRichText = true;
            text.fontStyle = FontStyle.Italic;
            text.alignment = TextAnchor.MiddleCenter;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;

            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(0.28f, 0.05f);
            component.position = new Vector3(0.056f, 0f, 0.165f);
            component.rotation = Quaternion.Euler(180f, 90f, 90f);

            if (fpsCounter)
                {
                    fpsObject = new GameObject
                    {
                        transform =
                        {
                            parent = canvasObject.transform
                        }
                    }.AddComponent<Text>();
                    fpsObject.font = currentFont;
                    fpsObject.text = "FPS: " + Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
                    fpsObject.color = textColors[0];
                    fpsObject.fontSize = 1;
                    fpsObject.supportRichText = true;
                    fpsObject.fontStyle = FontStyle.Italic;
                    fpsObject.alignment = TextAnchor.MiddleCenter;
                    fpsObject.horizontalOverflow = HorizontalWrapMode.Overflow;
                    fpsObject.resizeTextForBestFit = true;
                    fpsObject.resizeTextMinSize = 0;
                    RectTransform component2 = fpsObject.GetComponent<RectTransform>();
                    component2.localPosition = Vector3.zero;
                    component2.sizeDelta = new Vector2(0.28f, 0.02f);
                    component2.position = new Vector3(0.056f, 0f, 0.135f);
                    component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }

            // Buttons
                // Disconnect
                    if (disconnectButton)
                    {
                        GameObject disconnectbutton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        if (!UnityInput.Current.GetKey(keyboardButton))
                        disconnectbutton.layer = 2;
                        Destroy(disconnectbutton.GetComponent<Rigidbody>());
                        disconnectbutton.GetComponent<BoxCollider>().isTrigger = true;
                        disconnectbutton.transform.parent = menu.transform;
                        disconnectbutton.transform.rotation = Quaternion.identity;
                        disconnectbutton.transform.localScale = new Vector3(0.09f, 1.3f, 0.08f);
                        disconnectbutton.transform.localPosition = new Vector3(0.56f, 0f, 0.55f);
                        disconnectbutton.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                        disconnectbutton.AddComponent<Classes.Button>().relatedText = "Disconnect";

                        colorChanger = disconnectbutton.AddComponent<ColorChanger>();
                        colorChanger.colors = buttonColors[0];

                        Text discontext = new GameObject
                        {
                            transform =
                            {
                                parent = canvasObject.transform
                            }
                        }.AddComponent<Text>();
                        discontext.text = "Disconnect";
                        discontext.font = currentFont;
                        discontext.fontSize = 1;
                        discontext.color = textColors[0];
                        discontext.alignment = TextAnchor.MiddleCenter;
                        discontext.resizeTextForBestFit = true;
                        discontext.resizeTextMinSize = 0;

                        RectTransform rectt = discontext.GetComponent<RectTransform>();
                        rectt.localPosition = Vector3.zero;
                        rectt.sizeDelta = new Vector2(0.2f, 0.03f);
                        rectt.localPosition = new Vector3(0.064f, 0f, 0.21f);
                        rectt.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                    }
                // Reconnect
                    if (reconnectButton)
                    {
                        GameObject reconnectbutton = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        if (!UnityInput.Current.GetKey(keyboardButton))
                        reconnectbutton.layer = 2;
                        Destroy(reconnectbutton.GetComponent<Rigidbody>());
                        reconnectbutton.GetComponent<BoxCollider>().isTrigger = true;
                        reconnectbutton.transform.parent = menu.transform;
                        reconnectbutton.transform.rotation = Quaternion.identity;
                        reconnectbutton.transform.localScale = new Vector3(0.09f, 1.3f, 0.08f);
                        reconnectbutton.transform.localPosition = new Vector3(0.56f, 0f, 0.65f);
                        reconnectbutton.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                        reconnectbutton.AddComponent<Classes.Button>().relatedText = "Reconnect";

                        colorChanger = reconnectbutton.AddComponent<ColorChanger>();
                        colorChanger.colors = buttonColors[0];

                        Text recontext = new GameObject
                        {
                            transform =
                            {
                                parent = canvasObject.transform
                            }
                        }.AddComponent<Text>();
                        recontext.text = "Reconnect";
                        recontext.font = currentFont;
                        recontext.fontSize = 1;
                        recontext.color = textColors[0];
                        recontext.alignment = TextAnchor.MiddleCenter;
                        recontext.resizeTextForBestFit = true;
                        recontext.resizeTextMinSize = 0;

                        RectTransform rectt = recontext.GetComponent<RectTransform>();
                        rectt.localPosition = Vector3.zero;
                        rectt.sizeDelta = new Vector2(0.2f, 0.03f);
                        rectt.localPosition = new Vector3(0.064f, 0f, 0.245f);
                        rectt.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
                    }

            // Page Buttons
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    if (!UnityInput.Current.GetKey(keyboardButton))
                        gameObject.layer = 2;
                    Destroy(gameObject.GetComponent<Rigidbody>());
                    gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    gameObject.transform.parent = menu.transform;
                    gameObject.transform.rotation = Quaternion.identity;
                    gameObject.transform.localScale = new Vector3(0.09f, 0.2f, 1f);
                    gameObject.transform.localPosition = new Vector3(0.6f, 1f, 0);
                    gameObject.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                    gameObject.AddComponent<Classes.Button>().relatedText = "PreviousPage";

                    colorChanger = gameObject.AddComponent<ColorChanger>();
                    colorChanger.colors = buttonColors[0];

                    text = new GameObject
                    {
                        transform =
                        {
                            parent = canvasObject.transform
                        }
                    }.AddComponent<Text>();
                    text.font = currentFont;
                    text.text = "";
                    text.fontSize = 1;
                    text.color = textColors[0];
                    text.alignment = TextAnchor.MiddleCenter;
                    text.resizeTextForBestFit = true;
                    text.resizeTextMinSize = 0;
                    component = text.GetComponent<RectTransform>();
                    component.localPosition = Vector3.zero;
                    component.sizeDelta = new Vector2(0.2f, 0.03f);
                    component.localPosition = new Vector3(0.052f, 0.195f, 0f);
                    component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

                    gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    if (!UnityInput.Current.GetKey(keyboardButton))
                    {
                        gameObject.layer = 2;
                    }
                    Destroy(gameObject.GetComponent<Rigidbody>());
                    gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    gameObject.transform.parent = menu.transform;
                    gameObject.transform.rotation = Quaternion.identity;
                    gameObject.transform.localScale = new Vector3(0.09f, 0.2f, 1f);
                    gameObject.transform.localPosition = new Vector3(0.56f, -1f, 0);
                    gameObject.GetComponent<Renderer>().material.color = buttonColors[0].colors[0].color;
                    gameObject.AddComponent<Classes.Button>().relatedText = "NextPage";

                    colorChanger = gameObject.AddComponent<ColorChanger>();
                    colorChanger.colors = buttonColors[0];

                    text = new GameObject
                    {
                        transform =
                        {
                            parent = canvasObject.transform
                        }
                    }.AddComponent<Text>();
                    text.font = currentFont;
                    text.text = " ";
                    text.fontSize = 1;
                    text.color = textColors[0];
                    text.alignment = TextAnchor.MiddleCenter;
                    text.resizeTextForBestFit = true;
                    text.resizeTextMinSize = 0;
                    component = text.GetComponent<RectTransform>();
                    component.localPosition = Vector3.zero;
                    component.sizeDelta = new Vector2(0.2f, 0.03f);
                    component.localPosition = new Vector3(0.06f, -0.195f, 0f);
                    component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            // Mod Buttons
            ButtonInfo[] activeButtons = GetVisibleButtons(currentCategory).Skip(pageNumber * buttonsPerPage).Take(buttonsPerPage).ToArray();
            for (int i = 0; i < activeButtons.Length; i++)
                CreateButton(i * 0.1f, activeButtons[i]);
        }

        public static void CreateButton(float offset, ButtonInfo method)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (!UnityInput.Current.GetKey(keyboardButton))
                gameObject.layer = 2;
            
            Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = menu.transform;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localScale = new Vector3(0.09f, 1.3f, 0.08f);
            gameObject.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - offset);
            gameObject.AddComponent<Classes.Button>().relatedText = method.buttonText;

            ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
            colorChanger.colors = method.enabled ? buttonColors[1] : buttonColors[0];

            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            text.font = currentFont;
            text.text = method.buttonText;

            if (method.overlapText != null)
                text.text = method.overlapText;
            
            text.supportRichText = true;
            text.fontSize = 1;
            text.color = method.enabled ? textColors[1] : textColors[0];
            text.alignment = TextAnchor.MiddleCenter;
            text.fontStyle = FontStyle.Italic;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(.2f, .03f);
            component.localPosition = new Vector3(.064f, 0, .111f - offset / 2.6f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        public static void RecreateMenu()
        {
            if (menu != null)
            {
                Destroy(menu);
                menu = null;

                CreateMenu();
                RecenterMenu(rightHanded, UnityInput.Current.GetKey(keyboardButton));
            }
        }

        public static void RecenterMenu(bool isRightHanded, bool isKeyboardCondition)
        {
            if (!isKeyboardCondition)
            {
                if (!isRightHanded)
                {
                    menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                }
                else
                {
                    menu.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    Vector3 rotation = GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles;
                    rotation += new Vector3(0f, 0f, 180f);
                    menu.transform.rotation = Quaternion.Euler(rotation);
                }
            }
            else
            {
                try
                {
                    TPC = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera").GetComponent<Camera>();
                }
                catch { }

                GameObject.Find("Shoulder Camera").transform.Find("CM vcam1").gameObject.SetActive(false);

                if (TPC != null)
                {
                    TPC.transform.position = new Vector3(-999f, -999f, -999f);
                    TPC.transform.rotation = Quaternion.identity;
                    GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    bg.transform.localScale = new Vector3(10f, 10f, 0.01f);
                    bg.transform.transform.position = TPC.transform.position + TPC.transform.forward;
                    Color realcolor = backgroundColor.GetCurrentColor();
                    bg.GetComponent<Renderer>().material.color = new Color32((byte)(realcolor.r * 50), (byte)(realcolor.g * 50), (byte)(realcolor.b * 50), 255);
                    Destroy(bg, 0.05f);
                    menu.transform.parent = TPC.transform;
                    menu.transform.position = TPC.transform.position + (TPC.transform.forward * 0.5f) + (TPC.transform.up * -0.02f);
                    menu.transform.rotation = TPC.transform.rotation * Quaternion.Euler(-90f, 90f, 0f);

                    if (reference != null)
                    {
                        if (Mouse.current.leftButton.isPressed)
                        {
                            Ray ray = TPC.ScreenPointToRay(Mouse.current.position.ReadValue());
                            bool hitButton = Physics.Raycast(ray, out RaycastHit hit, 100);
                            if (hitButton)
                            {
                                Classes.Button collide = hit.transform.gameObject.GetComponent<Classes.Button>();
                                collide?.OnTriggerEnter(buttonCollider);
                            }
                        } 
                        else
                            reference.transform.position = new Vector3(999f, -999f, -999f);
                    }
                }
            }
        }

        public static void CreateReference(bool isRightHanded)
        {
            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            reference.transform.parent = isRightHanded ? GorillaTagger.Instance.leftHandTransform : GorillaTagger.Instance.rightHandTransform;
            reference.GetComponent<Renderer>().material.color = backgroundColor.colors[0].color;
            reference.transform.localPosition = new Vector3(0f, -0.1f, 0f);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            buttonCollider = reference.GetComponent<SphereCollider>();

            ColorChanger colorChanger = reference.AddComponent<ColorChanger>();
            colorChanger.colors = backgroundColor;
        }

        public static void Toggle(string buttonText)
        {
            ButtonInfo[] visibleButtons = GetVisibleButtons(currentCategory);
            int lastPage = ((visibleButtons.Length + buttonsPerPage - 1) / buttonsPerPage) - 1;
            if (lastPage < 0)
                lastPage = 0;

            if (buttonText == "PreviousPage")
            {
                pageNumber--;
                if (pageNumber < 0)
                    pageNumber = lastPage;
            } else
            {
                if (buttonText == "NextPage")
                {
                    pageNumber++;
                    if (pageNumber > lastPage)
                        pageNumber = 0;
                } else
                {
                    ButtonInfo target = GetIndex(buttonText);
                    if (target != null)
                    {
                        if (target.isTogglable)
                        {
                            target.enabled = !target.enabled;
                            if (target.enabled)
                            {
                                NotifiLib.SendNotification("<color=grey>[</color><color=green>ENABLE</color><color=grey>]</color> " + target.toolTip);
                                if (target.enableMethod != null)
                                    try { target.enableMethod.Invoke(); } catch { }
                            }
                            else
                            {
                                NotifiLib.SendNotification("<color=grey>[</color><color=red>DISABLE</color><color=grey>]</color> " + target.toolTip);
                                if (target.disableMethod != null)
                                    try { target.disableMethod.Invoke(); } catch { }
                            }
                        }
                        else if (target.toolTip == "for master client")
                            Master.CheckIsMaster();
                        else
                        {
                            NotifiLib.SendNotification("<color=grey>[</color><color=green>TOGGLE</color><color=grey>]</color> " + target.toolTip);
                            if (target.method != null)
                                try { target.method.Invoke(); } catch { }
                        }
                    }
                    else
                        Debug.LogError(buttonText + " does not exist");
                }
            }
            RecreateMenu();
        }

        public static ButtonInfo[] GetVisibleButtons(int category)
        {
            if (category < 0 || category >= buttons.Length)
                return Array.Empty<ButtonInfo>();

            return buttons[category]
                .Where(button => button != null && button.IsVisible())
                .ToArray();
        }

        private static readonly Dictionary<string, (int Category, int Index)> cacheGetIndex = new Dictionary<string, (int Category, int Index)>(); // Looping through 800 elements is not a light task :/        public static ButtonInfo GetIndex(string buttonText)
        public static ButtonInfo GetIndex(string buttonText)
        {
            if (buttonText == null)
                return null;

            if (cacheGetIndex.ContainsKey(buttonText))
            {
                var CacheData = cacheGetIndex[buttonText];
                try
                {
                    if (buttons[CacheData.Category][CacheData.Index].buttonText == buttonText)
                        return buttons[CacheData.Category][CacheData.Index];
                }
                catch { cacheGetIndex.Remove(buttonText); }
            }

            int categoryIndex = 0;
            foreach (ButtonInfo[] buttons in buttons)
            {
                int buttonIndex = 0;
                foreach (ButtonInfo button in buttons)
                {
                    if (button.buttonText == buttonText)
                    {
                        try
                        {
                            cacheGetIndex.Add(buttonText, (categoryIndex, buttonIndex));
                        }
                        catch
                        {
                            if (cacheGetIndex.ContainsKey(buttonText))
                                cacheGetIndex.Remove(buttonText);
                        }

                        return button;
                    }
                    buttonIndex++;
                }
                categoryIndex++;
            }

            return null;
        }

        public static Vector3 RandomVector3(float range = 1f) =>
            new Vector3(UnityEngine.Random.Range(-range, range),
                        UnityEngine.Random.Range(-range, range),
                        UnityEngine.Random.Range(-range, range));

        public static Quaternion RandomQuaternion(float range = 360f) =>
            Quaternion.Euler(UnityEngine.Random.Range(0f, range),
                        UnityEngine.Random.Range(0f, range),
                        UnityEngine.Random.Range(0f, range));

        public static Color RandomColor(byte range = 255, byte alpha = 255) =>
            new Color32((byte)UnityEngine.Random.Range(0, range),
                        (byte)UnityEngine.Random.Range(0, range),
                        (byte)UnityEngine.Random.Range(0, range),
                        alpha);

        public static (Vector3 position, Quaternion rotation, Vector3 up, Vector3 forward, Vector3 right) TrueLeftHand()
        {
            Quaternion rot = GorillaTagger.Instance.leftHandTransform.rotation * GTPlayer.Instance.LeftHand.handRotOffset;
            return (GorillaTagger.Instance.leftHandTransform.position + GorillaTagger.Instance.leftHandTransform.rotation * GTPlayer.Instance.LeftHand.handOffset, rot, rot * Vector3.up, rot * Vector3.forward, rot * Vector3.right);
        }

        public static (Vector3 position, Quaternion rotation, Vector3 up, Vector3 forward, Vector3 right) TrueRightHand()
        {
            Quaternion rot = GorillaTagger.Instance.rightHandTransform.rotation * GTPlayer.Instance.RightHand.handRotOffset;
            return (GorillaTagger.Instance.rightHandTransform.position + GorillaTagger.Instance.rightHandTransform.rotation * GTPlayer.Instance.RightHand.handOffset, rot, rot * Vector3.up, rot * Vector3.forward, rot * Vector3.right);
        }

        public static void WorldScale(GameObject obj, Vector3 targetWorldScale)
        {
            Vector3 parentScale = obj.transform.parent.lossyScale;
            obj.transform.localScale = new Vector3(
                targetWorldScale.x / parentScale.x,
                targetWorldScale.y / parentScale.y,
                targetWorldScale.z / parentScale.z
            );
        }

        public static void FixStickyColliders(GameObject platform)
        {
            Vector3[] localPositions = new Vector3[]
            {
        new Vector3(0, 1f, 0),
        new Vector3(0, -1f, 0),
        new Vector3(1f, 0, 0),
        new Vector3(-1f, 0, 0),
        new Vector3(0, 0, 1f),
        new Vector3(0, 0, -1f)
            };
            Quaternion[] localRotations = new Quaternion[]
            {
        Quaternion.Euler(90, 0, 0),
        Quaternion.Euler(-90, 0, 0),
        Quaternion.Euler(0, -90, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.identity,
        Quaternion.Euler(0, 180, 0)
            };
            for (int i = 0; i < localPositions.Length; i++)
            {
                GameObject side = GameObject.CreatePrimitive(PrimitiveType.Cube);
                try
                {
                    if (platform.GetComponent<GorillaSurfaceOverride>() != null)
                    {
                        side.AddComponent<GorillaSurfaceOverride>().overrideIndex = platform.GetComponent<GorillaSurfaceOverride>().overrideIndex;
                    }
                }
                catch { }
                float size = 0.025f;
                side.transform.SetParent(platform.transform);
                side.transform.position = localPositions[i] * (size / 2);
                side.transform.rotation = localRotations[i];
                WorldScale(side, new Vector3(size, size, 0.01f));
                side.GetComponent<Renderer>().enabled = false;
            }
        }

        public static void FixStickyColliders1(GameObject platform)
        {
            Vector3[] normals =
            {
        Vector3.up, Vector3.down,
        Vector3.left, Vector3.right,
        Vector3.forward, Vector3.back
    };

            var surface = platform.GetComponent<GorillaSurfaceOverride>();

            foreach (var normal in normals)
            {
                GameObject side = GameObject.CreatePrimitive(PrimitiveType.Cube);

                side.transform.SetParent(platform.transform);
                side.transform.localPosition = normal * 0.5f;
                side.transform.localRotation = Quaternion.LookRotation(normal);
                side.transform.localScale = new Vector3(1f, 1f, 0.02f);

                if (surface != null)
                    side.AddComponent<GorillaSurfaceOverride>().overrideIndex = surface.overrideIndex;

                Destroy(side.GetComponent<MeshRenderer>());
            }

            var main = platform.GetComponent<BoxCollider>();
            if (main == null)
                main = platform.AddComponent<BoxCollider>();

            main.isTrigger = true;
        }

        private static int? noInvisLayerMask;
        public static int NoInvisLayerMask()
        {
            noInvisLayerMask ??= ~(
                1 << LayerMask.NameToLayer("TransparentFX") |
                1 << LayerMask.NameToLayer("Ignore Raycast") |
                1 << LayerMask.NameToLayer("Zone") |
                1 << LayerMask.NameToLayer("Gorilla Trigger") |
                1 << LayerMask.NameToLayer("Gorilla Boundary") |
                1 << LayerMask.NameToLayer("GorillaCosmetics") |
                1 << LayerMask.NameToLayer("GorillaParticle"));

            return noInvisLayerMask ?? GTPlayer.Instance.locomotionEnabledLayers;
        }

        public static bool gunLocked;
        public static VRRig lockTarget;

        public static (RaycastHit Ray, GameObject NewPointer) RenderGun(int? overrideLayerMask = null)
        {
            bool usingMouse = !ControllerInputPoller.instance.rightGrab &&
                              Mouse.current != null &&
                              Mouse.current.rightButton.isPressed;

            Vector3 StartPosition;
            Vector3 Direction;

            if (usingMouse && TPC != null)
            {
                // Mouse mode: ray from camera through mouse cursor position
                Ray mouseRay = TPC.ScreenPointToRay(Mouse.current.position.ReadValue());
                StartPosition = mouseRay.origin;
                Direction = mouseRay.direction;
            }
            else
            {
                // VR mode: ray from right hand
                Transform GunTransform = GorillaTagger.Instance.rightHandTransform;
                StartPosition = GunTransform.position;
                Direction = GunTransform.forward;
            }

            Physics.Raycast(StartPosition + Direction / 4f, Direction, out var Ray, 512f, overrideLayerMask ?? NoInvisLayerMask());
            Vector3 EndPosition = gunLocked ? lockTarget.transform.position : Ray.point;

            if (EndPosition == Vector3.zero)
                EndPosition = StartPosition + Direction * 512f;

            if (GunPointer == null)
                GunPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            GunPointer.SetActive(true);
            GunPointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            GunPointer.transform.position = EndPosition;

            bool triggerActive = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f ||
                                 (Mouse.current != null && Mouse.current.leftButton.isPressed);

            Renderer PointerRenderer = GunPointer.GetComponent<Renderer>();
            PointerRenderer.material.shader = Shader.Find("GUI/Text Shader");
            PointerRenderer.material.color = gunLocked || triggerActive ? buttonColors[1].GetCurrentColor() : buttonColors[0].GetCurrentColor();

            Destroy(GunPointer.GetComponent<Collider>());

            if (GunLine == null)
            {
                GameObject line = new GameObject("iiMenu_GunLine");
                GunLine = line.AddComponent<LineRenderer>();
            }

            GunLine.gameObject.SetActive(true);
            GunLine.material.shader = Shader.Find("GUI/Text Shader");
            GunLine.startColor = backgroundColor.GetCurrentColor();
            GunLine.endColor = backgroundColor.GetCurrentColor(0.5f);
            GunLine.startWidth = 0.025f;
            GunLine.endWidth = 0.025f;
            GunLine.positionCount = 2;
            GunLine.useWorldSpace = true;

            GunLine.SetPosition(0, StartPosition);
            GunLine.SetPosition(1, EndPosition);

            return (Ray, GunPointer);
        }

        // Variables
        // Important
        // Objects
        public static GameObject menu;
        public static GameObject menuBackground;   
        public static GameObject reference;
        public static GameObject canvasObject;

        public static SphereCollider buttonCollider;
        public static Camera TPC;
        public static Text fpsObject;

        private static GameObject GunPointer;
        private static LineRenderer GunLine;

        // Data
        public static int pageNumber = 0;
        public static int _currentCategory;
        public static int currentCategory
        {
            get => _currentCategory;
            set
            {
                _currentCategory = value;
                pageNumber = 0;
            }
        }
    }
}
