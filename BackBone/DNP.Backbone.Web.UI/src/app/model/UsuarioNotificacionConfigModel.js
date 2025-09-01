///<reference path="base/ModelBase.js" />
///<reference path="ProcedimientoAlmacenadoModel.js" />
///<reference path="UsuarioNotificacionModel.js" />

(function () {
    'use strict'

    angular.module('backbone.model').factory('UsuarioNotificacionConfigModel', ['ModelBase', 'UsuarioNotificacionModel', 'ProcedimientoAlmacenadoModel' , 
        function (ModelBase, UsuarioNotificacionModel, ProcedimientoAlmacenadoModel) {
            return class UsuarioNotificacionConfigModel extends ModelBase {
                constructor(data = {}) {
                    super(data);
                    
                    this.NombreNotificacion = data.NombreNotificacion;
                    this.FechaInicio = data.FechaInicio && moment.utc(data.FechaInicio).local().toDate();
                    this.FechaFin = data.FechaFin && moment.utc(data.FechaFin).local().toDate();
                    this.EsManual = data.EsManual == null && data.EsManual == undefined && true || data.EsManual;
                    this.ContenidoNotificacion = data.ContenidoNotificacion;
                    this.Tipo = data.Tipo;
                    this.NombreArchivo = data.NombreArchivo;
                    this.ProcedimientoAlmacenadoId = data.ProcedimientoAlmacenadoId
                    this.ProcedimientoAlmacenado = data.ProcedimientoAlmacenado && new ProcedimientoAlmacenadoModel(data.ProcedimientoAlmacenado);
                    this.UsuarioNotificaciones = (data.UsuarioNotificaciones || []).map(x => new UsuarioNotificacionModel(x));
                    this.IdArchivo = data.IdArchivo;
                }

                get NombreTipo() {
                    const descriciones = {
                        1: "No Prioritário",
                        2: "Prioritário",
                        3: "Urgente"
                    }

                    return descriciones[this.Tipo];
                }
            };
    }])
})();