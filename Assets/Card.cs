using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Easier to manage an object. Contains helpful functions for debug formatting and score calculation
// That can be handled by the Card object, and not as a static function
public class Card 
{
    private int suit, rank;

    private bool mobileFormatting;

    private Card[][] list;

    public Card(int suit, int rank, bool mobileFormatting)
    {
        this.suit = suit; //0=spades ♠, 1=hearts ♥, 2=diamonds ♦, 3=clubs ♣
        this.rank = rank;
        this.mobileFormatting = mobileFormatting;
    }

    public void setList(Card[][] l)
    {
        this.list = l;
    }

    public string asString()
    {
        var r = "";
        var s = "";

        if (rank == 1)
        {
            r = "A";
        }
        else if (rank < 11)
        {
            r = rank.ToString();
        }
        else
        {
            switch (rank)
            {
                case 11:
                    r = "J";
                    break;
                case 12:
                    r = "Q";
                    break;
                case 13:
                    r = "K";
                    break;
            }
        }

        switch (suit)
        {
            case 0:
                s = mobileFormatting ? "S" : "♠";
                break;
            case 1:
                s = mobileFormatting ? "H" : "♥";
                break;
            case 2:
                s = mobileFormatting ? "D" : "♦";
                break;
            case 3:
                s = mobileFormatting ? "C" : "♣";
                break;
        }

        return r + s;
    }

    public int getRawValue() // Point values as determined by the rules of the game
    {
        switch (rank)
        {
            case 2:
                return -2;
            case 13:
                return 0;
            case 11:
            case 12:
                return 10;
            default:
                return rank;
        }
    }

    public bool equals(Card c)
    {
        return c.getRank() == this.getRank() && c.getSuit() == this.getSuit();
    }

    public int getRank()
    {
        return this.rank;
    }

    public int getSuit()
    {
        return this.suit;
    }

}