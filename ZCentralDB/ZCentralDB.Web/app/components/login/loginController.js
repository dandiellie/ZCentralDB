(function () {
    'use strict';

    angular
        .module('ZCentralDB')
        .controller('loginController', ['loginService', '$location', loginController]);

    function loginController(loginService, $location) {
        var vm = this;
        vm.isLoading = false;

        vm.login = login;
        vm.isLoggedIn = isLoggedIn;
        vm.logout = logout;

        function login() {
            vm.isLoading = true;
            loginService.login(vm.username, vm.password).then(successLogin, failLogin);
        }
        function isLoggedIn() {
            return loginService.isLoggedIn();
        }
        function logout() {
            loginService.logout();
            $location.path('/');
        }

        function successLogin(data) {
            $location.path('/index');
            vm.isLoading = false;
        }
        function failLogin(data) {
            
        }
    }
})();