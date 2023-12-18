using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace adventofcode2023;

public class DaySeven
{
    public void SolvePartOne()
    {
        var lines = File.ReadAllLines("day7.txt");
        List<(List<char> cards, int bid)> plays = new List<(List<char> cards, int bid)>();

        foreach (var line in lines)
        {
            plays.Add(GetCardsAndBid(line));
        }

        var handsResults = new List<HandResult>();

        foreach (var (cards, bid) in plays)
        {
            (char[] cardsArray, HandType handType) = GetMatchesFromCards(cards);
            handsResults.Add(new HandResult(handType, cardsArray, bid));
        }

        var orderedResults = handsResults.OrderBy(x => x, new CardComparer()).ToList();
        int result = 0;
        for (int i = 0; i < orderedResults.Count; i++)
        {
            result += (i + 1) * orderedResults[i].Bid;
        }

        Console.WriteLine($"Result: {result}");
    }

    public void SolvePartTwo()
    {
        var lines = File.ReadAllLines("day7.txt");

        var plays = new List<(List<char>, int, int)>();
        foreach (var line in lines)
        {
            plays.Add(GetCardsBidAndJokersCount(line));
        }

        var handsResults = new List<HandResult>();

        foreach (var (cards, bid, jokersCount) in plays)
        {
            (char[] cardsArray, HandType handType) = GetMatchesFromCardsAndJokersCount(cards, jokersCount);
            handsResults.Add(new HandResult(handType, cardsArray, bid));
        }

        var orderedResults = handsResults.OrderBy(x => x, new CardComparerWithJoker()).ToList();
        int result = 0;
        for (int i = 0; i < orderedResults.Count; i++)
        {
            result += (i + 1) * orderedResults[i].Bid;
        }

        Console.WriteLine($"Result: {result}");
    }

    (List<char> cards, int bid) GetCardsAndBid(string line)
    {
        var playInfo = line.Split(" ");
        var cards = playInfo[0].ToCharArray();
        var bid = int.Parse(playInfo[1]);

        return (cards.ToList(), bid);
    }

    (List<char> cards, int bid, int jokersCount) GetCardsBidAndJokersCount(string line)
    {
        var playInfo = line.Split(" ");
        var cards = playInfo[0].ToCharArray();
        var bid = int.Parse(playInfo[1]);
        var jokersCount = cards.Count(x => x == 'J');

        return (cards.ToList(), bid, jokersCount);
    }

    (char[] strength, HandType handType) GetMatchesFromCards(List<char> cards)
    {
        var dict = GetDictionaryFromCards(cards);

        var groupedCards = dict.GroupBy(x => x.Value).OrderByDescending(x => x.Key).ToList();

        var groupedCombination = GetGroupedCombinationFromGroupedCards(groupedCards);

        var handType = GetHandTypeFromGroupedCombination(groupedCombination);

        return (cards.ToArray(), handType);
    }

    (char[] strength, HandType handType) GetMatchesFromCardsAndJokersCount(List<char> cards, int jokersCount)
    {
        var dict = GetDictionaryFromCardsBasedOnCardsStrength(cards);

        var handType = GetaHandTypeFromGroupedCardsAndJokersCount(dict, jokersCount);

        return (cards.ToArray(), handType);
    }

    (int, int, int, int, int) GetGroupedCombinationFromGroupedCards(List<IGrouping<int, KeyValuePair<char, int>>> groupedCards)
    {
        int one = 0, two = 0, three = 0, four = 0, five = 0;
        foreach (var groupedCard in groupedCards)
        {
            if (groupedCard.Key == 1)
            {
                one = groupedCard.Count();
            }

            if (groupedCard.Key == 2)
            {
                two = groupedCard.Count();
            }

            if (groupedCard.Key == 3)
            {
                three = groupedCard.Count();
            }

            if (groupedCard.Key == 4)
            {
                four = groupedCard.Count();
            }

            if (groupedCard.Key == 5)
            {
                five = groupedCard.Count();
            }
        }

        return (one, two, three, four, five);
    }

    HandType GetaHandTypeFromGroupedCardsAndJokersCount(Dictionary<char, int> groupedCards, int jokersCount)
    {
        if (groupedCards.Any(x => x.Value == (5 - jokersCount)))
        {
            return HandType.FiveOfAKind;
        }

        if (groupedCards.Any(x => x.Value >= (4 - jokersCount)))
        {
            return HandType.FourOfAKind;
        }

        if (groupedCards.Any(x => x.Value == 3) && groupedCards.Any(x => x.Value == 2) ||
        (groupedCards.Count(x => x.Value == 2) == 2 && jokersCount == 1))
        {
            return HandType.FullHouse;
        }

        if (groupedCards.Any(x => x.Value == (3 - jokersCount)))
        {
            return HandType.ThreeOfAKind;
        }

        if (groupedCards.Count(x => x.Value == 2) == 2 )
        {
            return HandType.TwoPair;
        }

        if (groupedCards.Any(x => x.Value == (2 - jokersCount)))
        {
            return HandType.OnePair;
        }

        return HandType.HighCard;
    }

    HandType GetHandTypeFromGroupedCombination((int, int, int, int, int) combination)
    {
        return combination switch
        {
            (5, _, _, _, _) => HandType.HighCard,
            (3, 1, _, _, _) => HandType.OnePair,
            (1, 2, _, _, _) => HandType.TwoPair,
            (2, _, 1, _, _) => HandType.ThreeOfAKind,
            (_, 1, 1, _, _) => HandType.FullHouse,
            (1, _, _, 1, _) => HandType.FourOfAKind,
            (0, _, _, _, 1) => HandType.FiveOfAKind,
            _ => throw new InvalidOperationException()
        };
    }

    Dictionary<char, int> GetDictionaryFromCards(List<char> cards)
    {
        var dict = new Dictionary<char, int>();

        foreach (var card in cards)
        {
            if (!dict.ContainsKey(card))
            {
                dict.Add(card, 1);
            }
            else
            {
                dict[card]++;
            }
        }

        return dict;
    }

    Dictionary<char, int> GetDictionaryFromCardsBasedOnCardsStrength(List<char> cards)
    {
        char[] cardsStrength = ['A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J'];

        var matches = new Dictionary<char, int>();

        foreach (var card in cardsStrength)
        {
            matches.Add(card, 0);
        }

        foreach (var card in cards)
        {
            if (card == 'J')
            {
                continue;
            }
            matches[card]++;
        }

        return matches;
    }
}

enum HandType
{
    HighCard = 1,
    OnePair = 2,
    TwoPair = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7
}

enum Strength
{
    two = 2,
    three = 3,
    four = 4,
    five = 5,
    six = 6,
    seven = 7,
    eight = 8,
    nine = 9,
    T = 10,
    J = 11,
    Q = 12,
    K = 13,
    A = 14
}

record HandResult(HandType HandType, char[] Cards, int Bid);

class CardComparer : IComparer<HandResult>
{
    public int Compare(HandResult? x, HandResult? y)
    {
        if (x is null || y is null)
        {
            throw new InvalidOperationException();
        }

        if ((int)x.HandType > (int)y.HandType)
        {
            return 1;
        }

        if ((int)x.HandType < (int)y.HandType)
        {
            return -1;
        }

        for (int i = 0; i < x.Cards.Length; i++)
        {
            Strength cardX = GetStrengthFromCard(x.Cards[i]);
            Strength cardY = GetStrengthFromCard(y.Cards[i]);
            if ((int)cardX > (int)cardY)
            {
                return 1;
            }

            if ((int)cardY > (int)cardX)
            {
                return -1;
            }
        }

        return 0;
    }
    Strength GetStrengthFromCard(char card)
    {
        return card switch
        {
            '2' => Strength.two,
            '3' => Strength.three,
            '4' => Strength.four,
            '5' => Strength.five,
            '6' => Strength.six,
            '7' => Strength.seven,
            '8' => Strength.eight,
            '9' => Strength.nine,
            'T' => Strength.T,
            'J' => Strength.J,
            'Q' => Strength.Q,
            'K' => Strength.K,
            'A' => Strength.A,
            _ => throw new InvalidOperationException()
        };
    }
}

enum StrengthWithJoker
{
    two = 2,
    three = 3,
    four = 4,
    five = 5,
    six = 6,
    seven = 7,
    eight = 8,
    nine = 9,
    T = 10,
    J = 1,
    Q = 12,
    K = 13,
    A = 14
}

class CardComparerWithJoker : IComparer<HandResult>
{
    public int Compare(HandResult? x, HandResult? y)
    {
        if (x is null || y is null)
        {
            throw new InvalidOperationException();
        }

        if ((int)x.HandType > (int)y.HandType)
        {
            return 1;
        }

        if ((int)x.HandType < (int)y.HandType)
        {
            return -1;
        }

        for (int i = 0; i < x.Cards.Length; i++)
        {
            StrengthWithJoker cardX = GetStrengthFromCard(x.Cards[i]);
            StrengthWithJoker cardY = GetStrengthFromCard(y.Cards[i]);
            if ((int)cardX > (int)cardY)
            {
                return 1;
            }

            if ((int)cardY > (int)cardX)
            {
                return -1;
            }
        }

        return 0;
    }
    StrengthWithJoker GetStrengthFromCard(char card)
    {
        return card switch
        {
            '2' => StrengthWithJoker.two,
            '3' => StrengthWithJoker.three,
            '4' => StrengthWithJoker.four,
            '5' => StrengthWithJoker.five,
            '6' => StrengthWithJoker.six,
            '7' => StrengthWithJoker.seven,
            '8' => StrengthWithJoker.eight,
            '9' => StrengthWithJoker.nine,
            'T' => StrengthWithJoker.T,
            'J' => StrengthWithJoker.J,
            'Q' => StrengthWithJoker.Q,
            'K' => StrengthWithJoker.K,
            'A' => StrengthWithJoker.A,
            _ => throw new InvalidOperationException()
        };
    }
}