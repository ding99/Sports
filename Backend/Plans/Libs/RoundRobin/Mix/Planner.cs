namespace Libs.RoundRobin.Mix;

public partial class Planner {

    public void StartMixed(int men, int women, int games) {
        Console.WriteLine($"-- Round Robin Doubles: men {men}, women {women}, games {games}");
        Console.WriteLine();

        CreateMix(men, women, games);

        Console.ResetColor();
    }

}
