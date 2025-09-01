/// <reference path="base/ModelBase.js" />

(function () {
    'use strict'

    angular.module('backbone.model').factory('MapColumnaModel', ['ModelBase', function (ModelBase) {
        return class MapColumnaModel extends ModelBase {
            constructor(data = {}) {
                super(data);

                this.NombreColumna = data.NombreColumna;
                this.TipoColumna = data.TipoColumna;
                this.Estado = data.Estado;
            }
        };
    }])
})();