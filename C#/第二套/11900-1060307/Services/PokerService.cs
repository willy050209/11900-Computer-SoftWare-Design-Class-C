namespace PokerGame.Services;

using PokerGame.Models;

public interface IPokerService
{
    RoundResult PlayRound(int roundIndex, double playerRandom, double bankerRandom);
}

public class PokerService : IPokerService
{
    public RoundResult PlayRound(int roundIndex, double playerRandom, double bankerRandom)
    {
        int playerCardNum = (int)(playerRandom * 52);
        int bankerCardNum = (int)(bankerRandom * 52);

        // Requirement: If same card is picked, player picks the next available random number?
        // PDF p19: "但須注意同一張牌不可發出兩次或兩次以上，遇取得之牌張已發出時，則捨棄該牌張，重新另取一張。"
        // Since we are given a fixed sequence of random numbers from a file, if bankerCardNum == playerCardNum, 
        // we should conceptually take the *next* random number from the file.
        // This implies the PlayRound logic needs to be handled by the Presenter who reads the file.

        var playerCard = new Card(playerCardNum);
        var bankerCard = new Card(bankerCardNum);

        string result = "平手";
        if (playerCard.PowerValue > bankerCard.PowerValue)
        {
            result = "玩家贏";
        }
        else if (playerCard.PowerValue < bankerCard.PowerValue)
        {
            result = "莊家贏";
        }

        return new RoundResult(roundIndex, playerCard, bankerCard, result);
    }
}
