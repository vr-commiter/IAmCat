using Actors.Modules;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using FightDemo;
using HarmonyLib;
using MyTrueGear;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace IAmCat_TrueGear
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BasePlugin
    {
        internal static new ManualLogSource Log;
        private static TrueGearMod _TrueGear = null;
        private static bool isOpenLeftClaw = false;
        private static bool isOpenRightClaw = false;

        public override void Load()
        {
            // Plugin startup logic
            Log = base.Log;
            Harmony.CreateAndPatchAll(typeof(Plugin));
            _TrueGear = new TrueGearMod();
            _TrueGear.Play("PlayerCrash");
            Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Mouth), "MouthAttach")]
        private static void Mouth_MouthAttach_Postfix(Mouth __instance)
        {
            Log.LogInfo("-------------------------------------------");
            Log.LogInfo("PlayerMouthAttach");
            _TrueGear.Play("PlayerMouthAttach");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Mouth), "MouthRelease")]
        private static void Mouth_MouthRelease_Postfix(Mouth __instance)
        {
            Log.LogInfo("-------------------------------------------");
            Log.LogInfo("PlayerMouthRelease");
            _TrueGear.Play("PlayerMouthRelease");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PawStatePoser), "OpenClaw")]
        private static void PawStatePoser_OpenClaw_Postfix(PawStatePoser __instance)
        {
            if (__instance.Controller == VRController.Left)
            {
                Log.LogInfo("-------------------------------------------");
                Log.LogInfo("LeftClawOpened");
                _TrueGear.Play("LeftClawOpened");
                isOpenLeftClaw = true;
            }
            else if (__instance.Controller == VRController.Right)
            {
                Log.LogInfo("-------------------------------------------");
                Log.LogInfo("RightClawOpened");
                _TrueGear.Play("RightClawOpened");
                isOpenRightClaw = true;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PawStatePoser), "CloseClaw")]
        private static void PawStatePoser_CloseClaw_Postfix(PawStatePoser __instance)
        {
            if (__instance.Controller == VRController.Left && isOpenLeftClaw)
            {
                Log.LogInfo("-------------------------------------------");
                Log.LogInfo("LeftClawClosed");
                _TrueGear.Play("LeftClawClosed");
                isOpenLeftClaw = false;
            }
            else if (__instance.Controller == VRController.Right && isOpenRightClaw)
            {
                Log.LogInfo("-------------------------------------------");
                Log.LogInfo("RightClawClosed");
                _TrueGear.Play("RightClawClosed");
                isOpenRightClaw = false;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(PawStatePoser), "SetPawPoseTouch")]
        private static void PawStatePoser_SetPawPoseTouch_Postfix(PawStatePoser __instance)
        {
            if (__instance._touchCurrent == "PawTear")
            {
                if (__instance.Controller == VRController.Left)
                {
                    Log.LogInfo("-------------------------------------------");
                    Log.LogInfo("LeftPawTear");
                    _TrueGear.Play("LeftPawTear");
                }
                else if (__instance.Controller == VRController.Right)
                {
                    Log.LogInfo("-------------------------------------------");
                    Log.LogInfo("RightPawTear");
                    _TrueGear.Play("RightPawTear");
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Edible), "Eat")]
        private static void Edible_Eat_Postfix(Edible __instance)
        {
            Log.LogInfo("-------------------------------------------");
            Log.LogInfo("Eat");
            _TrueGear.Play("Eat");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Grabber), "Grab")]
        private static void Grabber_Grab_Postfix(Grabber __instance, SurfaceType surfaceType)
        {
            if (surfaceType == SurfaceType.CatWatch)
            {
                if (__instance.PawStatePoser.Controller == VRController.Left)
                {
                    Log.LogInfo("-------------------------------------------");
                    Log.LogInfo("LeftPawTouchCatWatch");
                    _TrueGear.Play("LeftPawTouchCatWatch");
                }
                else if (__instance.PawStatePoser.Controller == VRController.Right)
                {
                    Log.LogInfo("-------------------------------------------");
                    Log.LogInfo("RightPawTouchCatWatch");
                    _TrueGear.Play("RightPawTouchCatWatch");
                }
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CatDeathAnimator), "AnimateDeath")]
        private static void CatDeathAnimator_AnimateDeath_Postfix(CatDeathAnimator __instance)
        {
            Log.LogInfo("-------------------------------------------");
            Log.LogInfo("PlayerDeath");
            _TrueGear.Play("PlayerDeath");
        }

        [HarmonyPostfix, HarmonyPatch(typeof(VRInputSystem), "PlayHapticImpulse")]
        private static void VRInputSystem_PlayHapticImpulse_Postfix(VRInputSystem __instance, float amplitude, float duration, ActionBasedController controller)
        {
            Log.LogInfo("-------------------------------------------");
            Log.LogInfo("PlayHapticImpulse");
            if ((amplitude == 0.1f && duration == 0.08f) || (amplitude == 0f && duration == 0.1f))
            {
                if (controller == __instance.LeftController)
                {
                    Log.LogInfo("LeftPawTouch");
                    _TrueGear.Play("LeftPawTouch");
                }
                else if (controller == __instance.RightController)
                {
                    Log.LogInfo("RightPawTouch");
                    _TrueGear.Play("RightPawTouch");
                }
            }
            else if (amplitude == 0.15f && duration == 0.35f)
            {
                Log.LogInfo("PlayerCrash");
                _TrueGear.Play("PlayerCrash");
            }
            else if (amplitude == 0.05f && duration == 0.1f)
            {
                if (controller == __instance.LeftController)
                {
                    Log.LogInfo("LeftPawGrab");
                    _TrueGear.Play("LeftPawGrab");
                }
                else if (controller == __instance.RightController)
                {
                    Log.LogInfo("RightPawGrab");
                    _TrueGear.Play("RightPawGrab");
                }
            }
            else if (amplitude != 0.1f && duration == 0.1f)
            {
                if (controller == __instance.LeftController)
                {
                    Log.LogInfo("LeftPawTouchItem");
                    _TrueGear.Play("LeftPawTouchItem");
                }
                else if (controller == __instance.RightController)
                {
                    Log.LogInfo("RightPawTouchItem");
                    _TrueGear.Play("RightPawTouchItem");
                }
            }

            Log.LogInfo(amplitude);
            Log.LogInfo(duration);
            Log.LogInfo(controller == __instance.LeftController);
            Log.LogInfo(controller == __instance.RightController);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Balloon), "OnCollisionEnter")]
        private static void Balloon_OnCollisionEnter_Postfix(Balloon __instance, Collision other)
        {
            try
            {
                if (other.rigidbody.name == "LeftHand")
                {
                    Log.LogInfo("-------------------------------------------");
                    Log.LogInfo("LeftPawBoomBalloon");
                    _TrueGear.Play("LeftPawBoomBalloon");
                }
                else if (other.rigidbody.name == "RightHand")
                {
                    Log.LogInfo("-------------------------------------------");
                    Log.LogInfo("RightPawBoomBalloon");
                    _TrueGear.Play("RightPawBoomBalloon");
                }
            }
            catch
            { 
                
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ClawLineDrawer), "OnUpdate")]
        private static void ClawLineDrawer_OnUpdate_Postfix(ClawLineDrawer __instance )
        {
            if (!__instance.IsDrawing)
            {
                return;
            }
            if (__instance._pawStatePoser.Controller == VRController.Left)
            {
                Log.LogInfo("-------------------------------------------");
                Log.LogInfo("LeftPawTear");
                _TrueGear.Play("LeftPawTear");
            }
            else if (__instance._pawStatePoser.Controller == VRController.Right)
            {
                Log.LogInfo("-------------------------------------------");
                Log.LogInfo("RightPawTear");
                _TrueGear.Play("RightPawTear");
            }
        }







    }
}
