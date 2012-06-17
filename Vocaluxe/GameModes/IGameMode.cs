﻿using System;
using System.Collections.Generic;
using System.Text;

using Vocaluxe.Base;
using Vocaluxe.Lib.Song;

namespace Vocaluxe.GameModes
{
    public enum EGameMode
    {
        TR_GAMEMODE_NORMAL,
        TR_GAMEMODE_MEDLEY,
        TR_GAMEMODE_DUET,
        TR_GAMEMODE_SHORTSONG
    }

    interface IGameMode
    {
        void Init();

        EGameMode GetGameMode();

        bool AddVisibleSong(int VisibleIndex);
        bool AddSong(int AbsoluteIndex);
        bool RemoveVisibleSong(int VisibleIndex);
        bool RemoveSong(int AbsoluteIndex);
        void ClearSongs();

        void Reset();
        void Start(SPlayer[] Player);
        void NextRound(SPlayer[] Player);
        bool IsFinished();
        int GetActualRoundNr();
        CSong GetSong();
        CPoints GetPoints();

        int GetNumSongs();
        CSong GetSong(int Num);
    }
}
