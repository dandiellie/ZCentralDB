(function () {
    'use strict';

    angular
		.module('ZCentralDB')
        .constant('ProductUrls', {
            postProducts: '/api/products/upload',
            getForgotPassword: '/api/login/forgotPassword'
        });
})();