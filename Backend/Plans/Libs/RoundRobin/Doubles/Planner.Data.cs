using Libs.RoundRobin.Commons;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public static List<int> CreateMaster(int players, int maxGames, bool sample = false) {

        if (sample) {
            // 9 persons, 36 games
            return [6, 7, 6, 2, 1, 8, 7, 1, 0, 4, 8, 6, 5, 7, 4, 6, 4, 1, 8, 1, 3, 3, 2, 7, 0, 3, 0, 3, 5, 2, 5, 5, 8, 4, 2, 0];
        } else {
            // random
            return Utils.NewMaster(players, maxGames);
        }
    }

}
