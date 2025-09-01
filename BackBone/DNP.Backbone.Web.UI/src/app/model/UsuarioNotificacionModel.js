///<reference path="base/ModelBase.js" />

(function () {
    'use strict'

    angular.module('backbone.model').factory('UsuarioNotificacionModel', ['ModelBase', 
        function (ModelBase) {
            return class UsuarioNotificacionModel extends ModelBase {
                constructor(data = {}) {
                    super(data);
                    
                    this.IdUsuarioDNP = data.IdUsuarioDNP;
                    this.NombreUsuario = data.NombreUsuario;
                    this.UsuarioConfigNotificacionId = data.UsuarioConfigNotificacionId;
                    this.Visible = data.Visible;
                    this.UsuarioYaLeyo = data.UsuarioYaLeyo;
                }
            };
    }])
})();