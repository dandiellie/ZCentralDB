(function () {
    angular
        .module('ZCentralDB')
        .config(['$httpProvider', '$locationProvider', '$stateProvider', '$urlRouterProvider', Config]);

    function Config($httpProvider, $locationProvider, $stateProvider, $urlRouterProvider) {
        $httpProvider.interceptors.push('authService');
        $locationProvider.html5Mode(true);

        $urlRouterProvider.otherwise('/');

        $stateProvider
            .state('index', { // index
                url: '/',
                templateUrl: 'app/components/index/index.html',
                controller: 'indexController as vm'
            }).state('login', { // login
                url: '/login',
                templateUrl: 'app/components/login/login.html',
                controller: 'loginController as vm'
            }).state('products', { // products
                abstract: true,
                url: '/products',
                templateUrl: 'app/components/products/products.html',
                controller: 'productsController as vm'
            }).state('products.upload', {
                url: '/upload',
                templateUrl: 'app/components/products/views/upload.html'
            }).state('products.all', {
                url: '/all',
                templateUrl: 'app/components/products/views/all.html'
            }).state('products.find', {
                url: '/find',
                templateUrl: 'app/components/products/views/find.html'
            });
    }

})();