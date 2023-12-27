using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A class to manage each player's hand, instead of needing a bunch of static arrays in the main class
public class Hand
{
    private List<int> cardsVisible;

    private Card[][] allCards;

    /*       Card List order:
     *       0  2  4
     *       1  3  5
     */

    private Card[] cards;

    public Hand() // Constructor if using a real deck
    {
        cardsVisible = new List<int>();
        cards = new Card[6] { null, null, null, null, null, null };
    }

    public Hand(Card[] cards) // Constructor if the game is managing the deck
    {
        this.cards = cards;
        cardsVisible = new List<int>();
    }

    public List<int> getVisibleCardsList() { return cardsVisible; }

    public Card[] getVisibleCardsArray()
    {
        //Done this way so that the returned object is not the same object in memory
        var output = new Card[6];
        for (int i = 0; i < 6; i++)
        {
            if (cardsVisible.Contains(i))
            {
                output[i] = cards[i];
            } else
            {
                output[i] = null;
            }
        }
        return output;
    }

    public Card flipCard(int index) // Flip a card (if the game is managing the deck)
    {
        cardsVisible.Add(index);
        return cards[index];
    }

    public void flipCard(int index, Card card) // Flip a card (if using a real deck)
    {
        cardsVisible.Add(index);
        cards[index] = card;
    }

    public Card placeCard(int index, Card card) // Play a card
    {
        if (!cardsVisible.Contains(index))
        {
            cardsVisible.Add(index);
        }
        var temp = cards[index];
        cards[index] = card;
        return temp;
    }

    public int getVisibleScore()
    {
        var c = getVisibleCardsArray();
        return getColumnScore(0, c) + getColumnScore(1, c) + getColumnScore(2, c);
    }

    public int getScore() // This will count unseen cards if the game is managing the deck
    {   
        return getColumnScore(0, cards)+getColumnScore(1, cards)+getColumnScore(2, cards);
    }

    public int getColumnScore(int column, Card[] list)
    {
        if (column < 0 || column > 2) return -100;
        var card1 = list[column * 2];
        var card2 = list[column * 2 + 1];


        // If one or both card is unknown, just return the value of the visible card, unless both are unknown, in which case return 0
        if(card1==null && card2 == null)
        {
            return 0;
        } else if (card1 == null)
        {
            return card2.getRawValue();
        } else if (card2 == null)
        {
            return card1.getRawValue();
        }

        // Checks if the two cards are equal in rank
        if (card1.getRank() == card2.getRank())
        {
            return 0;
        }

        // Otherwise, returns the sum of the individual values of the cards
        return card1.getRawValue() + card2.getRawValue();
    }

    public int testColumnScore(Card card1, Card card2) // Will return the value of a column containing the supplied cards
    {
        if (card1 == null && card2 == null)
        {
            return 0;
        }
        else if (card1 == null)
        {
            return card2.getRawValue();
        }
        else if (card2 == null)
        {
            return card1.getRawValue();
        }

        if (card1.getRank() == card2.getRank())
        {
            return 0;
        }
        return card1.getRawValue() + card2.getRawValue();
    }

    public void setList(Card[][] l)
    {
        this.allCards = l;
    }

    public Card[] getCards()
    {
        return this.cards;
    }

}