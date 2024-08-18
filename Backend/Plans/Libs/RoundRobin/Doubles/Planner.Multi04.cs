using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    #region 4 games

    public void Select054() {
        int persons = 5, games = 4, times = 5, max = 21;
        Chose(persons, games, times, 1, max);
    }

    public void Select064() {
        int persons = 6, games = 4, times = 30, max = 19;
        Chose(persons, games, times, 1, max);
    }

    public void Select074() {
        int persons = 7, games = 4, times = 30, max = 15;
        Chose(persons, games, times, 1, max);
    }

    public void Select084() {
        int persons = 8, games = 4, times = 30, max = 9;
        Chose(persons, games, times, 1, max);
    }

    public void Select094() {
        int persons = 9, games = 4, times = 100, max = 11;
        Chose(persons, games, times, 1, max);
    }

    public void Select104() {
        int persons = 10, games = 4, times = 600, max = 9;
        Chose(persons, games, times, 1, max);
    }

    #endregion

}
