(function($, dl) {

    class CounterDataResource {
        get(callback) {
            $.get('/api', (data) => {
                callback(data);
            });
        }
        sendUp(callback) {
            $.post('/api/up', (data) => {
                callback(data);
            });
        }
    }

    // exports
    dl.CounterDataResource = CounterDataResource;
})(jQuery, window['dl'] = window['dl'] || {});