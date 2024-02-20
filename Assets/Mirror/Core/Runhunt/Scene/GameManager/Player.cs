using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerTeam
{
    Runner, Hunter
}

public enum PlayerState
{
    Win, Lose, Playing
}

public class Player
{

    public string m_name { get; private set; }

    public PlayerTeam m_team { get; private set; }

    public PlayerState m_state { get; private set; }

    public Player(string name, PlayerTeam team)
    {
        m_name = name;
        m_team = team;
        m_state = PlayerState.Playing;
    }

    public Player(string name, string team)
    {
        PlayerTeam output;
        m_name = name;
        Enum.TryParse<PlayerTeam>(team, true, out output);
        m_team = output;
        m_state = PlayerState.Playing;
    }

    public void SetState(PlayerState state)
    {
        m_state = state;
    }

}
