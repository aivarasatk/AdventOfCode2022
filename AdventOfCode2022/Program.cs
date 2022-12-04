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