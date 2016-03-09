(function () {
    angular
        .module('ZCentralDB')
        .controller('productsController', ['uploadManager', '$scope', '$rootScope', productsController]);

    function productsController(uploadManager, $scope, $rootScope) {
        var vm = this;
        vm.isLoading = false;
        vm.files = [];
        vm.percentage = 0;
        vm.upload = function () {
            uploadManager.upload();
            vm.files = [];
        };
        $rootScope.$on('fileAdded', function (e, call) {
            vm.files.push(call);
            $scope.$apply();
        });
        $rootScope.$on('uploadProgress', function (e, call) {
            vm.percentage = call;
            $scope.$apply();
        });

        function loadPage() {
            //employeesService.getEmployees().then(successGetEmployees, failGetEmployees);
            //employeesService.getStores().then(successGetStores, failGetStores);
        }
        loadPage();
    }
})();