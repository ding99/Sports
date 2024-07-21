using System.Text;

using Libs.RoundRobin.Mix.Models;

namespace Libs.RoundRobin.Mix; 

public partial class Planner {

    public void CreateMix(int persons, int games) {
        if ((persons * games) % 4 != 0) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"persons*games should be multiple of 4!");
            return;
        }

        Console.WriteLine("Create Mix");

        //var (tour, players, log) = Find(persons, games);
        //Console.ResetColor();
        //Console.WriteLine(log);

        //Console.ForegroundColor = ConsoleColor.Yellow;
        //Console.WriteLine(DTour(tour, $"New_{persons}-{games}"));

        //Console.ForegroundColor = ConsoleColor.Cyan;
        //Console.WriteLine(DPlayers(players));

    }

}
