using System;
using System.Collections.Generic;
using System.Linq;

namespace deck_of_cards
{
    public class Card{
        public string stringVal;
        public string suit;
        public int val;
        public Card(string stringValue, string cardSuit, int cardValue){
            stringVal = stringValue;
            suit = cardSuit;
            val = cardValue;
        }
    }

    public class CardDeck{ //can change name back to Deck later
        public List<Card> cards;
        public List<Card> resetRef; 
        public bool init(){
            bool isUnique = (cards.Distinct().Count() == cards.Count());
            resetRef = cards; //where to set resetRef? here seems okay for now.
            return isUnique;
        } //not sure what to do with this per insturctions? e.g. if false return...
        public Card deal(){
            Card selectedCard = cards[0];
            cards.Remove(selectedCard);
            return selectedCard;
        }
        public void reset(){
            cards = resetRef; //sets deck back to list from init()
        } 
        public void shuffle(){ //not so efficient shuffle function... I hope it works?
            List<Card> refDeck = cards;
            List<Card> newDeck = cards;
            int check = 0;
            Random rand = new Random();
            for (int i = 0; i < refDeck.Count; i++){
                int g = rand.Next(0, refDeck.Count);
                Card selected = refDeck[g];
                for (int j = 0; j < i; j++){
                    Card refCard = newDeck[j];
                    if (selected == refCard){
                        i--;
                    }
                    else{
                        continue;
                    }
                }
                if (check == i){
                    newDeck[i] = selected;
                    check++; 
                }
            }
            cards = newDeck;
        }
    }
    public class Player{
        public string name;
        public List<Card> hand;
        public Player(string nameEnt){
            name = nameEnt;
        }

        public void Draw(CardDeck deck){
            Card newCard = deck.deal();
            hand.Add(newCard);
            //draws card from deck, adds it to the players hand and returns the card - ref deck
        }
        public void Discard(int i){
            Card selectedCard = hand[i];
            if(selectedCard != null){
                hand.Remove(selectedCard);
            }
        }
    }
}