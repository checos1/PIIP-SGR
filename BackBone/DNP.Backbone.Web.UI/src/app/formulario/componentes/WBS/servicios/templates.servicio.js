(function () {
    'use strict';

    angular.module('backbone.formulario').factory('templatesServicio', templatesServicio);

    templatesServicio.$inject = [];

    function templatesServicio() {
        return {
            tieneHijosTipoArray: tieneHijosTipoArray
        };

        function tieneHijosTipoArray(data) {
            if (data && data.items) {
                for (var i = 0; i < data.items.length; i++) {
                    if (data.items[i].tipo === 'array') return true;
                }
            }
            return false;
        }
    }
})();