(function () {
    angular
		.module('ZCentralDB')
        .service('productsService', ['$q', '$http', productsService])

    function productsService($q, $http) {
        var service = {};

        //service.getStores = getStores;
        //service.getEmployees = getEmployees;
        //service.getEmployee = getEmployee;
        service.uploadProducts = uploadProducts;

        //function getStores() {
        //    var deferred = $q.defer();
        //    $http({
        //        url: UserUrls.base + EmployeeUrls.getStores + "?locationID=" + 0,
        //        method: 'GET'
        //    }).success(function (result) {
        //        deferred.resolve(result);
        //    }).error(function () {
        //        deferred.reject();
        //    });
        //    return deferred.promise;
        //}
        //function getEmployees() {
        //    var deferred = $q.defer();
        //    $http({
        //        url: UserUrls.base + EmployeeUrls.getEmployees + "?locationID=" + 0,
        //        method: 'GET'
        //    }).success(function (result) {
        //        deferred.resolve(result);
        //    }).error(function () {
        //        deferred.reject();
        //    });
        //    return deferred.promise;
        //}
        //function getEmployee(email) {
        //    var deferred = $q.defer();
        //    $http({
        //        url: UserUrls.base + EmployeeUrls.getEmployee + "?email=" + email,
        //        method: 'GET'
        //    }).success(function (result) {
        //        deferred.resolve(result);
        //    }).error(function () {
        //        deferred.reject();
        //    });
        //    return deferred.promise;
        //}
        function uploadProducts(vm) {
            var deferred = $q.defer();

            $http({
                url: '/api/products/upload',
                method: 'POST',
                data: vm
            }).success(function (result) {
                deferred.resolve();
            }).error(function (result) {
                deferred.reject();
            });

            return deferred.promise;
        }

        return service;
    };
})();