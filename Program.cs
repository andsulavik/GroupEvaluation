using GroupEvaluation;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        string filePath = Path.Combine("C:\\Users\\andsu\\source\\repos\\GroupEvaluation\\GroupEvaluation\\answers.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            List<Answer>? answers = JsonConvert.DeserializeObject<List<Answer>>(json);

            if (answers == null)
            {
                return;
            }

            while (true)
            {
                Console.Write("Insert groupId: ");
                if (int.TryParse(Console.ReadLine(), out int groupId))
                {
                    PrintMonthAverages(GetAverageScores(answers, groupId));
                }
                else
                {
                    Console.WriteLine("Error reading input.");
                }

                Console.Write("End program y/n: ");
                if (char.TryParse(Console.ReadLine(), out char end))
                {
                    if (end == 'y')
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Error reading input.");
                }
            }
        }
        else
        {
            Console.WriteLine("File does not exist.");
        }
    }
    public static List<EvaluationResult> GetAverageScores(List<Answer> answers, int groupId)
    {
        return answers.Where(a => a.GroupId == groupId)
                      .GroupBy(a => new { a.EmployeeId, a.AnsweredOn.Month })
                      .Select(g => new
                      {
                          g.Key.Month,
                          AverageScore = g.Average(a => (a.Answer1 + a.Answer2 + a.Answer3 + a.Answer4 + a.Answer5) / 5)
                      })
                      .GroupBy(g => g.Month)
                      .Select(g => new EvaluationResult
                      {
                          Month = g.Key,
                          OverallAverage = g.Average(e => e.AverageScore)
                      })
                      .OrderBy(r => r.Month)
                      .ToList();
    }

    public static void PrintMonthAverages(List<EvaluationResult> groupMonthAverages)
    {
        foreach (var groupMonthAverage in groupMonthAverages)
        {
            Console.WriteLine($"Month no. {groupMonthAverage.Month}, Score: {groupMonthAverage.OverallAverage}");
        }
    }
}