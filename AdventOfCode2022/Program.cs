using System.Linq;

void Day1()
{
    var sampleData = File.ReadAllLines("Day1.txt");

    var max = 0;
    var current = 0;
    foreach(var deerCalorieEntry in sampleData)
    {
        if (string.IsNullOrWhiteSpace(deerCalorieEntry) || sampleData.Last() == deerCalorieEntry)
        {
            if(current > max)
                max = current;

            current = 0;
            continue;
        }

        current += int.Parse(deerCalorieEntry);
    }

    Console.WriteLine(max);
}

void Day1Part2()
{
    var sampleData = File.ReadAllLines("Day1.txt");

    var allDeerCalories = new List<int>();

    var current = 0;
    foreach (var deerCalorieEntry in sampleData)
    {
        if (string.IsNullOrWhiteSpace(deerCalorieEntry) || sampleData.Last() == deerCalorieEntry)
        {
            allDeerCalories.Add(current);
            current = 0;
            continue;
        }

        current += int.Parse(deerCalorieEntry);
    }

    Console.WriteLine(allDeerCalories
        .OrderByDescending(calorie => calorie)
        .Take(3)
        .Sum());
}

void Day2()
{
    const char RockOpponent = 'A';
    const char PaperOpponent = 'B';
    const char ScissorsOpponent = 'C';

    const char RockMe = 'X';
    const char PaperMe = 'Y';
    const char ScissorsMe = 'Z';

    const int RockPoints = 1;
    const int PaperPoints = 2;
    const int ScissorsPoints = 3;

    const int LosingPoints = 0;
    const int DrawPoints = 3;
    const int WinPoints = 6;

    var pointsOutcomes = new Dictionary<(char OponentMove, char MyMove), int>
    {
        [(RockOpponent, RockMe)] = RockPoints + DrawPoints,
        [(RockOpponent, PaperMe)] = PaperPoints + WinPoints,
        [(RockOpponent, ScissorsMe)] = ScissorsPoints + LosingPoints,

        [(PaperOpponent, RockMe)] = RockPoints + LosingPoints,
        [(PaperOpponent, PaperMe)] = PaperPoints + DrawPoints,
        [(PaperOpponent, ScissorsMe)] = ScissorsPoints + WinPoints,

        [(ScissorsOpponent, RockMe)] = RockPoints + WinPoints,
        [(ScissorsOpponent, PaperMe)] = PaperPoints + LosingPoints,
        [(ScissorsOpponent, ScissorsMe)] = ScissorsPoints + DrawPoints,
    };

    var sampleData = File.ReadAllLines("Day2.txt");

    var points = 0;
    foreach(var (opponentPlay, myPlay) in sampleData.Select(row => (row.First(), row.Last())))
    {
        points += pointsOutcomes[(opponentPlay, myPlay)];
    }

    Console.WriteLine(points);
}

void Day2Part2()
{
    const char RockOpponent = 'A';
    const char PaperOpponent = 'B';
    const char ScissorsOpponent = 'C';

    const char NeedToLose = 'X';
    const char NeedADraw = 'Y';
    const char NeedToWin = 'Z';

    const int RockPoints = 1;
    const int PaperPoints = 2;
    const int ScissorsPoints = 3;

    const int LosingPoints = 0;
    const int DrawPoints = 3;
    const int WinPoints = 6;

    var pointsOutcomes = new Dictionary<(char OponentMove, char MyMove), int>
    {
        [(RockOpponent, NeedToLose)] = ScissorsPoints + LosingPoints,
        [(RockOpponent, NeedADraw)] = RockPoints + DrawPoints,
        [(RockOpponent, NeedToWin)] = PaperPoints + WinPoints,

        [(PaperOpponent, NeedToLose)] = RockPoints + LosingPoints,
        [(PaperOpponent, NeedADraw)] = PaperPoints + DrawPoints,
        [(PaperOpponent, NeedToWin)] = ScissorsPoints + WinPoints,

        [(ScissorsOpponent, NeedToLose)] = PaperPoints + LosingPoints,
        [(ScissorsOpponent, NeedADraw)] = ScissorsPoints + DrawPoints,
        [(ScissorsOpponent, NeedToWin)] = RockPoints + WinPoints,
    };

    var sampleData = File.ReadAllLines("Day2.txt");

    var points = sampleData
        .Select<string, (char OpponentPlay, char MyPlay)> (row => (row.First(), row.Last()))
        .Sum(tuple => pointsOutcomes[(tuple.OpponentPlay, tuple.MyPlay)]);

    Console.WriteLine(points);
}

void Day3()
{
    var sampleData = File.ReadAllLines("Day3.txt");

    int PointsForPriority(char item)
    {
        if(item >= 'a' && item <= 'z')
            return Math.Abs(item - 'a' + 1);

        return Math.Abs(item - 'A' + 27);
    }

    var rucksackHalfs = sampleData
        .Select<string, (string FirstHalf, string SecondHalf)>(line => 
            (line.Substring(0, line.Length / 2), line.Substring(line.Length / 2)));

    var compartmentMistakes = rucksackHalfs.SelectMany((tuple) => tuple.FirstHalf.Intersect(tuple.SecondHalf));

    var result = compartmentMistakes.Sum(PointsForPriority);

    Console.WriteLine(result);
}

void Day3Part2()
{
    var sampleData = File.ReadAllLines("Day3.txt");

    int PointsForPriority(char item)
    {
        if (item >= 'a' && item <= 'z')
            return Math.Abs(item - 'a' + 1);

        return Math.Abs(item - 'A' + 27);
    }

    var rucksackGroups = new string[sampleData.Length / 3][];
    for(var groupId = 0; groupId < sampleData.Length / 3; groupId++)
    {
        rucksackGroups[groupId] = sampleData
            .Skip(groupId * 3)
            .Take(3)
            .ToArray();
    }

    var result = rucksackGroups
        .SelectMany(group => group[0].Intersect(group[1]).Intersect(group[2]))
        .Sum(PointsForPriority);
    
    Console.WriteLine(result);
}

void Day4()
{
    var sampleData = File.ReadAllLines("Day4.txt");

    var pairs = sampleData.Select(line => line.Split(','))
        .Select(pair => (pair[0].Split('-'), pair[1].Split('-')))
        .Select<(string[], string[]), (IEnumerable<int> PairOne, IEnumerable<int> PairTwo)>(pair => (pair.Item1.Select(int.Parse), pair.Item2.Select(int.Parse)));

    var assignments = pairs.Select(pair => 
        {
            return (Enumerable.Range(pair.PairOne.First(), pair.PairOne.Last() - pair.PairOne.First() + 1),
            Enumerable.Range(pair.PairTwo.First(), pair.PairTwo.Last() - pair.PairTwo.First() + 1));
        });

    var result = assignments.Count(pair =>
    {
        if (pair.Item1.Count() >= pair.Item2.Count())
            return pair.Item2.All(pair.Item1.Contains);
        
        return pair.Item1.All(pair.Item2.Contains);
    });

    Console.WriteLine(result);
}

void Day4Part2()
{
    var sampleData = File.ReadAllLines("Day4.txt");

    var pairs = sampleData.Select(line => line.Split(','))
        .Select(pair => (pair[0].Split('-'), pair[1].Split('-')))
        .Select<(string[], string[]), (IEnumerable<int> PairOne, IEnumerable<int> PairTwo)>(pair => (pair.Item1.Select(int.Parse), pair.Item2.Select(int.Parse)));

    var assignments = pairs.Select(pair =>
    {
        return (Enumerable.Range(pair.PairOne.First(), pair.PairOne.Last() - pair.PairOne.First() + 1),
        Enumerable.Range(pair.PairTwo.First(), pair.PairTwo.Last() - pair.PairTwo.First() + 1));
    });

    var result = assignments.Count(pair =>
    {
        if (pair.Item1.Count() >= pair.Item2.Count())
            return pair.Item2.Any(pair.Item1.Contains);

        return pair.Item1.Any(pair.Item2.Contains);
    });

    Console.WriteLine(result);
}

void Day5()
{
    var sampleData = File.ReadAllLines("Day5.txt");

    var indexOfCrateNumbers = 0;
    for(int i = 0; i < sampleData.Length; ++i)
    {
        if (string.IsNullOrWhiteSpace(sampleData[i]))
        {
            indexOfCrateNumbers = i - 1;
            break;
        }
    }

    var stacks = new List<Stack<char>>();
    foreach(var column in sampleData[indexOfCrateNumbers])
    {
        if (!char.IsAsciiDigit(column))
            continue;

        var cargoIndex = sampleData[indexOfCrateNumbers].IndexOf(column);

        var stack = new Stack<char>();
        var loopIndex = indexOfCrateNumbers - 1;
        //loop down to top and push items to stack
        while (loopIndex >= 0)
        {
            var cargo = sampleData[loopIndex][cargoIndex];
            if (cargo is ' ')
            {
                loopIndex--;
                continue;
            }

            stack.Push(cargo);

            loopIndex--;
        }

        stacks.Add(stack);
    }

    var indexOfMoveList = indexOfCrateNumbers + 2;
    var movesLines = sampleData.Skip(indexOfMoveList);

    var moves = movesLines.Select<string, (int MoveCount, int MoveFrom, int MoveTo)>(line =>
    {
        var trimmedLine = line.Replace("move ", "").Replace("from ", "").Replace("to ", "");

        var moveCount = 0;
        var moveFrom = 0;
        var moveTo = 0;
        if (char.IsAsciiDigit(trimmedLine[1]))
        {
            moveCount = int.Parse(string.Join("", trimmedLine.Take(2)));
            moveFrom = int.Parse(trimmedLine[3].ToString());
            moveTo = int.Parse(trimmedLine[5].ToString());
        }
        else
        {
            moveCount = int.Parse(trimmedLine[0].ToString());
            moveFrom = int.Parse(trimmedLine[2].ToString());
            moveTo = int.Parse(trimmedLine[4].ToString());
        }

        return (moveCount, moveFrom, moveTo);
    });

    foreach(var (moveCount, moveFrom, moveTo) in moves)
    {
        for(int i = 0; i < moveCount; i++)
        {
            var from = stacks[moveFrom - 1].Pop();
            stacks[moveTo - 1].Push(from);
        }
    }

    foreach(var stack in stacks)
    {
        Console.Write(stack.Peek());
    }
}

void Day5Part2()
{
    var sampleData = File.ReadAllLines("Day5.txt");

    var indexOfCrateNumbers = 0;
    for (int i = 0; i < sampleData.Length; ++i)
    {
        if (string.IsNullOrWhiteSpace(sampleData[i]))
        {
            indexOfCrateNumbers = i - 1;
            break;
        }
    }

    var stacks = new List<Stack<char>>();
    foreach (var column in sampleData[indexOfCrateNumbers])
    {
        if (!char.IsAsciiDigit(column))
            continue;

        var cargoIndex = sampleData[indexOfCrateNumbers].IndexOf(column);

        var stack = new Stack<char>();
        var loopIndex = indexOfCrateNumbers - 1;
        //loop down to top and push items to stack
        while (loopIndex >= 0)
        {
            var cargo = sampleData[loopIndex][cargoIndex];
            if (cargo is ' ')
            {
                loopIndex--;
                continue;
            }

            stack.Push(cargo);

            loopIndex--;
        }

        stacks.Add(stack);
    }

    var indexOfMoveList = indexOfCrateNumbers + 2;
    var movesLines = sampleData.Skip(indexOfMoveList);

    var moves = movesLines.Select<string, (int MoveCount, int MoveFrom, int MoveTo)>(line =>
    {
        var trimmedLine = line.Replace("move ", "").Replace("from ", "").Replace("to ", "");

        var moveCount = 0;
        var moveFrom = 0;
        var moveTo = 0;
        if (char.IsAsciiDigit(trimmedLine[1]))
        {
            moveCount = int.Parse(string.Join("", trimmedLine.Take(2)));
            moveFrom = int.Parse(trimmedLine[3].ToString());
            moveTo = int.Parse(trimmedLine[5].ToString());
        }
        else
        {
            moveCount = int.Parse(trimmedLine[0].ToString());
            moveFrom = int.Parse(trimmedLine[2].ToString());
            moveTo = int.Parse(trimmedLine[4].ToString());
        }

        return (moveCount, moveFrom, moveTo);
    });

    foreach (var (moveCount, moveFrom, moveTo) in moves)
    {
        var cargoMove = new List<char>();
        for (int i = 0; i < moveCount; i++)
            cargoMove.Add(stacks[moveFrom - 1].Pop());

        cargoMove.Reverse();

        foreach(var cargo in cargoMove)
            stacks[moveTo - 1].Push(cargo);
    }

    foreach (var stack in stacks)
    {
        Console.Write(stack.Peek());
    }
}