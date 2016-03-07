(function () {
    angular
        .module('ZCentralDB')
        .config(['$httpProvider', '$locationProvider', '$stateProvider', '$urlRouterProvider', Config]);

    function Config($httpProvider, $locationProvider, $stateProvider, $urlRouterProvider) {
        //$httpProvider.interceptors.push('authService');
        $locationProvider.html5Mode(true);

        $urlRouterProvider.otherwise('/');

        $stateProvider
            .state('index', { // index
                url: '/',
                templateUrl: 'app/components/index/index.html',
                controller: 'indexController as vm'
            });
    }

})();