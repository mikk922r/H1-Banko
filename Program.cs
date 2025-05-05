namespace Banko
{
    public class Program
    {
        public List<Plate> plates = new List<Plate>();

        public List<int> guessedNumbers = new List<int>();

        public bool gameOver = false;

        static void Main(string[] args)
        {
            var program = new Program();

            program.Run();
        }

        private void Run()
        {
            while (plates.Count == 0)
            {
                Console.WriteLine("Indtast plade-id'er (komma-separeret):");

                string[] plateIds = (Console.ReadLine()?.Split(",") ?? []).Select(x => x.Trim()).ToArray();

                Scraper scraper = new Scraper();

                plates.AddRange(scraper.GetPlates(plateIds));
            }

            while (plates.Count > 0 && !gameOver)
            {
                ShowPlates();

                string input = Console.ReadLine();

                GuessNumber(input);
            }
        }

        private void ShowPlates()
        {
            Console.Clear();

            foreach (var plate in plates)
            {
                Console.WriteLine($"Plade: {plate.Id}");

                for (int i = 0; i < plate.Numbers.Count; i++)
                {
                    Console.Write($"{i + 1}: ");

                    foreach (var value in plate.Numbers[i])
                    {
                        if (guessedNumbers.Contains(value))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ResetColor();
                        }

                        Console.Write($"{value} ");
                    }

                    Console.ResetColor();

                    if (plate.Numbers[i].All(x => guessedNumbers.Contains(x)))
                    {
                        Console.Write("- Banko!");
                    }

                    Console.Write("\n");
                }

                Console.Write("\n");
            }
        }

        private void GuessNumber(string input)
        {
            if (!int.TryParse(input, out int guessedNumber))
            {
                return;
            }

            guessedNumbers.Add(guessedNumber);

            foreach (var plate in plates)
            {
                foreach (var numbers in plate.Numbers)
                {
                    if (!numbers.Contains(guessedNumber))
                    {
                        continue;
                    }

                    if (!numbers.All(x => guessedNumbers.Contains(x)))
                    {
                        continue;
                    }

                    gameOver = true;
                }
            }

            ShowPlates();

            Plate? winningPlate = plates.FirstOrDefault(plate => plate.Numbers.Any(row => row.All(n => guessedNumbers.Contains(n))));

            if (winningPlate is null)
            {
                return;
            }

            Console.WriteLine($"Banko på pladen: {winningPlate.Id}");
        }
    }

    public class Plate
    {
        public string Id { get; set; }

        public List<List<int>> Numbers { get; set; }
    }
}
