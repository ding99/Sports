namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    #region parameters

    public void Select066() {
        int persons = 6, games = 6, times = 30, max = 13;
        Chose06(persons, games, times, max);
    }

    #endregion

    public void Chose06(int persons, int games, int loop, int maxOppo) {
        Chose(persons, games, loop, persons + 1, maxOppo, 2);
    }

}
