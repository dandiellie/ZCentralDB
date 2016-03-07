(function () {
    'use strict';

    angular
		.module('ZCentralDB')
        .constant('EmployeeUrls', {
		    getStores: '/api/employee/stores',
		    getEmployees: '/api/employee/all',
		    getEmployee: '/api/employee/specific',
		    postEmployee: '/api/employee/save'
		});
})();