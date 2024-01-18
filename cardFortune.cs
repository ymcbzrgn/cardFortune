using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

public class CardFortune
{
    private IMongoCollection<BsonDocument> _collection;

    public CardFortune(IMongoCollection<BsonDocument> collection)
    {
        _collection = collection;
    }

    public async Task StartFortune()
    {
        var shuffledWords = await Shuffle();
        int nameCharacter;
        Console.WriteLine("How many character is your partners name?");
        while (!int.TryParse(Console.ReadLine(), out nameCharacter))
        {
            Console.WriteLine("Sayi gir lan essek");
        }
        var fortune = await fortuneAlgorithm();

        for (int i = 0; i < fortune.Count; i++)
        {
            Console.WriteLine("");
        }
    }
    private async Task<List<string?>> GetAllWords()
    {
        var allWords = await _collection.Find(new BsonDocument()).ToListAsync();
        return allWords.Select(wordDocument =>
        {
            if (wordDocument.Contains("Card"))
            {
                return wordDocument["Card"].AsString;
            }
            return null;
        }).Where(word => word != null).ToList();
    }

    private async Task<List<string>> Shuffle()
    {
        List<string?> allWords = await GetAllWords();

        Console.WriteLine("How many times you want to shuffle the cards?");
        if (!int.TryParse(Console.ReadLine(), out int iterations) || iterations <= 0)
        {
            Console.WriteLine("Invalid input. Using default iterations.");
            iterations = 5;
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

    private async Task<List<string?>> fortuneAlgorithm()
    {
        List<string?> result =new List<string?>();
        result.Add("IBNELERE INAT YASA");
        return result;
    }
}

