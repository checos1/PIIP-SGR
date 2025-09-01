///<reference path="base/ModelBase.js" />

(function () {
    'use strict'

    angular.module('backbone.model').factory('EntidadModel', ['ModelBase', function (ModelBase) {
        return class EntidadModel extends ModelBase {
            constructor(data = {}) {
                super(data);
                
                this.Id = data.Id;
                this.EntidadPadreId = data.EntidadPadreId;
                this.NombreEntidad = data.NombreEntidad;
                this.Tipo = data.Tipo;
                this.Sig = data.Sig;
                this.Codigo = data.Codigo;
            }
        };
    }])
})();