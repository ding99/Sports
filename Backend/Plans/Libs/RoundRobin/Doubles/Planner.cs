namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public void StartDouble(int persons, int games) {
        Console.WriteLine($"-- Round Robin Doubles: persons {persons}, games {games}");
        Console.WriteLine();

        //ShowSample(persons);
        CreateDbl(persons, games);

        Console.ResetColor();
    }

    public void StartMixed(int persons, int games) {
        Console.WriteLine($"-- Round Robin Doubles: persons {persons}, games {games}");
        Console.WriteLine();

        CreateMix(persons, games);

        Console.ResetColor();
    }

}