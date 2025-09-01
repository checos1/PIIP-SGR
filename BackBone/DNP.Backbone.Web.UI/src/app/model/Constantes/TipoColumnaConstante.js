(function() {
    'use strict'

    angular.module('backbone.model')
        .constant('TipoColumnaConstante', {
            Int: 1,
            String: 2,
            Money: 3,
            Datetime: 4,
            Date: 5
        });
})();