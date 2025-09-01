angular.module('backbone.core.filtros', [])
    .filter('language', function () {
        return function (input) {
            if (input) {
                try {
                    return Code52.Language.Dictionary[input];
                } catch (e) {
                    return "Recurso no encontrado.";
                }
            }

            return "";
        };
    }).filter('tipoDatoFiltro', function () {
        return function (input, entity) {
            return entity.TipoDatoNombre;
        };
    }).filter('siNo', function () {
        return function (input) {
            return input ? 'Si' : 'No';
        };
    }).filter('currencyCellFilter', function ($filter) {
        return function (input) {
            return input.toLocaleString('en-US');
        };
    });