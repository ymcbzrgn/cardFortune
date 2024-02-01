using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;

public class CardFortune
{
    private IMongoCollection<BsonDocument> _collection;
    private IMongoCollection<BsonDocument> _finalCollection;
    public int nameCharacter;
    public List<string> finalFortune = new List<string>();
    public List<string> fortuneMeaning = new List<string>();

    public CardFortune(IMongoCollection<BsonDocument> collection, IMongoCollection<BsonDocument> finalCollection)
    {
        _collection = collection;
        _finalCollection = finalCollection;
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
        fortune = await UpDown(fortune);
        fortune = chooseCard(fortune);

        Console.WriteLine("AAAAAAAAA");
        Console.WriteLine(fortune.Count);
        Console.WriteLine("AAAAAAAAA");
        Console.WriteLine(string.Join(Environment.NewLine, fortune));
        Console.WriteLine("IBNELERE INAT YASA");
        Console.WriteLine(string.Join(Environment.NewLine, finalFortune));
        Console.WriteLine("IBNELERE INAT YASA");
        var final = await FortuneTeller();
        Console.WriteLine(string.Join(Environment.NewLine, final));

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

    private async Task<List<List<string>>> GetAllFortunes()
    {
        var allFortunes = await _finalCollection.Find(new BsonDocument()).ToListAsync();

        return allFortunes.Select(wordDocument =>
        {
            if (wordDocument.Contains("fortune") && wordDocument.Contains("meaning"))
            {
                string fortune = wordDocument["fortune"].AsString ?? string.Empty;
                string meaning = wordDocument["meaning"].AsString ?? string.Empty;

                return new List<string> { fortune, meaning };
            }

            return new List<string>();
        }).ToList();
    }

    private async Task<List<string>> FortuneTeller()
    {
        List<List<string>> allFortunes = await GetAllFortunes();
        List<string> fortuneMeanings = new List<string>();

        foreach (var fortunePair in allFortunes)
        {
            if (fortunePair.Count == 2)
            {
                string fortune = fortunePair[0];
                string meaning = fortunePair[1];

                if (finalFortune.Contains(fortune))
                {
                    fortuneMeanings.Add($"{fortune} - {meaning}");
                }
            }
        }
        return fortuneMeanings;
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

    private Task<List<string>> fortuneAlgorithm(List<string> shuffledWords) //degisiklik gerekebilir
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
        for (int x = 0; x < 3; x++)
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


    private List<string> chooseCard(List<string> fortune)
    {
        List<string> originalFortune = new List<string>(fortune);

        List<string> specialCards = new List<string> { "Yourself", "Him/Her", "Us" };

        foreach (var specialCard in specialCards)
        {
            int temp = 0;
            Console.WriteLine($"Now I want you to pick a card from the remaining deck ({originalFortune.Count}) onto {specialCard}");

            while (!int.TryParse(Console.ReadLine(), out temp) || temp < 0 || temp >= originalFortune.Count)
            {
                Console.WriteLine("ekinler bas vermeden kor buzagi topallamazmis");
            }

            finalFortune.Add($"{specialCard}_{originalFortune[temp]}");
            originalFortune.RemoveAt(temp);
        }

        for (int i = 0; i < finalFortune.Count - 3; i++)
        {
            int temp = 0;
            Console.WriteLine("Now I want you to pick a card from the remaining deck (" + (originalFortune.Count) + ") onto the " + finalFortune[i]);

            while (!int.TryParse(Console.ReadLine(), out temp) || temp < 0 || temp >= originalFortune.Count)
            {
                Console.WriteLine("ekinler bas vermeden kor buzagi topallamazmis");
            }

            finalFortune[i] = finalFortune[i] + "_" + originalFortune[temp];
            originalFortune.RemoveAt(temp);
        }

        return originalFortune;
    }


    // Windows form

    // WEB
}