using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BowlingScoreCalculator
{
    private List<int> rolls = new List<int>();

    public void Roll(int pins)
    {
        rolls.Add(pins);
    }

    public int CalculateScore()
    {
        int score = 0;
        int frameIndex = 0;

        for (int frame = 0; frame < 10; ++frame)
        {
            if (IsStrike(frameIndex))
            {
                score += 10 + StrikeBonus(frameIndex);
                frameIndex++;
            }
            else if (IsSpare(frameIndex))
            {
                score += 10 + SpareBonus(frameIndex);
                frameIndex += 2;
            }
            else
            {
                score += SumOfPinsInSet(frameIndex);
                frameIndex += 2;
            }
        }

        return score;
    }

    private bool IsStrike(int frameIndex)
    {
        return rolls[frameIndex] == 10;
    }

    private bool IsSpare(int frameIndex)
    {
        return SumOfPinsInSet(frameIndex) == 10;
    }

    private int StrikeBonus(int frameIndex)
    {
        if (frameIndex + 2 < rolls.Count)
        {
            return rolls[frameIndex + 1] + rolls[frameIndex + 2];
        }
        else if (frameIndex + 1 < rolls.Count)
        {
            return rolls[frameIndex + 1];
        }
        else
        {
            return 0;
        }
    }

    private int SpareBonus(int frameIndex)
    {
        if (frameIndex + 2 < rolls.Count)
        {
            return rolls[frameIndex + 2];
        }
        else
        {
            return 0;
        }
    }

    private int SumOfPinsInSet(int frameIndex)
    {
        if (frameIndex + 1 < rolls.Count)
        {
            return rolls[frameIndex] + rolls[frameIndex + 1];
        }
        else
        {
            return rolls[frameIndex];
        }
    }
}

public class Program
{
    public static void Main()
    {
        BowlingScoreCalculator calculator = new BowlingScoreCalculator();

        Console.WriteLine("Enter each set as 'x' or 'x, y' where x and y represent the number of pins knocked down.");
        Console.WriteLine("If you strike, just input '10'. For a spare, input two numbers like '5, 5'.");
        Console.WriteLine("Enter 'q' to quit the application without scoring your game.");

        string input;
        int frame = 1;

        while (frame <= 10)
        {
            Console.Write($"Frame {frame} - Set: ");
            input = Console.ReadLine();

            if (input == "q")
            {
                Console.WriteLine("You have chosen to quit the application. Your score will not be calculated. Goodbye.");
                return;
            }

            if (!ValidateInput(input))
            {
                Console.WriteLine("Invalid input. Please make sure you either input '10' or two numbers separated by a comma and a space, like '5, 5'.");
                continue;
            }

            var parts = input.Split(',');
            int pins1 = Int32.Parse(parts[0]);
            int pins2 = parts.Length > 1 ? Int32.Parse(parts[1]) : 0;

            if (pins1 == 10 || pins1 + pins2 == 10)
            {
                calculator.Roll(pins1);
                if (pins1 != 10 && frame == 10)
                    calculator.Roll(pins2);

                if (frame == 10)
                {
                    Console.Write("Bonus Set for 10th frame - Set: ");
                    var bonus = Console.ReadLine().Split(',');
                    calculator.Roll(Int32.Parse(bonus[0]));
                    calculator.Roll(Int32.Parse(bonus[1]));
                }
            }
            else if (pins1 + pins2 < 10)
            {
                calculator.Roll(pins1);
                calculator.Roll(pins2);
            }
            else
            {
                Console.WriteLine("Invalid input. Please make sure your score for a frame does not exceed 10.");
                continue;
            }

            frame++;
        }

        int score = calculator.CalculateScore();
        Console.WriteLine($"Your game score is: {score.ToString().PadLeft(3, '0')} / 300");

        double percentage = (double)score / 300 * 100;
        Console.WriteLine($"Your score percentage is: {percentage.ToString("0.##")}%");

        int handicap = 200 - score;
        Console.WriteLine($"Your handicap is: {handicap}");

        int comparedToAverage = score - 150;
        Console.WriteLine($"Compared to the national average (150), your score is: {(comparedToAverage >= 0 ? "+" : "")}{comparedToAverage}");

        string advice = provideAdvice(score);
        Console.WriteLine($"Advice for improvement: {advice}");
    }

    static bool ValidateInput(string input)
    {
        var parts = input.Split(',');

        if (parts.Length > 2)
            return false;

        if (!int.TryParse(parts[0], out int pins1) || pins1 < 0 || pins1 > 10)
            return false;

        if (parts.Length == 2 && (!int.TryParse(parts[1], out int pins2) || pins2 < 0 || pins2 > 10 || pins1 + pins2 > 10))
            return false;

        return true;
    }

    static string provideAdvice(int score)
    {
        if (score < 100)
        {
            return "Practice more on your aim and try to knock down more pins each time.";
        }
        else if (score < 200)
        {
            return "Work on your strikes and spares to get higher scores.";
        }
        else
        {
            return "You're doing great! Keep practicing to maintain your performance.";
        }
    }
}