using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EveStrategy : Strategy
{
    private string title = "Eve's Strategy";

    /*    Will Take a Card if:
     *    > 
     */

    public override string getName()
    {
        return title;
    }

    // Always flip a card in different columns
    public override (int, int) initalFlips(Card firstCard, bool firstPlayer) 
    {
        return (0, 2);
    }

    public override int turnStart(int me, Card card, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands)
    {
        var myHand = hands[me];
        var myCards = myHand.getVisibleCardsArray();
        var rank = card.getRank();
        var returnValue = -1;

        if(rank == 2 || rank == 13)
        {

        } else
        {
            for (int i = 0; i < 6; i++)
            {
                if (myCards[i] != null)
                {
                    if (myCards[i].getRank() == rank)
                    {
                        var comp = i % 2 == 0 ? i + 1 : i - 1;
                        if (myCards[comp] == null)
                        {
                            returnValue = comp;
                        }
                        else if (myCards[comp].getRank() != rank)
                        {
                            
                        }
                    }
                }
            }
        }

        if(lastTurn)
        {

        } 
        else
        {

        }




        return returnValue;
    }

    public override int turnGivenFlip(int me, Card pulledCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands)
    {




        return -1;
    }
}
