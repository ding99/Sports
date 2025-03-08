using Libs.RoundRobin.Mix.Models;

namespace Libs.RoundRobin.Mix;

public partial class Planner {

    public void ReviewTour(int men, int women, int games) {
        Tour? rr = null;

        switch (men) {
        case 6:
            switch (women) {
            case 5:
                //if ()
                break;
            case 6:
                switch (games) {
                case 12:
                    rr = GetSample060612();
                    break;
                }
                break;
            }
            break;
        case 10:
            switch (women) {
            case 8:
                break;
            case 9:
                break;
            }
            break;
        }

        if (rr != null) {
            log.Information(DTour(rr, $"Sample{men}Men-{women}Women-{games}Games"));
            //TODO: display men players, women players
        } else {
            log.Error("Not found review data for {m}-men, {w}-women, {g}-games!", men, women, games);
        }
    }

    #region data
    private static Tour GetSample060612() {
        return new Tour(new([
            new(new([
                new(new(3,2), new(1,0)),
                new(new(5,3), new(4,1)),
                new(new(0,5), new(2,4))
                ])),
            new(new([
                new(new(3,0), new(5,2)),
                new(new(0,3), new(1,5)),
                new(new(4,4), new(2,1))
                ])),
            new(new([
                new(new(3,3), new(0,4)),
                new(new(5,1), new(1,2)),
                new(new(4,0), new(2,5))
                ])),
            new(new([
                new(new(3,1), new(2,3)),
                new(new(1,4), new(4,5)),
                new(new(5,0), new(0,2))
                ]))
        ]));
    }
    #endregion

}
