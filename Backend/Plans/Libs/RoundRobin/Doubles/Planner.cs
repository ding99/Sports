namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public void Start(int persons, int games) {
        Console.WriteLine($"-- Round Robin Doubles: persons {persons}, games {games}");
        Console.WriteLine();

        ShowSample(persons);
        Create(persons, games);

        Console.ResetColor();
    }

}