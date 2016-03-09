(function () {
    //Dependencies here
    angular
        .module('ZCentralDB', ['ui.router'])
        .directive('ngEnter', ngEnter)
        .directive('upload', ['uploadManager', function factory(uploadManager) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    $(element).fileupload({
                        dataType: 'text',
                        add: function (e, data) {
                            uploadManager.add(data);
                        },
                        progressall: function (e, data) {
                            var progress = parseInt(data.loaded / data.total * 100, 10);
                            uploadManager.setProgress(progress);
                        },
                        done: function (e, data) {
                            uploadManager.setProgress(0);
                        }
                    });
                }
            };
        }]);

    function ngEnter() {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.ngEnter);
                    });
                    event.preventDefault();
                }
            })
        }
    }
})();