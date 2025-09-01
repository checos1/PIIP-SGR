(function () {
    "use strict";

    angular.module("backbone.archivo.directivas", []);
    angular.module("backbone").directive("maxValue", function() {
        return {
            restrict: "A",
            link: function(scope, elem, attrs) {
                var limit = parseInt(attrs.maxValue);
                angular.element(elem).on("keypress", function(e) {
                    if (Number(this.value.concat(e.key))>limit) e.preventDefault();
                });
            }
        }
    }).directive("minValue", function() {
        return {
            restrict: "A",
            link: function(scope, elem, attrs) {
                var limit = parseInt(attrs.minValue);
                angular.element(elem).on("keyup", function() {
                    if (Number(this.value)<limit) this.value = 2;
                });
            }
        }
    }).directive("noEspecialCarac", function() {
        return {
            restrict: "A",
            link: function(scope, elem, attrs) {
// ReSharper disable once UnusedLocals
                var limit = parseInt(attrs.minValue);
                angular.element(elem).on("keypress", function(e) {
                    if(e.keyCode === 43 || e.keyCode === 44 || e.keyCode === 45){
                        e.preventDefault();
                    }
                });
            }
        }
    });
}());