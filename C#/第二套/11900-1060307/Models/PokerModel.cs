namespace PokerGame.Models;

public enum Suit
{
    Spades = 0,   // ♠
    Hearts = 1,   // ♥
    Diamonds = 2, // ♦
    Clubs = 3     // ♣
}

public enum Rank
{
    Ace = 0,
    Two = 1,
    Three = 2,
    Four = 3,
    Five = 4,
    Six = 5,
    Seven = 6,
    Eight = 7,
    Nine = 8,
    Ten = 9,
    Jack = 10,
    Queen = 11,
    King = 12
}

public record Card(int CardNumber)
{
    public Suit Suit => (Suit)(CardNumber / 13);
    public Rank Rank => (Rank)(CardNumber % 13);

    public string SuitSymbol => Suit switch
    {
        Suit.Spades => "♠",
        Suit.Hearts => "♥",
        Suit.Diamonds => "♦",
        Suit.Clubs => "♣",
        _ => "?"
    };

    public string RankDisplay => Rank switch
    {
        Rank.Ace => "A",
        Rank.Jack => "J",
        Rank.Queen => "Q",
        Rank.King => "K",
        _ => ((int)Rank + 1).ToString()
    };

    // Ranking value for comparison: A(14) > K(13) > Q(12) > J(11) > 10(10) ... > 2(2)
    public int PowerValue => Rank switch
    {
        Rank.Ace => 14,
        _ => (int)Rank + 1
    };

    public override string ToString() => $"{SuitSymbol}{RankDisplay}";
}

public record RoundResult(int RoundIndex, Card PlayerCard, Card BankerCard, string Result);
