using Libs.RoundRobin.Commons;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    public static List<int> CreateMaster(int players, int maxGames) {

        ////last one
        //return [1, 6, 1, 6, 5, 0, 5, 0, 0, 3, 2, 9, 7, 2, 6, 8, 0, 3, 9, 4, 3, 8, 2, 2, 5, 6, 4, 9, 8, 4, 1, 9, 6, 5, 6, 4, 0, 5, 7, 1, 9, 5, 8, 8, 3, 0, 2, 4, 3, 2, 1, 1, 3, 4, 8, 7, 9, 7, 7, 7];

        //// 6 round
        //return [3, 1, 6, 0, 8, 6, 3, 0, 5, 8, 9, 0, 0, 6, 3, 9, 9, 0, 1, 8, 1, 0, 7, 5, 6, 1, 6, 6, 9, 7, 5, 4, 1, 8, 8, 4, 9, 8, 4, 4, 9, 7, 1, 7, 3, 4, 4, 7, 3, 7, 5, 3, 2, 2, 5, 2, 2, 2, 2, 5];

        #region random
        //List<int> list = [];
        //Random rd = new();
        //int a, maxPosition = players * maxGames;

        //while (list.Count < maxPosition) {
        //    a = rd.Next(players);
        //    if (list.Count(x => x == a) < maxGames) {
        //        list.Add(a);
        //    }
        //}

        return Utils.NewMaster(players, maxGames);
        #endregion
    }

}
