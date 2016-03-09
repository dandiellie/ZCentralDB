(function () {
    angular
        .module('ZCentralDB')
        .controller('productsController', [productsController]);

    function productsController() {
        var vm = this;
        vm.isLoading = false;

        function loadPage() {
            //employeesService.getEmployees().then(successGetEmployees, failGetEmployees);
            //employeesService.getStores().then(successGetStores, failGetStores);
        }
        loadPage();
    }
})();