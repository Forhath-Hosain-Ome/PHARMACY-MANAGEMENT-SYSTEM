namespace PharmacySystem.Console.Utilities;

public static class ConsoleHelper
{
    #region Colors

    private static readonly ConsoleColor HeaderColor = ConsoleColor.Cyan;
    private static readonly ConsoleColor SuccessColor = ConsoleColor.Green;
    private static readonly ConsoleColor ErrorColor = ConsoleColor.Red;
    private static readonly ConsoleColor WarningColor = ConsoleColor.Yellow;
    private static readonly ConsoleColor InfoColor = ConsoleColor.Blue;

    #endregion

    #region Display Methods (Static Methods)
    public static void PrintHeader(string title)
    {
        System.Console.ForegroundColor = HeaderColor;
        System.Console.WriteLine();
        System.Console.WriteLine("═══════════════════════════════════════════════════════════════");
        System.Console.WriteLine($"  {title.ToUpper()}");
        System.Console.WriteLine("═══════════════════════════════════════════════════════════════");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }
    public static void PrintSuccess(string message)
    {
        System.Console.ForegroundColor = SuccessColor;
        System.Console.WriteLine($"✓ {message}");
        System.Console.ResetColor();
    }
    public static void PrintError(string message)
    {
        System.Console.ForegroundColor = ErrorColor;
        System.Console.WriteLine($"✗ ERROR: {message}");
        System.Console.ResetColor();
    }

    public static void PrintWarning(string message)
    {
        System.Console.ForegroundColor = WarningColor;
        System.Console.WriteLine($"⚠ WARNING: {message}");
        System.Console.ResetColor();
    }

    public static void PrintInfo(string message)
    {
        System.Console.ForegroundColor = InfoColor;
        System.Console.WriteLine($"ℹ {message}");
        System.Console.ResetColor();
    }

    public static void PrintSeparator()
    {
        System.Console.WriteLine("───────────────────────────────────────────────────────────────");
    }

    public static void PrintSection(string title)
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = HeaderColor;
        System.Console.WriteLine($"▶ {title}");
        System.Console.ResetColor();
        PrintSeparator();
    }

    #endregion

    #region Input Methods (Static Methods with Function Overloading)

    public static int GetIntInput(string prompt)
    {
        while (true)
        {
            System.Console.Write($"{prompt}: ");
            if (int.TryParse(System.Console.ReadLine(), out int result))
            {
                return result;
            }
            PrintError("Invalid input. Please enter a valid number.");
        }
    }

    public static int GetIntInput(string prompt, int min, int max)
    {
        while (true)
        {
            int value = GetIntInput(prompt);
            if (value >= min && value <= max)
            {
                return value;
            }
            PrintError($"Please enter a number between {min} and {max}.");
        }
    }

    public static decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            System.Console.Write($"{prompt}: ");
            if (decimal.TryParse(System.Console.ReadLine(), out decimal result))
            {
                return result;
            }
            PrintError("Invalid input. Please enter a valid decimal number.");
        }
    }

    public static decimal GetDecimalInput(string prompt, decimal min, decimal max)
    {
        while (true)
        {
            decimal value = GetDecimalInput(prompt);
            if (value >= min && value <= max)
            {
                return value;
            }
            PrintError($"Please enter a number between {min} and {max}.");
        }
    }

    public static string GetStringInput(string prompt)
    {
        System.Console.Write($"{prompt}: ");
        return System.Console.ReadLine() ?? string.Empty;
    }

    public static string GetStringInput(string prompt, bool required)
    {
        while (true)
        {
            string input = GetStringInput(prompt);
            if (!required || !string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            PrintError("This field is required. Please enter a value.");
        }
    }

    public static DateTime GetDateInput(string prompt)
    {
        while (true)
        {
            System.Console.Write($"{prompt} (yyyy-MM-dd): ");
            if (DateTime.TryParse(System.Console.ReadLine(), out DateTime result))
            {
                return result;
            }
            PrintError("Invalid date format. Please use yyyy-MM-dd.");
        }
    }

    public static DateTime GetDateInput(string prompt, DateTime minDate, DateTime maxDate)
    {
        while (true)
        {
            DateTime date = GetDateInput(prompt);
            if (date >= minDate && date <= maxDate)
            {
                return date;
            }
            PrintError($"Date must be between {minDate:yyyy-MM-dd} and {maxDate:yyyy-MM-dd}.");
        }
    }

    public static bool GetBoolInput(string prompt)
    {
        while (true)
        {
            System.Console.Write($"{prompt} (y/n): ");
            string input = System.Console.ReadLine()?.ToLower() ?? "";
            
            if (input == "y" || input == "yes")
                return true;
            if (input == "n" || input == "no")
                return false;
            
            PrintError("Please enter 'y' for yes or 'n' for no.");
        }
    }

    public static bool ConfirmAction(string message)
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = WarningColor;
        System.Console.Write($"⚠ {message} Are you sure? (y/n): ");
        System.Console.ResetColor();
        
        string input = System.Console.ReadLine()?.ToLower() ?? "";
        return input == "y" || input == "yes";
    }

    #endregion

    #region Menu Methods (Static Methods)

    public static int DisplayMenu(string title, params string[] options)
    {
        PrintHeader(title);
        
        for (int i = 0; i < options.Length; i++)
        {
            System.Console.WriteLine($"  {i + 1}. {options[i]}");
        }
        System.Console.WriteLine($"  0. Back/Exit");
        System.Console.WriteLine();

        return GetIntInput("Select an option", 0, options.Length);
    }

    public static int DisplayMenu(string title, Dictionary<string, string> optionsWithDescriptions)
    {
        PrintHeader(title);
        
        int index = 1;
        var options = optionsWithDescriptions.ToList();
        
        foreach (var option in options)
        {
            System.Console.WriteLine($"  {index}. {option.Key}");
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"     {option.Value}");
            System.Console.ResetColor();
            index++;
        }
        System.Console.WriteLine($"  0. Back/Exit");
        System.Console.WriteLine();

        return GetIntInput("Select an option", 0, options.Count);
    }

    #endregion

    #region Utility Methods (Static Methods)

    public static void ClearScreen()
    {
        System.Console.Clear();
    }

    public static void PauseForUser()
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.DarkGray;
        System.Console.Write("Press any key to continue...");
        System.Console.ResetColor();
        System.Console.ReadKey(true);
        System.Console.WriteLine();
    }

    public static void ShowLoading(string message, int milliseconds = 1000)
    {
        System.Console.Write($"{message}");
        string[] animation = { ".", "..", "..." };
        
        for (int i = 0; i < 3; i++)
        {
            System.Console.Write(animation[i % animation.Length]);
            Thread.Sleep(milliseconds / 3);
        }
        System.Console.WriteLine(" Done!");
    }

    public static void DisplayTable(string[] headers, List<string[]> rows)
    {
        // Calculate column widths
        int[] widths = new int[headers.Length];
        for (int i = 0; i < headers.Length; i++)
        {
            widths[i] = headers[i].Length;
            foreach (var row in rows)
            {
                if (i < row.Length && row[i].Length > widths[i])
                {
                    widths[i] = row[i].Length;
                }
            }
            widths[i] += 2; // Padding
        }

        // Print headers
        System.Console.WriteLine();
        System.Console.ForegroundColor = HeaderColor;
        for (int i = 0; i < headers.Length; i++)
        {
            System.Console.Write(headers[i].PadRight(widths[i]));
        }
        System.Console.WriteLine();
        System.Console.WriteLine(new string('─', widths.Sum()));
        System.Console.ResetColor();

        // Print rows
        foreach (var row in rows)
        {
            for (int i = 0; i < row.Length && i < widths.Length; i++)
            {
                System.Console.Write(row[i].PadRight(widths[i]));
            }
            System.Console.WriteLine();
        }
        System.Console.WriteLine();
    }

    public static void ShowProgressBar(int current, int total, string label = "Progress")
    {
        int barWidth = 40;
        int progress = (int)((double)current / total * barWidth);
        
        System.Console.Write($"\r{label}: [");
        System.Console.Write(new string('█', progress));
        System.Console.Write(new string('░', barWidth - progress));
        System.Console.Write($"] {current}/{total} ({(int)((double)current / total * 100)}%)");
        
        if (current == total)
            System.Console.WriteLine();
    }

    #endregion

    #region Format Methods (Static Methods)

    public static string FormatCurrency(decimal amount)
    {
        return $"${amount:N2}";
    }

    public static string FormatDate(DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }

    public static string FormatDate(DateTime date, bool includeTime)
    {
        return includeTime ? date.ToString("yyyy-MM-dd HH:mm:ss") : FormatDate(date);
    }

    public static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;
        
        return value.Substring(0, maxLength - 3) + "...";
    }

    #endregion
}
