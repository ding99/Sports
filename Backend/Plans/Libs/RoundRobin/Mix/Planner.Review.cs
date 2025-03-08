using CSharpFunctionalExtensions;

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
        } else {
            log.Error("Not found review data for {m}-men, {w}-women, {g}-games!", men, women, games);
        }
    }

    #region data
    #endregion

}
