﻿using Steamworks;

namespace F.Economy.Models
{
    public class Account
    {
        public string _id { get; set; }
        public int _money { get; set; }

        public Account(CSteamID steamID, int playermonney)
        {
            _id = steamID.m_SteamID.ToString();
            _money = playermonney;
        }

        public Account()
        {

        }
    }
}

