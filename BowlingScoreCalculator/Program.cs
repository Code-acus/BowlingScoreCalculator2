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
        /*
        Note: These comments apply identically to the C++ and C# versions of this program. 
        Calculation of the score percentage:
            The program has calculated A score percentage based on the players total score in the bowling game.
            The score percentage represents the proportion the player achieved relative 
            to the maximum possible score (300) in the game.

        Percentage representation:
             A score with a low percentage should suggest the player has achieved a 
             relatively low percentage of the maximum score.

            It indicates that there is room for improvement in the player's bowling performance 
            needed to increase their score percentage.
        
        Assessment of performance:

            The score percentage can serve as a benchmark to evaluate a user's bowling skills and progress.
            It provides an indication of how effectively they have utilized scoring opportunities in their game.
        
        */
        Console.WriteLine($"Your score percentage is: {percentage.ToString("0.##")}%");

        int handicap = 200 - score;
        Console.WriteLine($"Your handicap is: {handicap}");

        int comparedToAverage = score - 150;
        /*
         
        The statement "Compared to the national average (150), your score is:... should be interpreted as follows:

        National average score:
            The national average score for bowling games is set as 150. This represents an average or 
            benchmark score that might be commonly used as a reference point in a real world application.
        
        Calculation of the difference:
            Here, the program calculate the difference between the players total score and the national average score.
            With the difference is either being a ( - ) or a ( + ), thus indicating that
            the player's score is a negative sum of points below the national average or 
            as positive sum of points above the national average. This value is not based on any "real" derived metrics. 
            Rather the national average, as it were, is to give a sense of where a bowler might fall if the bowler
            were using this application to improve their game. In reality such as application would need to be far
            more advanced than what I have written here.
            
        */
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