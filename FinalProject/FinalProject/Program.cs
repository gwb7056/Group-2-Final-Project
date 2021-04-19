using System;

/// <summary>
/// This game is called Magic: The Towering. 
/// It is a Tower Defense. 
/// The goal is to defend the objective from enemies by summoning defenses (i.e. towers and spells). 
/// The game is organized into a 15 x 15 board (think of it as a chess board). 
/// Enemies move along a coffee colored path that trails from the left side of the board to the right side of the board where the objective is. 
/// Defenses can be placed on the green spaces not marked by an X. 
/// This chess board layout ensures that all entities are placed in a very organized and cohesive manner. 
/// The player has a hand which is a set of cards he is restricted to playing. 
/// If the player needs more cards in his hand after playing some, the Mana System will draw cards from the deck to place in their hand. 
/// After the player plays a card, that card goes in a discard pile. 
/// Eventually, the deck will run out of cards to draw from. 
/// This is where the discard pile comes in. 
/// The discard pile is shuffled and every card that has been discarded will now be pushed onto the deck again. 
/// The deck fills up and the player can continue drawing cards onto their hand to play. 
/// </summary>
namespace FinalProject 
{
    public static class Program 
    {
        [STAThread]
        static void Main() {
            using (var game = new Game1())
                game.Run();
        }
    }
}
