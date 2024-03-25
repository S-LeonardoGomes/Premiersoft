using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Questao2;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main()
    {
        string opcaoDigitada;
        
        do
        {
            try
            {
                Console.Clear();
                Console.Write("Time: ");
                string teamName = Console.ReadLine().Trim();

                Console.Write("Ano: ");
                int year = int.Parse(Console.ReadLine());

                List<MatchResult> matches = getMatchesResult(teamName, year).Result;

                int totalGoals = getTotalScoredGoals(matches, teamName);

                Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");
            }
            catch (FormatException)
            {
                Console.WriteLine();
                Console.WriteLine("-----------------------------------------------------------------------------------");
                Console.WriteLine("Erro: Um dos campos foi digitado no formato incorreto. Tente novamente, por favor.");
                Console.WriteLine("-----------------------------------------------------------------------------------");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.Write("Entre 's' caso deseje inserir mais um exemplo: ");
            opcaoDigitada = Console.ReadLine().ToLower();
        } while (opcaoDigitada == "s");

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    private static int getTotalScoredGoals(List<MatchResult> matches, string teamName)
    {
        int scoredGoalsAsHost = matches.Where(x => x.Team1 == teamName).Select(x => int.Parse(x.Team1Goals)).Sum();
        int scoredGoalsAsGuest = matches.Where(x => x.Team2 == teamName).Select(x => int.Parse(x.Team2Goals)).Sum();

        return scoredGoalsAsGuest + scoredGoalsAsHost;
    }

    private static async Task<List<MatchResult>> getMatchesResult(string teamName, int year)
    {
        int totalPages = 1;
        List<MatchResult> matches = new();

        Dictionary<string, string>? queryFilters = new()
        {
            { "year", year.ToString() },
            { "page", "0" }
        };

        for(int page = 1; page <= totalPages; page++)
        {
            if (page == 1)
            {
                queryFilters.Add("team1", teamName);
            }

            queryFilters["page"] = page.ToString();

            ApiResponse? footballMatches = await GetFootballMatches(queryFilters);

            if (page == 1) totalPages = footballMatches?.Total_Pages ?? 1;
            matches.AddRange(footballMatches?.Data);
        }

        for (int page = 1; page <= totalPages; page++)
        {
            if (page == 1)
            {
                queryFilters.Remove("team1");
                queryFilters.Add("team2", teamName);
            }

            queryFilters["page"] = page.ToString();

            ApiResponse? footballMatches = await GetFootballMatches(queryFilters);

            if (page == 1) totalPages = footballMatches?.Total_Pages ?? 1;
            matches.AddRange(footballMatches?.Data);
        }

        return matches;
    }

    private static async Task<ApiResponse?> GetFootballMatches(Dictionary<string, string> queryFilters)
    {
        try
        {
            using (HttpClient client = new())
            {
                string urlRequest = QueryHelpers.AddQueryString("https://jsonmock.hackerrank.com/api/football_matches", queryFilters);

                HttpResponseMessage response = await client.GetAsync(urlRequest);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ApiResponse>(responseBody);                    
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }

        return null;
    } 
}