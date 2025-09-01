///<reference path="base/ModelBase.js" />

(function () {
    'use strict'

    angular.module('backbone.model').factory('RoleModel', ['ModelBase', function (ModelBase) {
        return class RoleModel extends ModelBase {
            constructor(data = {}) {
                super(data);
                
                this.Id = data.Id;
                this.NombreRol = data.NombreRol;
                this.IdMensaje = data.IdMensaje;
            }
        };
    }])
})();