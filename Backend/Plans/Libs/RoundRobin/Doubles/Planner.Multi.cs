using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles;

public partial class Planner {

    // maxPart: 1:4-games, persons:6-game, persons:7-game
    // repeat: 1:4-game, 2:6-game, 2:7-game
    /// <summary>
    /// Chose tours
    /// </summary>
    /// <param name="persons">Number of players</param>
    /// <param name="games">Number of games</param>
    /// <param name="loop">Loop times</param>
    /// <param name="maxPart">Max number of repeated partners</param>
    /// <param name="maxOppo">Max mumber of repeated opponents</param>
    /// <param name="repeat">Number of opponent's repeated times</param>
    public void Chose(int persons, int games, int loop, int maxPart, int maxOppo, int repeat) {
        int cn = 0;
        log.Information("Round Robin double: players {p}, games {g}, times {l}. maxPart {mp}, maxOppo {mo}, repeat {r}", persons, games, loop, maxPart, maxOppo, repeat);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- loop {i}", i);
            }
            var result = CreateDbl(persons, games, false, false);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var parts = PartCount(ps);
                var oppos = OppoCount(ps, repeat);
                if (parts < maxPart && oppos < maxOppo) {
                    log.Information("Master: {m}", result.Value.mst);
                    log.Information("{i,2}: Oppo {o2} Part {p2}", ++cn, oppos, parts);
                    log.Information("{d}", DTour(result.Value.t, $"{persons}-Player {games}-Game"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {cn}", cn);
    }

    public static int OppoCount(Player[] ps, int repeat = 1) {
        return ps.Sum(p => p.Opponents.Count(o => o > repeat));
    }

    public static int PartCount(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }


}