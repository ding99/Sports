namespace Libs.RoundRobin.Commons;

public partial class Utils {

    public static List<int> NewMaster(int players, int maxGames) {
        List<int> list = [];
        Random rd = new();
        int rounds = maxGames / players, n;

        while (list.Count < rounds * players) {
            n = rd.Next(players);
            if (list.Count(x => x == n) < rounds) {
                list.Add(n);
            }
        }

        while (list.Count < maxGames) {
            n = rd.Next(players);
            if (list.Count(x => x == n) < rounds + 1) {
                list.Add(n);
            }
        }

        return list;
    }

}
