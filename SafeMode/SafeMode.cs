using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using FrooxEngine.UIX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BaseX;
using FrooxEngine.LogiX;

namespace SafeMode
{
    public class SafeMode : NeosMod
    {
        public override string Name => "SafeMode";
        public override string Author => "TheJebForge";
        public override string Version => "1.0.1";

        [AutoRegisterConfigKey]
        readonly ModConfigurationKey<bool> ENABLED = new ModConfigurationKey<bool>("enabled","Safe Mode enabled", () => false);

        private ModConfiguration config;
        private static SafeMode modInstance;

        bool IsModEnabled() => config.GetValue(ENABLED);

        void DisableMod() => config.Set(ENABLED, false);
        
        public override void OnEngineInit()
        {
            modInstance = this;
            config = GetConfiguration();
            
            Harmony harmony = new Harmony($"net.{Author}.{Name}");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(WorldManager), nameof(WorldManager.FocusWorld))]
        class DisableOnSessionFocus
        {
            public static void Prefix()
            {
                modInstance.DisableMod();
            }
        }

        [HarmonyPatch(typeof(Impulse), nameof(Impulse.Trigger))]
        class ImpulseTrigger
        {
            public static bool Prefix()
            {
                return !modInstance.IsModEnabled();
            }
        }
    }
}