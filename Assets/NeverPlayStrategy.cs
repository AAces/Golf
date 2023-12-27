using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeverPlayStrategy : Strategy
{
    private string title = "Bad Strategy";

    /*     Will Take a Card if (in this priority):
     *     > Never. This strategy always draws a card, and then discards it immediately.
     */

    public override string getName()
    {
        return title;
    }

    public override (int, int) initalFlips(Card firstCard, bool firstPlayer)
    {
        return (0, 5);
    }

    public override int turnStart(int me, Card discardedCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands)
    {
        return -1;
    }

    public override int turnGivenFlip(int me, Card pulledCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands)
    {
        return -1;
    }
}
