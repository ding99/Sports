﻿using Libs.RoundRobin.Doubles.Models;


namespace Libs.RoundRobin.Doubles; 

public partial class Planner {

    public void Chose(int persons, int games, int loop, int max) {
        int cn = 0;
        log.Information("Round Robin double: players {p}, games {games}. times {times}, maxC2 {max}", persons, games, loop, max);
        for (int i = 0; i < loop; i++) {
            if (i > 0 && i % 1000 == 0) {
                log.Information("-- loop {i}", i);
            }
            var result = CreateDbl(persons, games, false);

            if (result.IsSuccess) {
                var ps = result.Value.p;
                var c3 = Count3(ps);
                var o2 = Oppo2(ps);
                var p2 = Part2(ps);
                if (c3 < 1 && o2 < max && p2 < 1) {
                    log.Information("Master: {m}", result.Value.mst);
                    log.Information("{i,2}: C3 {c3} O2 {o2} P2 {p2}", ++cn, c3, o2, p2);
                    log.Information("{d}", DTour(result.Value.t, $"{persons}-{games}Games"));
                    log.Information("{d}", DPlayers(result.Value.p));
                }
            }
        }
        log.Information("Sum {sum}", cn);
    }

    #region util

    public int Count3(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1) + p.Opponents.Count(o => o > 2));
    }

    public int Oppo2(Player[] ps) {
        return ps.Sum(p => p.Opponents.Count(o => o > 1));
    }

    public int Part2(Player[] ps) {
        return ps.Sum(p => p.Partners.Count(o => o > 1));
    }

    #endregion

}
