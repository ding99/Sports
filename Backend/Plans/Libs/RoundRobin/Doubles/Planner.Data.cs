using Libs.RoundRobin.Commons;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public static List<int> CreateMaster(int players, int games, bool sample) {

        if (sample) {
            switch (players) {
            case 5:
                if (games == 20) {  // 5 persons, 4 games
                    List<int> e054_1 = [0, 4, 1, 2, 2, 1, 1, 1, 0, 2, 2, 4, 3, 4, 4, 3, 3, 0, 0, 3];
                    return e054_1;
                }
                break;
            case 6:
                switch(games) {
                case 24:  // 6 players, 4 games
                    List<int> e064_1 = [3, 3, 0, 3, 5, 2, 4, 0, 4, 4, 3, 4, 0, 5, 2, 1, 1, 0, 2, 1, 2, 1, 5, 5];
                    return e064_1;
                case 36:  // 6 players, 6 games
                    List<int> e066_1 = [1, 0, 3, 2, 5, 0, 5, 3, 1, 0, 1, 2, 2, 1, 2, 4, 4, 0, 0, 5, 1, 0, 3, 4, 5, 5, 4, 3, 3, 4, 1, 2, 4, 3, 2, 5];
                    return e066_1;
                }
                break;
            case 7:
                if (games == 28) {  // 7 persons, 4 games
                    List<int> e074_1 = [1, 3, 3, 4, 5, 2, 3, 1, 0, 4, 0, 5, 4, 1, 6, 4, 2, 3, 0, 2, 0, 5, 2, 1, 6, 6, 5, 6];
                    return e074_1;
                }
                break;
            case 8:
                switch (games) {
                case 32:  // 8 persons, 4 games
                    List<int> e084_1 = [7, 0, 5, 2, 5, 3, 6, 3, 1, 0, 2, 7, 2, 5, 4, 7, 7, 1, 0, 5, 4, 2, 3, 3, 6, 0, 6, 4, 4, 1, 1, 6];
                    return e084_1;
                case 56:  // 8 persons, 7 games
                    List<int> e087_1 = [6, 0, 1, 4, 4, 3, 7, 5, 6, 6, 0, 7, 0, 4, 5, 1, 7, 1, 7, 7, 3, 7, 6, 4, 2, 4, 1, 3, 3, 3, 3, 4, 5, 3, 5, 4, 0, 7, 6, 0, 2, 1, 5, 0, 5, 2, 1, 2, 2, 5, 6, 6, 2, 0, 2, 1];
                    return e087_1;
                }
                break;
            case 9:
                if (games == 36) {  // 9 persons, 4 games
                    List<int> e094_2 = [3, 2, 2, 8, 5, 3, 6, 8, 0, 5, 8, 7, 4, 2, 0, 7, 6, 1, 1, 2, 8, 0, 6, 1, 1, 4, 0, 4, 6, 5, 4, 7, 7, 3, 3, 5];
                    return e094_2;
                }
                break;
            case 10:
                if (games == 40) {  // 10 persons, 4 games
                    List<int> e104_1 = [2, 4, 7, 6, 8, 5, 2, 3, 0, 6, 6, 1, 2, 3, 6, 2, 0, 9, 9, 1, 5, 5, 7, 5, 1, 1, 8, 4, 8, 7, 0, 3, 7, 3, 9, 4, 9, 4, 0, 8];
                    return e104_1;
                }
                break;
            }
            return [];
        }
        
        // random
        return Utils.NewMaster(players, games);
    }

}
