using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;
using System;
using System.Collections.Generic;

namespace DutyNotif
{
    
    public class Class1 : Plugin
    {
        public static bool duty = false;
        internal Class1()
        {
            EventHandlers["Kilo::DutyNotif"] += DutyNotif;
        }
        private void DutyNotif(bool duty)
        {
            Debug.WriteLine(duty.ToString());
            if (duty)
            {
                PlayerData plrData = Utilities.GetPlayerData();
                Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "STRING");
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "[" + plrData.Callsign + "] " + plrData.DisplayName + " ~g~i tjanst");
                Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, "CHAR_CALL911", "CHAR_CALL911", false, 0, "Polismyndigheten", "~f~Status");
                Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, false, true);
                Utilities.SetPlayerDuty(duty);
            } else
            {
                PlayerData plrData = Utilities.GetPlayerData();
                Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "STRING");
                Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, "[" + plrData.Callsign + "] " + plrData.DisplayName + " ~r~ledig");
                Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, "CHAR_CALL911", "CHAR_CALL911", false, 0, "Polismyndigheten", "~f~Status");
                Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, false, true);
                Utilities.SetPlayerDuty(duty);
            }
        }
    }

    public class Command : BaseScript
    {
        public Command()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);

        }

        private void OnClientResourceStart(string resource)
        {
            if (API.GetCurrentResourceName() != resource) return;
            API.RegisterCommand("duty", new Action<int, List<object>, string>((source, args, raw) =>
            {
                
                if (!DutyNotif.Class1.duty)
                {
                    TriggerEvent("Kilo::DutyNotif", true);
                    Class1.duty = true;
                    Debug.WriteLine("Setting duty true");
                } else
                {
                    TriggerEvent("Kilo::DutyNotif", false);
                    Class1.duty = false;
                    Debug.WriteLine("Setting duty false");
                }
                
            }),false);

        }
    }
}