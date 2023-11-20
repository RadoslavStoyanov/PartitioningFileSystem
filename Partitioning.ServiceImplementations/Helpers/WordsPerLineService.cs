using Partitioning.ServiceInterfaces.Helpers;
using System.Collections.Concurrent;
using System.Text;

namespace Partitioning.ServiceImplementations.Helpers
{
    public class WordsPerLineService : IWordsPerLineService
    {
        private ConcurrentBag<int> longestRow;
        private StringBuilder previousLine;
        private string[] lineBreak;

        public WordsPerLineService()
        {
            longestRow = new ConcurrentBag<int>();
            previousLine = new StringBuilder();
            lineBreak = new string[]
            {
                "\n",
                "\r\n",
                "\r"
            };
        }

        public void ParseChunk(string chunk)
        {
            Queue<string> lines = new Queue<string>(chunk.Split(lineBreak, StringSplitOptions.None));

            if (previousLine.Length > 0)
            {
                lines.TryDequeue(out var partOfPreviousLine);
                previousLine.Append(partOfPreviousLine);

                HandleEndOfFile();
                HandleEndOfPreviousLine(lines);
            }

            ExtracLinesFromChunk(lines);

            HandleEndOfFile();
        }

        private int GetWordsInLine(string line) =>
            line
                .Split(' ')
                .Length;

        private void UpdateLongestRowData(string currentLine)
        {
            var count = GetWordsInLine(currentLine);
            longestRow.Add(count);
        }

        private void HandleEndOfFile()
        {
            if (previousLine.ToString().EndsWith('\0'))
            {
                UpdateLongestRowData(previousLine.ToString());
                previousLine = new StringBuilder();

                Console.WriteLine($"Longest row has {longestRow.Max()} words");
            }
        }

        private void HandleEndOfPreviousLine(IEnumerable<string> lines)
        {
            if (lines.Count() >= 1)
            {
                UpdateLongestRowData(previousLine.ToString());
                previousLine = new StringBuilder();
            }
        }

        private void ExtracLinesFromChunk(Queue<string> lines)
        {
            var currentLine = string.Empty;

            while (lines.TryDequeue(out currentLine))
            {
                if (lines.Count > 0)
                {
                    UpdateLongestRowData(currentLine);

                    continue;
                }

                previousLine.Append(currentLine);
            }
        }
    }
}