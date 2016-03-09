(function () {
    'use strict';

    angular
		.module('ZCentralDB')
        .constant('UserUrls', {
		    getStores: '/api/employee/stores',
		    getEmployees: '/api/employee/all',
		    getEmployee: '/api/employee/specific',
		    postEmployee: '/api/employee/save'
		});
})();