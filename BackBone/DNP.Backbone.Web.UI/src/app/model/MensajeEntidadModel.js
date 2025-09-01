/// <reference path="base/ModelBase.js" />

(function () {
    'use strict'

    angular.module('backbone.model').factory('MensajeEntidadModel', ['ModelBase', function (ModelBase) {
        return class MensajeEntidadModel extends ModelBase {
            constructor(data = {}) {
                super(data);

                this.NombreEntidad = data.NombreEntidad;
                this.IdMensaje = data.IdMensaje;
            }
        };
    }])
})();