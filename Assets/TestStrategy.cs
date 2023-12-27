using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStrategy : Strategy
{

    private string title = "Test Strategy";

    /*    Will Take a Card if:
     *    > Always takes a card. Alternates between the discard pile and the draw pile with
     *      no regard for its existing cards. It just puts the card in the next open spot.
     */

    public override string getName()
    {
        return title;
    }

    public override (int, int) initalFlips(Card firstCard, bool firstPlayer)
    {
        return (0, 1);
    }

    public override int turnStart(int me, Card discardedCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands)
    {
        int first = 0;
        var myCards = hands[me].getVisibleCardsList();
        for(int i = 0; i<6; i++)
        {
            if (!myCards.Contains(i))
            {
                first = i;
                break;
            }
        }
        if(first%2==0)
        {
            return first;
        } else
        {
            return -1;
        }
    }

    public override int turnGivenFlip(int me, Card pulledCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands)
    {
        var myCards = hands[me].getVisibleCardsList();
        for (int i = 0; i < 6; i++)
        {
            if (!myCards.Contains(i))
            {
                return i;
            }
        }
        return -1;
    }


}
