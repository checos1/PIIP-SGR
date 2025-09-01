(function () {
    "use strict";


    angular.module("backbone").directive('llamarFuncionConTecla', function llamarFuncionConTecla() {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (e) {
                if (e.which === 13 || e.which === 32) {
                    e.preventDefault();

                    scope.$apply(function () {
                        scope.$eval(attrs.llamarFuncionConTecla);
                    });
                }
            });
        };
    });


})();