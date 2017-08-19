(function($, bl) {

    /**
     * Model
     */
    class CounterModel {
        private team;
        private count;
        constructor(team, count) {
            this.team = team || "unspecified";
            this.count = count || 0;
        }
        static fromDto(dto) {
            return new CounterModel(dto.team, dto.count);
        }
    }

    /**
     * Service
     */
    class CounterService {
        private counterDataResource;
        constructor(counterDataResource) {
            this.counterDataResource = counterDataResource;
        }
        load(callback) {
            this.counterDataResource.get((dto) => {
                callback(CounterModel.fromDto(dto));
            });
        }
        up(callback) {
            this.counterDataResource.sendUp((dto) => {
                callback(CounterModel.fromDto(dto));
            });
        }
    }

    // exports
    bl.CounterModel = CounterModel;
    bl.CounterService = CounterService;

})(jQuery, window['bl'] = window['bl'] || {});