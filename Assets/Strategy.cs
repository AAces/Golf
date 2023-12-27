using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Any class wishing to be runnable as a strategy must inherit this class
public abstract class Strategy : MonoBehaviour 
{

    /**
     * Parameters:
     * 
     * Card firstCard: The card at the top of the discard pile, flipped over after the hands were dealt
     * bool firstPlayer: Whether or not this player is going first (i.e., will have a chance at the flipped card)
     * 
     * Returns:
     * 
     * Two indicies corresponding to which two cards should be flipped. 
     * 
     */
    public abstract (int,int) initalFlips(Card firstCard, bool firstPlayer);


    /**
     * Paramaters:
     * 
     * int me: The player's index.
     * Card discardedCard: The top card of the discard pile.
     * List<Card> discardedCards: The entire discard pile (i.e., memory of which cards have been played this game).
     * bool lastTurn: Whether or not this is the final turn of the game.
     * bool deckReshuffled: Whether or not the deck has been reshuffled this game.
     * Hand[] hands: All of the players' hands. In the event that the game is managing the deck, this would technically allow
     *               cheating. If using a real deck, there is no issue with this. Using only getVisibleCardsList() and
     *               getVisibleCardsArray() mitigates this potential issue.
     * 
     * Returns:
     * 
     * -1: Indicates not to use the top card of the discard pile. The game will then flip the top card of the deck and supply 
     * it to the player, calling turnGivenFlip().
     * 
     * An integer between 0 and 5 (incl): Indicates both that the player would like to use the top card of
     * the discard pile, and where to place it.
     */
    public abstract int turnStart(int me, Card discardedCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands);


    /**
     * Parameters:
     * 
     * Same as above except the following:
     * Card pulledCard: The card drawn off the top of the discard pile.
     * 
     * Returns:
     * 
     * -1: Discard the drawn card.
     * 
     * An integer between 0 and 5 (incl): Play the drawn card in the returned index.
     */
    public abstract int turnGivenFlip(int me, Card pulledCard, List<Card> discardedCards, bool lastTurn, bool deckReshuffled, Hand[] hands);


    /// Should just return the display name of the strategy. Will be used in debug messages and (eventually) on-screen labels.
    public abstract string getName();

}
