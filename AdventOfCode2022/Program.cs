using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Numerics;

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

void Day6()
{
    var sampleData = File.ReadAllText("Day6.txt");


    for (int i = 0; i < sampleData.Length; ++i)
    {
        var uniqueChars = new List<char>
        {
            sampleData[i],
            sampleData[i + 1],
            sampleData[i + 2],
            sampleData[i + 3]
        };

        if (uniqueChars.Distinct().Count() is 4)
        {
            Console.WriteLine(i + 4);
            break;
        }
    }
}

void Day6Part2()
{
    var sampleData = File.ReadAllText("Day6.txt");


    for (int i = 0; i < sampleData.Length; ++i)
    {
        var uniqueChars = new List<char>
        {
            sampleData[i],
            sampleData[i + 1],
            sampleData[i + 2],
            sampleData[i + 3],
            sampleData[i + 4],
            sampleData[i + 5],
            sampleData[i + 6],
            sampleData[i + 7],
            sampleData[i + 8],
            sampleData[i + 9],
            sampleData[i + 10],
            sampleData[i + 11],
            sampleData[i + 12],
            sampleData[i + 13],
        };

        if (uniqueChars.Distinct().Count() is 14)
        {
            Console.WriteLine(i + 14);
            break;
        }
    }
}

void Day7()
{
    var sampleData = File.ReadLines("Day7.txt").Skip(1);

    var unique = Guid.NewGuid().ToString();
    Directory.CreateDirectory(unique);
    Directory.SetCurrentDirectory(unique);
    var baseDir = Directory.GetCurrentDirectory();

    foreach (var command in sampleData)
    {
        var parts = command.Split(" ");

        if (parts[0] is "$")//command
        {
            switch (parts[1])
            {
                case "cd" when parts[2] is "..":
                    var parent = Directory.GetParent(Directory.GetCurrentDirectory());
                    Directory.SetCurrentDirectory(parent.FullName);
                    break;
                case "cd":
                    Directory.CreateDirectory(parts[2]);
                    Directory.SetCurrentDirectory(parts[2]);
                    break;
                case "ls":
                    break;
            }
        }
        else //items and dirs
        {
            switch(parts[0])
            {
                case "dir":
                    Directory.CreateDirectory(parts[1]);
                    break;
                default:
                    File.Create($"{Directory.GetCurrentDirectory()}/{parts[0]}_{parts[1]}");
                    break;
            }
        }
    }

    var dirs = Directory.GetDirectories(baseDir, "*", SearchOption.AllDirectories);

    var sum = 0;
    foreach(var dir in dirs.Append(baseDir)) 
    { 
        var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
        
        var folderSize = files.Select(file => new FileInfo(file).Name)
            .Select(name => name.Substring(0, name.IndexOf("_")))
            .Select(int.Parse)
            .Sum();

        if(folderSize <= 100000)
            sum += folderSize;
    }

    Console.WriteLine(sum);
}

void Day7Part2()
{
    var sampleData = File.ReadLines("Day7.txt").Skip(1);

    var unique = Guid.NewGuid().ToString();
    Directory.CreateDirectory(unique);
    Directory.SetCurrentDirectory(unique);
    var baseDir = Directory.GetCurrentDirectory();

    foreach (var command in sampleData)
    {
        var parts = command.Split(" ");

        if (parts[0] is "$")//command
        {
            switch (parts[1])
            {
                case "cd" when parts[2] is "..":
                    var parent = Directory.GetParent(Directory.GetCurrentDirectory());
                    Directory.SetCurrentDirectory(parent.FullName);
                    break;
                case "cd":
                    Directory.CreateDirectory(parts[2]);
                    Directory.SetCurrentDirectory(parts[2]);
                    break;
                case "ls":
                    break;
            }
        }
        else //items and dirs
        {
            switch (parts[0])
            {
                case "dir":
                    Directory.CreateDirectory(parts[1]);
                    break;
                default:
                    File.Create($"{Directory.GetCurrentDirectory()}/{parts[0]}_{parts[1]}");
                    break;
            }
        }
    }

    var dirs = Directory.GetDirectories(baseDir, "*", SearchOption.AllDirectories);

    var folderSizes = new List<int>();
    foreach (var dir in dirs.Append(baseDir))
    {
        var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);

        var folderSize = files.Select(file => new FileInfo(file).Name)
            .Select(name => name.Substring(0, name.IndexOf("_")))
            .Select(int.Parse)
            .Sum();

        folderSizes.Add(folderSize);
    }

     var baseDirFiles = Directory.GetFiles(baseDir, "*", SearchOption.AllDirectories);

     var usedSpace = baseDirFiles.Select(file => new FileInfo(file).Name)
        .Select(name => name.Substring(0, name.IndexOf("_")))
        .Select(int.Parse)
        .Sum();

    foreach(var size in folderSizes.Order())
    {
        if (70000000 - usedSpace + size is >= 30000000)
        {
            Console.WriteLine(size);
           break;
        }
    }
}

void Day8()
{
    var sampleData = File.ReadAllLines("Day8.txt");

    var defaultVisibleTrees = sampleData[0].Length * 2 + sampleData.Length * 2 - 4; //-4 is to account for top and bottom edges incluced in "sampleData[0].Length * 2"

    var data = sampleData.Select(line => line.ToCharArray()).ToArray();

    var visibleTrees = 0;
    for(int row = 1; row < data.Length - 1; row++)
    {
        for(int column = 1; column < data[row].Length - 1; column++) 
        {
            if (Day8IsVisibleInFromDirection(data, row, column - 1, data[row][column], "left")
                || Day8IsVisibleInFromDirection(data, row, column + 1, data[row][column], "right")
                || Day8IsVisibleInFromDirection(data, row + 1, column, data[row][column], "down")
                || Day8IsVisibleInFromDirection(data, row - 1, column, data[row][column], "up"))
                ++visibleTrees;
        }
    }

    Console.WriteLine(visibleTrees + defaultVisibleTrees);
}

void Day8Part2()
{
    var sampleData = File.ReadAllLines("Day8.txt");

    var data = sampleData.Select(line => line.ToCharArray()).ToArray();

    var maxScenicScore = 0;
    for (int row = 1; row < data.Length - 1; row++)
    {
        for (int column = 1; column < data[row].Length - 1; column++)
        {
            var score = Day8ScenicScore(data, row, column - 1, data[row][column], "left")
                * Day8ScenicScore(data, row, column + 1, data[row][column], "right")
                * Day8ScenicScore(data, row + 1, column, data[row][column], "down")
                * Day8ScenicScore(data, row - 1, column, data[row][column], "up");

            if(score > maxScenicScore)
            {
                maxScenicScore = score;
            }
        }
    }

    Console.WriteLine(maxScenicScore);
}

bool Day8IsVisibleInFromDirection(char[][] data, int row, int column, char tree, string direction)
{
    var allTreesShorter = true;
    switch (direction)
    {
        case "left":
            for(int i = column; i >= 0; i--)
                if (data[row][i] >= tree)
                    allTreesShorter = false;
            break;
        case "right":
            for (int i = column; i < data[0].Length; i++)
                if (data[row][i] >= tree)
                    allTreesShorter = false;
            break;
        case "down":
            for (int i = row; i < data.Length; i++)
                if (data[i][column] >= tree)
                    allTreesShorter = false;
            break;
        case "up":
            for (int i = row; i >= 0; i--)
                if (data[i][column] >= tree)
                    allTreesShorter = false;
            break;
    }

    return allTreesShorter;
}

int Day8ScenicScore(char[][] data, int row, int column, char tree, string direction)
{
    var count = 0;
    switch (direction)
    {
        case "left":
            for (int i = column; i >= 0; i--)
            {
                count++;
                if (data[row][i] >= tree)
                    break;
            }
            break;
        case "right":
            for (int i = column; i < data[0].Length; i++)
            {
                count++;
                if (data[row][i] >= tree)
                    break;
            }
            break;
        case "down":
            for (int i = row; i < data.Length; i++)
            {
                count++;
                if (data[i][column] >= tree)
                    break;
            }
            break;
        case "up":
            for (int i = row; i >= 0; i--)
            {
                count++;
                if (data[i][column] >= tree)
                    break;
            }    
            break;
    }

    return count;
}

void Day9()
{
    var sampleData = File.ReadAllLines("Day9.txt");

    var rowCount = 2000;
    var movementGrid = new int[rowCount][];
    for(int i = 0; i < movementGrid.Length; ++i)
        movementGrid[i] = new int[rowCount];

    movementGrid[rowCount / 2][rowCount / 2] = 1; //start position is visited

    var (headRow, headColumn) = (rowCount/2 , rowCount / 2);
    var (tailRow, tailColumn) = (rowCount/2, rowCount / 2);

    foreach (var line in sampleData)
    {
        var (move, repetition) = (line[0], int.Parse(line.Split(" ")[1]));

        foreach(var _ in Enumerable.Repeat(0, repetition))
        {
            var (previousHeadRow, previousHeadColumn) = (headRow, headColumn);
            (headRow, headColumn) = Day9MoveHead(headRow, headColumn, move);
            if(!Day9IsAdjacent(headRow, headColumn, tailRow, tailColumn))
                (tailRow, tailColumn) = Day9MoveTail(previousHeadRow, previousHeadColumn, headRow, headColumn, tailRow, tailColumn, move);

            movementGrid[tailRow][tailColumn]++;
        }
    }

    var visited = 0;
    for(int row = 0; row < movementGrid.Length; ++row)
    {
        for(int column = 0; column < movementGrid[0].Length; ++column)
        {
            if (movementGrid[row][column] > 0)
                visited++;
        }
    }

    Console.WriteLine(visited);
}

bool Day9IsAdjacent(int headRow, int headColumn, int tailRow, int tailColumn)
{
    return headRow == tailRow && headColumn == tailColumn
        || (Math.Abs(headRow - tailRow) is 1 && headColumn == tailColumn) //is up or down
        || headRow == tailRow && (Math.Abs(headColumn - tailColumn) is 1)//is left or right
        || headRow - tailRow is 1 && headColumn - tailColumn is 1 // down right
        || headRow - tailRow is 1 && headColumn - tailColumn is -1 // down left
        || headRow - tailRow is -1 && headColumn - tailColumn is 1 //up right
        || headRow - tailRow is -1 && headColumn - tailColumn is -1 //up left
        ;
}

(int row, int column) Day9MoveHead(int row, int column, char move) => move switch
{
    'U' => (row - 1, column),
    'D' => (row + 1, column),
    'L' => (row, column - 1),
    'R' => (row, column + 1),
    _ => throw new NotImplementedException()
};

(int row, int column) Day9MoveTail(
    int previousHeadRow,
    int previousHeadColumn,
    int headRow,
    int headColumn,
    int tailRow,
    int tailColumn,
    char move) 
{
    if (Day9IsAdjacent(headRow, headColumn, tailRow, tailColumn))
        return (tailRow, tailColumn);

    return move switch
    {
        'U' when headColumn == tailColumn => (tailRow - 1, tailColumn), // tail moves up
        'D' when headColumn == tailColumn => (tailRow + 1, tailColumn), // tail moves down
        'L' when headRow == tailRow => (tailRow, tailColumn - 1), // tail moves left
        'R' when headRow == tailRow => (tailRow, tailColumn + 1), // tail moves right

        'U' when headColumn != tailColumn => (tailRow - 1, headColumn), // tail moves up to head column
        'D' when headColumn != tailColumn => (tailRow + 1, headColumn), // tail moves down
        'L' when headRow != tailRow => (headRow, tailColumn - 1), // tail moves left
        'R' when headRow != tailRow => (headRow, tailColumn + 1), // tail moves left
        _ => throw new NotImplementedException()
    };
}

void Day10()
{
    var sampleData = File.ReadAllLines("Day10.txt");

    var cycleChecks = new[] { 20, 60, 100, 140, 180, 220 };

    var cycle = 1;
    var registerValue = 1;
    var result = 0;
    foreach(var command in sampleData)
    {
        if(command is "noop")
        {
            //do nothing
            if (cycleChecks.Contains(cycle))
                result += registerValue * cycle;

            cycle++;
        }
        else
        {
            for(int i = 0; i < 2; ++i)
            {
                if (cycleChecks.Contains(cycle))
                    result += registerValue * cycle;

                cycle++;
            }
            registerValue += int.Parse(command.Split(" ")[1]);
        }

        if (cycle > 220)
            break;
    }
    Console.WriteLine(result);

}

void Day10Part2()
{
    var sampleData = File.ReadAllLines("Day10.txt");

    var cycle = 1;
    var registerValue = 1;
    foreach (var command in sampleData)
    {
        if (command is "noop")
        {
            char pixel = Day10Pixel(cycle, registerValue);

            Day10Draw(cycle, pixel);

            cycle++;
        }
        else
        {
            for (int i = 0; i < 2; ++i)
            {
                char pixel = Day10Pixel(cycle, registerValue);

                Day10Draw(cycle, pixel);

                cycle++;
            }
            registerValue += int.Parse(command.Split(" ")[1]);
        }
    }
}

void Day10Draw(int cycle, char pixel)
{
    Console.Write(pixel);

    if (cycle % 40 is 0)
        Console.WriteLine();
}

char Day10Pixel(int cycle, int registerValue)
{
    char pixel = '.';
    var index = (cycle - 1) % 40;
    if (registerValue == index || registerValue - 1 == index || registerValue + 1 == index)
        pixel = '#';

    return pixel;
}