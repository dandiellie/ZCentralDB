(function () {
    angular
        .module('ZCentralDB')
        .controller('productsController', ['productsService', productsController]);

    function productsController(productsService) {
        var vm = this;
        vm.isLoading = false;

        vm.upload = upload;

        function loadPage() {
            //employeesService.getEmployees().then(successGetEmployees, failGetEmployees);
            //employeesService.getStores().then(successGetStores, failGetStores);
        }
        loadPage();

        function upload(result) {
            productsService.uploadProducts(result).then(successUpload, failUpload);
        }

        function successUpload(data) {

        }
        function failUpload(data) {

        }
    }
})();