using Libs.RoundRobin.Commons;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public static List<int> CreateMaster(int players, int games, bool sample = false) {

        if (sample) {
            switch (players) {
            case 7:
                if (games == 28) {  // 7 persons, 28 games
                    List<int> e07_1 = [1, 3, 3, 4, 5, 2, 3, 1, 0, 4, 0, 5, 4, 1, 6, 4, 2, 3, 0, 2, 0, 5, 2, 1, 6, 6, 5, 6];
                    return e07_1;
                }
                break;
            case 8:
                if (games == 32) {  // 8 persons, 32 games
                    List<int> e08_1 = [7, 0, 5, 2, 5, 3, 6, 3, 1, 0, 2, 7, 2, 5, 4, 7, 7, 1, 0, 5, 4, 2, 3, 3, 6, 0, 6, 4, 4, 1, 1, 6];
                    return e08_1;
                }
                break;
            case 9:
                if (games == 36) {  // 9 persons, 36 games
                    List<int> e09_2 = [3, 2, 2, 8, 5, 3, 6, 8, 0, 5, 8, 7, 4, 2, 0, 7, 6, 1, 1, 2, 8, 0, 6, 1, 1, 4, 0, 4, 6, 5, 4, 7, 7, 3, 3, 5];
                    return e09_2;
                }
                break;
            case 10:
                if (games == 40) {  // 10 persons, 40 games
                    List<int> e10_1 = [2, 4, 7, 6, 8, 5, 2, 3, 0, 6, 6, 1, 2, 3, 6, 2, 0, 9, 9, 1, 5, 5, 7, 5, 1, 1, 8, 4, 8, 7, 0, 3, 7, 3, 9, 4, 9, 4, 0, 8];
                    return e10_1;
                }
                break;
            }
            return [];
        }
        
        // random
        return Utils.NewMaster(players, games);
    }

}
