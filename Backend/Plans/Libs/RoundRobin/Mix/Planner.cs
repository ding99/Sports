namespace Libs.RoundRobin.Mix;

public partial class Planner {

    public void StartMixed(int persons, int games) {
        Console.WriteLine($"-- Round Robin Doubles: persons {persons}, games {games}");
        Console.WriteLine();

        CreateMix(persons, games);

        Console.ResetColor();
    }

}
