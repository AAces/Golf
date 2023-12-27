using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddsIffZerosStrategy : Strategy
{
    private string title = "Adds If Zeros";

    /*     Will Take a Card if (in this priority):
     *     > It zeros a column
     *     > It is worth fewer points than the existing card
     *     > It has an empty column
     *     > If it is the last turn or the deck has been reshuffled, it takes a card if it is worth fewer than a six.
     */

    public override string getName()
    {
        return title;
    }

    public override (int, int) initalFlips(Card firstCard, bool firstPlayer)
    {
        return (0, 2);
    }

    public override int turnStart(int me, Card discardedCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands)
    {
        var card = discardedCard;
        var myHand = hands[me];
        var myCards = myHand.getVisibleCardsArray();
        var returnValue = -1;

        for(int i = 0; i<3; i++)
        {
            if (myCards[2*i]==null && myCards[2 * i+1] == null)
            {
                returnValue = 2*i;
            }
        }

        if (deckReshuffled||lastTurn)
        {
            for (int i = 0; i < 3; i++)
            {
                if (myCards[2 * i] != null && myCards[2*i+1]==null)
                {
                    if (myHand.testColumnScore(myCards[2*i], card) < myCards[2 * i].getRawValue() + 6)
                    {
                        returnValue = 2*i+1;
                    }
                } else if(myCards[2 * i + 1] != null && myCards[2 * i] == null)
                {
                    if (myHand.testColumnScore(myCards[2 * i+1], card) < myCards[2 * i+1].getRawValue() + 6)
                    {
                        returnValue = 2 * i;
                    }
                }
            }
        }

        for(int i = 0; i < 6; i++)
        {
            if (myCards[i] != null)
            {
                if (myCards[i].getRawValue() > card.getRawValue())
                {
                    var comp = i % 2 == 0 ? i + 1 : i - 1;
                    if (myCards[comp] == null)
                    {
                        returnValue = i;
                    }
                    else if (myCards[comp].getRank() != myCards[i].getRank())
                    {
                        returnValue = i;
                    }
                }
            }
        }

        for(int i = 0; i < 6; i++) {
            if (myCards[i] != null)
            {
                if (myCards[i].getRank() == card.getRank())
                {
                    var comp = i % 2 == 0 ? i + 1 : i - 1;
                    if (myCards[comp] == null)
                    {
                        returnValue = comp;
                    } else if (myCards[comp].getRank() != card.getRank())
                    {
                        returnValue = comp;
                    }
                }
            }
        }

        return returnValue;
    }

    public override int turnGivenFlip(int me, Card pulledCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands)
    {
        var card = pulledCard;
        var myHand = hands[me];
        var myCards = myHand.getVisibleCardsArray();
        var returnValue = -1;

        for (int i = 0; i < 3; i++)
        {
            if (myCards[2 * i] == null && myCards[2 * i + 1] == null)
            {
                returnValue = 2 * i;
            }
        }

        if (deckReshuffled||lastTurn)
        {
            for (int i = 0; i < 3; i++)
            {
                if (myCards[2 * i] != null && myCards[2 * i + 1] == null)
                {
                    if (myHand.testColumnScore(myCards[2 * i], card) < myCards[2 * i].getRawValue() + 6)
                    {
                        returnValue = 2 * i + 1;
                    }
                }
                else if (myCards[2 * i + 1] != null && myCards[2 * i] == null)
                {
                    if (myHand.testColumnScore(myCards[2 * i + 1], card) < myCards[2 * i + 1].getRawValue() + 6)
                    {
                        returnValue = 2 * i;
                    }
                }
            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (myCards[i] != null)
            {
                if (myCards[i].getRawValue() > card.getRawValue())
                {
                    var comp = i % 2 == 0 ? i + 1 : i - 1;
                    if (myCards[comp] == null)
                    {
                        returnValue = i;
                    }
                    else if (myCards[comp].getRank() != myCards[i].getRank())
                    {
                        returnValue = i;
                    }
                }
            }
        }

        for (int i = 0; i < 6; i++)
        {
            if (myCards[i] != null)
            {
                if (myCards[i].getRank() == card.getRank())
                {
                    var comp = i % 2 == 0 ? i + 1 : i - 1;
                    if (myCards[comp] == null)
                    {
                        returnValue = comp;
                    }
                    else if (myCards[comp].getRank() != card.getRank())
                    {
                        returnValue = comp;
                    }
                }
            }
        }

        return returnValue;
    }
}
