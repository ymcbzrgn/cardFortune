using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

public class CardFortune
{
    private IMongoCollection<BsonDocument> _collection;
    public int nameCharacter;
    public List<string> finalFortune = new List<string>();

    public CardFortune(IMongoCollection<BsonDocument> collection)
    {
        _collection = collection;
    }

    public async Task StartFortune()
    {
        var shuffledWords = await Shuffle();

        Console.WriteLine("How many character is your partners name?");
        while (!int.TryParse(Console.ReadLine(), out nameCharacter))
        {
            Console.WriteLine("Sayi gir lan essek");
        }
        var fortune = await fortuneAlgorithm(shuffledWords);


        Console.WriteLine(string.Join(Environment.NewLine, fortune));
        Console.WriteLine("AAAAAAAAA");
        Console.WriteLine(fortune.Count);
        Console.WriteLine("AAAAAAAAA");

        var fortuneUpDown = await UpDown(fortune);

        Console.WriteLine("AAAAAAAAAB");
        Console.WriteLine(fortune.Count);
        Console.WriteLine("AAAAAAAAAB");

        Console.WriteLine(string.Join(Environment.NewLine, fortune));
        Console.WriteLine("IBNELERE INAT YASA");
        Console.WriteLine(string.Join(Environment.NewLine, finalFortune));
        Console.WriteLine("IBNELERE INAT YASA");

    }
    private async Task<List<string>> GetAllWords()
    {
        var allWords = await _collection.Find(new BsonDocument()).ToListAsync();
        return allWords.Select(wordDocument =>
        {
            if (wordDocument.Contains("Card"))
            {
                return wordDocument["Card"].AsString ?? string.Empty;
            }
            return string.Empty;
        }).ToList();
    }

    private async Task<List<string>> Shuffle()
    {
        List<string> allWords = await GetAllWords();

        int iterations = 5;
        Console.WriteLine("How many times you want to shuffle the cards?");
        while (!int.TryParse(Console.ReadLine(), out iterations) || iterations <= 0)
        {
            Console.WriteLine("Adam ol duzgun bisi yaz");
        }

        for (int i = 0; i < iterations; i++)
        {
            Random rng = new Random();
            int n = allWords.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = allWords[k]!;
                allWords[k] = allWords[n];
                allWords[n] = value;
            }
        }

        return allWords!;
    }

    private Task<List<string>> fortuneAlgorithm(List<string> shuffledWords)
    {
        List<List<string>> decks = new List<List<string>>();
        for (int i = 0; i < nameCharacter; i++)
        {
            decks.Add(new List<string>());
        }

        int currentIndex = 0;

        foreach (string card in shuffledWords)
        {
            decks[currentIndex].Add(card);
            currentIndex = (currentIndex + 1) % nameCharacter;
        }
        List<string> lastDeck = new List<string>();
        while (decks.Count > 1)
        {
            int lastIndex = currentIndex != 0 ? currentIndex - 1 : decks.Count - 1;
            lastDeck = decks[lastIndex];
            decks.RemoveAt(lastIndex);

            if (currentIndex != 0)
            {
                currentIndex--;
            }

            foreach (string card in lastDeck)
            {
                decks[currentIndex].Add(card);
                currentIndex = (currentIndex + 1) % decks.Count;
            }
        }

        List<string> result = decks.Count > 0 ? decks[0] : lastDeck;
        return Task.FromResult(result);
    }

    private Task<List<string>> UpDown(List<string> fortune)
    {
        for (int x = 0; x < 10; x++)
        {
            List<string> temp = new List<string>();
            for (int i = 0, j = fortune.Count - 1; i <= j; i++, j--)
            {
                if (fortune[i] == fortune[j])
                {
                    finalFortune.Add(fortune[i]);
                }
                else
                {
                    temp.Add(fortune[i]);
                    temp.Add(fortune[j]);
                }
            }
            fortune.Clear();
            fortune.AddRange(temp);
        }
        List<string> result = fortune;
        return Task.FromResult(result);
    }
}