(function() {
    'use strict'

    angular.module('backbone.model')
        .constant('ClassificacionAlertaConstante', [
            {
                descricion: 'Mensaje',
                valor: 1,
                tipo: 'success'
            },
            {
                descricion: 'Informativo',
                valor: 2,
                tipo: 'warning'
            },
            {
                descricion: 'Alerta',
                valor: 3,
                tipo: 'danger'
            }
        ]);
})();