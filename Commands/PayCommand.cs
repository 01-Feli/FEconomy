﻿using Rocket.API;
using F.Economy.Database;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using Rocket.Unturned.Chat;

namespace F.Economy.Commands
{
    public class PayCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "pay";

        public string Help => string.Empty;

        public string Syntax => "/pay <player> <ammount>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>()
        {
            "f.pay"
        };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            switch (command.Length)
            {
                case 1:
                    UnturnedChat.Say(caller, Syntax);
                    break;
                case 2:
                    UnturnedPlayer player2 = UnturnedPlayer.FromName(command[0]);
                    if (player2 != null)
                    {
                        if (player2.Id == player.Id)
                        {
                            UnturnedChat.Say(caller, Economy.Instance.Translate("pay_nopoint"));
                        }
                        else
                        {
                            int money = Convert.ToInt32(command[1]);
                            if (money > 0)
                            {
                                if (Economy.Instance.Configuration.Instance.XpMode == false)
                                {
                                    if (money < EconomyDB.GetBalance(player) + 1)
                                    {
                                        EconomyDB.RemoveBalance(player, money);
                                        EconomyDB.AddBalance(player2, money);
                                        UnturnedChat.Say(caller, string.Format(Economy.Instance.Translate("pay_success"), money, Economy.Instance.Configuration.Instance.CurrencyName, player2.DisplayName));
                                        UnturnedChat.Say(player2, string.Format(Economy.Instance.Translate("pay_recieve"), money, Economy.Instance.Configuration.Instance.CurrencyName, player.DisplayName));
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, Economy.Instance.Translate("no_balance"));
                                    }
                                }
                                else
                                {
                                    if (money < player.Experience + 1)
                                    {
                                        player.Experience = player.Experience - (uint)money;
                                        player2.Experience = player.Experience + (uint)money;
                                        UnturnedChat.Say(caller, string.Format(Economy.Instance.Translate("xppay_success"), money, player2.DisplayName));
                                        UnturnedChat.Say(player2, string.Format(Economy.Instance.Translate("xppay_recieve"), money, player.DisplayName));
                                        Logger.Log(string.Format(Economy.Instance.Translate("c_pay_success"), money, player2.DisplayName, caller.DisplayName));
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, Economy.Instance.Translate("no_balance"));
                                    }
                                }
                            }
                            else
                            {
                                UnturnedChat.Say(caller, Economy.Instance.Translate("err_ammount"));
                            }
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(caller, Syntax);
                    }
                    break;
                default:
                    UnturnedChat.Say(caller, Syntax);
                    break;
            }
        }
    }
}

