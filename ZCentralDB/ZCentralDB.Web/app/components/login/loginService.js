(function () {
    'use strict';

    angular
        .module('ZCentralDB')
        .factory('authService', ['$window', '$q', authService])
        .factory('loginService', ['$http', '$q', '$window', loginService]);

    function authService($window, $q) {
        var service = {};

        service.request = request;

        function request(config) {
            config.headers = config.headers || {};
            if ($window.sessionStorage.getItem('token')) {
                config.headers.Authorization = 'Bearer ' + $window.sessionStorage.getItem('token');
            }

            return config || $q.when(config);
        }

        return service;
    }

    function loginService($http, $q, $window) {
        var service = {};

        service.login = login;
        service.isLoggedIn = isLoggedIn;
        service.logout = logout;
        service.register = register;

        function login(username, password) {
            var deferred = $q.defer();

            $http({
                url: '/Token',
                method: 'POST',
                data: 'username=' + username + '&password=' + password + '&grant_type=password',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
            }).success(function (data) {
                $window.sessionStorage.setItem('token', data.access_token);
                deferred.resolve();
            }).error(function (data) {
                deferred.reject();
            });

            return deferred.promise;
        }
        function isLoggedIn() {
            return $window.sessionStorage.getItem('token');
        }
        function logout() {
            $window.sessionStorage.removeItem('token');
        }
        function register(email, password, confirmPassword) {
            var deferred = $q.defer();

            $http({
                url: '/api/Account/Register',
                method: 'POST',
                data: { 'email': email, 'password': password, 'confirmPassword': confirmPassword }
            }).success(function (data) {
                deferred.resolve();
            }).error(function (data) {
                deferred.reject();
            });

            return deferred.promise;
        }

        return service;
    }
})();