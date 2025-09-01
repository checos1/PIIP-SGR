angular.module('backbone.core.directives')
    .directive('modalDialog', function () {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            templateUrl: 'src/app/comunes/directives/modal-dialog/modalDialog.html',
            scope: {
                guardar: '=?',
                cerrar: '=',
                txtCerrar: '@',
                mostraGuardar: '=',
                txtGuardar: '@'
            }
        };
    });