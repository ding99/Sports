namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    #region parameters

    public void Select087() {
        int persons = 8, games = 7, times = 30, max = 1;
        Chose07(persons, games, times, max);
    }

    #endregion

    public void Chose07(int persons, int games, int loop, int maxOppo) {
        Chose(persons, games, loop, persons, maxOppo, 2);
    }

}
