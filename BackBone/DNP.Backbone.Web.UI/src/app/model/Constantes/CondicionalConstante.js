(function() {
    'use strict'

    angular.module('backbone.model')
        .constant('CondicionalConstante', [
            {
                descricion: "Igual",
                valor: 1
            },
            {
                descricion: "Menor",
                valor: 2
            },
            {
                descricion: "Menor igual",
                valor: 3
            },
            {
                descricion: "Más grande",
                valor: 4
            },
            {
                descricion: "Mayor igual",
                valor: 5
            },
            {
                descricion: "Diferente",
                valor: 6
            },
            {
                descricion: "Contiene",
                valor: 7
            }
        ]);
})();