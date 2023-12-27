using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour
{

    public Button exitButton;

    private Card discardedCard;

    private Card[][] cards;

    private Hand[] aiHands;

    private List<Card> deck, discardPile;

    public bool mobileFormatting, aiAuto, randomFirstPlayer, extraLogging;

    private bool deckReshuffled, finalRound;

    public Strategy[] strategies;

    private int turn, players, aiGameCounter, tieCounter, cardsInDeck, activePlayer, lastPlayer;

    private int[] aiVictories;

    public int aiGamesToPlay;

    public float gapTime;

    private float[] aiAverageScores;


    void Start()
    {
        init();
    }

    void init()
    {
        // Setup
        setupCards();
        cardsInDeck = 52;
        discardPile = new List<Card>();
        activePlayer = 0;
        finalRound = false;
        deckReshuffled = false;

        
        if (aiAuto)
        {
            players = strategies.Length;
            if (players<2) { Application.Quit(); }

            // Instantiate the deck
            deck = new List<Card> ();
            foreach(var l in cards)
            {
                foreach(var c in l)
                {
                    deck.Add(c);
                }
            }

            // Shuffle the deck
            deck.Shuffle();
            //printDeck();

            // Debug player logging
            Debug.Log("There are " + players + " AI players. They are: ");
            for(int i =0; i<players; i++)
            {
                Debug.Log("Player " + i + ": " + strategies[i].getName());
            }


            // Setup the stat trackers
            aiVictories = new int[players];
            aiGameCounter = 1;
            tieCounter = 0;
            aiAverageScores = new float[players];
            
            // Account for the dealt cards
            cardsInDeck -= 6 * players;
            
            // Deal the cards
            aiHands = new Hand[players];
            for (int i = 0; i < players; i++)
            {
                aiAverageScores[i] = 0f;
                aiVictories[i] = 0;
                aiHands[i] = new Hand(new Card[6] { deck[i * 6 + 0], deck[i * 6 + 1], deck[i * 6 + 2], deck[i * 6 + 3], deck[i * 6 + 4], deck[i * 6 + 5] });
            }

            // Remove the dealt cards from the deck
            for(int i = 0; i<players*6; i++)
            {
                deck.RemoveAt(0);
            }

            // Flip over the top card
            cardsInDeck--;
            discardedCard = deck[0];
            discardPile.Add(discardedCard);
            deck.RemoveAt(0);

            // Flip over the intitial cards
            for(int i = 0; i<players; i++)
            {
                (int f, int l) flips = strategies[i].initalFlips(discardedCard, i==activePlayer);

                aiHands[i].flipCard(flips.f);
                aiHands[i].flipCard(flips.l);
            }


            // Start the game(s)
            StartCoroutine(aiAutoTurn());
        } else
        {
            // TODO: Add a way to play against one of the strategies
        }

    }

    IEnumerator aiAutoTurn()
    {

        if (finalRound && activePlayer == lastPlayer)
        {
            // End the game
            yield return new WaitForSeconds(gapTime);
            resetBetweenAiRounds();
            yield break;

        }
        else
        {
            // Ask the current player for its action
            int action = strategies[activePlayer].turnStart(activePlayer, discardedCard, discardPile, finalRound, deckReshuffled, aiHands);

            if (action == -1) 
            {// The strategy asked for a card to be flipped
                var flippedCard = deck[0];
                deck.RemoveAt(0);
                cardsInDeck--;

                // Ask for its action given the flip
                int newAction = strategies[activePlayer].turnGivenFlip(activePlayer, flippedCard, discardPile, finalRound, deckReshuffled, aiHands);

                if (deck.Count == 0)
                {
                    // Reshuffle the deck
                    deck.AddRange(discardPile);
                    deck.Shuffle();
                    discardPile.Clear();
                    cardsInDeck = deck.Count;
                    deckReshuffled = true;
                }
                if (newAction == -1)
                {
                    // Discard the drawn card
                    discardedCard = flippedCard;
                    discardPile.Add(discardedCard);
                }
                else
                {
                    // Play the drawn card and discard the card it is replacing
                    discardedCard = aiHands[activePlayer].placeCard(newAction, flippedCard);
                    discardPile.Add(discardedCard);
                }
            }
            else
            {
                // Take the card on top of the discard pile and play it
                discardedCard = aiHands[activePlayer].placeCard(action, discardPile.Last());
                discardPile.Add(discardedCard);
                discardPile.RemoveAt(discardPile.Count - 2);
            }

            if (aiHands[activePlayer].getVisibleCardsList().Count == 6 && !finalRound)
            {
                // If the player has flipped over their last card, everyone gets one more turn
                finalRound = true;
                lastPlayer = activePlayer;
            }

            // Debug logging
            updateAiText();

            // Move to the next player
            yield return new WaitForSeconds(gapTime);
            activePlayer++;
            if (activePlayer >= players) { activePlayer = 0; }
            StartCoroutine(aiAutoTurn());

            yield break;
        }
    }

    void updateAiAverages()
    {
        for (int i = 0; i < players; i++)
        {
            if (aiGameCounter == 1)
            {
                aiAverageScores[i] = (float)aiHands[i].getScore();
            }
            else
            {
                // Calculate the players' average scores in a way that does not require adding them all up every time
                var oldAverage = aiAverageScores[i]; 
                aiAverageScores[i] = (oldAverage*(aiGameCounter-1)+ (float)aiHands[i].getScore())/aiGameCounter;
            }
        }
    }

    void printPlayerHand(int player)
    {
        // Print out the player's hand to the console
        var hand = aiHands[player].getVisibleCardsArray();
        Debug.Log("Player " + (player + 1) + "'s hand:");
        Debug.Log(cardFormatting(hand[0]) + " " + cardFormatting(hand[2]) + " " + cardFormatting(hand[4]));
        Debug.Log(cardFormatting(hand[1]) + " " + cardFormatting(hand[3]) + " " + cardFormatting(hand[5]));
        Debug.Log("Visible Score: " + aiHands[player].getVisibleScore());
    
    }

    string cardFormatting(Card c)
    {
        if(c == null)
        {
            return ("B"); // Not sure why I used 'B', to be honest. Probably for "blank"
        } else
        {
            return c.asString();
        }
    }

    void updateAiText()
    {
        //Shows the game stats. Clears between each game so that it stays still in the console
        ClearLog();
        for (int i = 0; i < players; i++)
        {
            if (extraLogging) { printPlayerHand(i); }
            Debug.Log("Player " + (i+1) + ": " + strategies[i].getName() + ". Win Count: " + aiVictories[i] + " ("+(aiGameCounter!=1? (100f * aiVictories[i] / (aiGameCounter - 1)).ToString().Truncate(5):"N") +"%). Average Score: " + aiAverageScores[i].ToString().Truncate(5) + "." + (extraLogging?" (" + aiHands[i].getVisibleCardsList().Count +"/6)" + (activePlayer==i?"<<":""):""));
        }
        Debug.Log("Ties: " + tieCounter + " (" + (aiGameCounter != 1 ? (100f * tieCounter / (aiGameCounter - 1)).ToString().Truncate(5) : "N") + "%).");
        Debug.Log("Game "+aiGameCounter+"/"+aiGamesToPlay+"."+(extraLogging?(" Cards in deck: " + cardsInDeck + "."):""));
        if (extraLogging) { Debug.Log("Just discarded: " + discardedCard.asString()); }
        
    }

    private void resetBetweenAiRounds()
    {
        // Everything required to restart the game between rounds;
        // Basically a copy of the init() funciton -- I probably could have done some consolidation tbh
        updateAiAverages();
        deck.Clear();
        discardPile.Clear();
        deck = new List<Card>();
        discardPile = new List<Card>();
        foreach (var l in cards)
        {
            foreach (var c in l)
            {
                deck.Add(c);
            }
        }

        deck.Shuffle();

        //Update the player stats
        var scores = new int[players];
        for(var i = 0; i < players; i++)
        {
            scores[i] = aiHands[i].getScore();
        }
        Array.Sort(scores);
        if (scores[0] == scores[1])
        {
            tieCounter++;
        } else
        {
            for (var i = 0; i < players; i++)
            {
                if (aiHands[i].getScore() == scores[0])
                {
                    aiVictories[i]++;
                    break;
                }
            }
        }
        updateAiText();
        aiGameCounter++;
        if (aiGameCounter > aiGamesToPlay)
        {
            return;
        }

        cardsInDeck = 52;
        cardsInDeck -= 6 * players;

        aiHands = new Hand[players];
        for (int i = 0; i < players; i++)
        {
            aiHands[i] = new Hand(new Card[6] { deck[i * 6 + 0], deck[i * 6 + 1], deck[i * 6 + 2], deck[i * 6 + 3], deck[i * 6 + 4], deck[i * 6 + 5] });
        }
        for (int i = 0; i < players * 6; i++)
        {
            deck.RemoveAt(0);
        }
        cardsInDeck--;
        discardedCard = deck[0];
        discardPile.Add(discardedCard);
        deck.RemoveAt(0);

        activePlayer = randomFirstPlayer ? UnityEngine.Random.Range(0, players) : 0;

        for (int i = 0; i < players; i++)
        {
            (int f, int l) flips = strategies[i].initalFlips(discardedCard, i == activePlayer);

            aiHands[i].flipCard(flips.f);
            aiHands[i].flipCard(flips.l);
        }
        
        finalRound = false;
        deckReshuffled = false;
        StartCoroutine(aiAutoTurn());
    }

    void printDeck()
    {
        // Used for debugging. Prints out every card in the deck, in order
        foreach(var c in deck)
        {
            Debug.Log(c.asString());
        }
    }

    void setupCards()
    {
        // Instantiates the card array. Copied from a previous project, where I coded a version of 500 Rummy.
        // I suppose the suit information is redundant for this game, but it is easier than having four copies of each card.

        cards = new Card[4][];
        for (var s = 0; s < 4; s++) //0=spades, 1=hearts, 2=diamonds, 3=clubs
        {
            cards[s] = new Card[13];
            for (var r = 0; r < 13; r++)
            {
                cards[s][r] = new Card(s, r + 1, mobileFormatting);
            }
        }

        foreach (var v in cards)
        {
            foreach (var c in v)
            {
                c.setList(cards);
            }
        }
    }

    public void ClearLog()
    {
        // Clears the console
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

}

public static class ThreadSafeRandom // Used in the Shuffle function
{
    [ThreadStatic] private static System.Random Local;

    public static System.Random ThisThreadsRandom 
    {
        get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
    }
}

static class MyExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public static class StringExt
{
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
}

