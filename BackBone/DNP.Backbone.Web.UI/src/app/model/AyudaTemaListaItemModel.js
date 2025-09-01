
///<reference path="base/ModelBase.js" />

(function () {
    'use strict'

    angular.module('backbone.model').factory('AyudaTemaListaItemModel', ['ModelBase', function (ModelBase) {
        return class AyudaTemaListaItemModel extends ModelBase {
            constructor(data = {}) {
                super(data);
                this.IdAyudaTemaPadre = data.IdAyudaTemaPadre;
                this.AyudaTipoEnum = data.AyudaTipoEnum;
                this.Nombre = data.Nombre;
                this.Descripcion = data.Descripcion;
                this.Activo = data.Activo;
                this.Contenido = data.Contenido;
                this.TemaGeneralId = data.TemaGeneralId;
                this.SubItems = (data.SubItems || []).map(x => new AyudaTemaListaItemModel(x));
            }

            comparar = function (otroItemAyuda) {
                return this.Activo != otroItemAyuda.Activo
                        || this.Contenido != otroItemAyuda.Contenido
                        || this.Descripcion != otroItemAyuda.Descripcion
                        || this.Nombre != otroItemAyuda.Nombre;
            }
        };
    }])
})();