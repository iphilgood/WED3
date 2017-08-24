;(function($) {

    ajaxUtil = window.ajax;
    var token = undefined;

    function login(userName, pwd) {
        return ajaxUtil.ajax("POST", "/api/tokens", { username: userName, password: pwd })
            .then(function (msg) {
                token = msg.token;
            });
    }

    function logout() {
        token = undefined;
        return Promise.resolve();
    }

    function createPizza(pizzeName) {
        return ajaxUtil.ajax("POST", "/api/orders", { name : pizzeName}, { "Authorization": "Bearer " + token })
         .done(function (msg) {
                console.log(msg);
            });
    }

    function isLoggedIn() {
        return !!token;
    }

    window.restClient = { login: login, logout: logout, createPizza: createPizza, isLogin: isLoggedIn };
}(jQuery));