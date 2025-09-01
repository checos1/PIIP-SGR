/// <reference path="base/ModelBase.js" />
/// <reference path="RoleModel.js"/>

(function () {
    'use strict'

    angular.module('backbone.model').factory('MensajeMantenimientoModel', ['ModelBase', 'RoleModel', 'MensajeEntidadModel', 
        function (ModelBase, RoleModel, MensajeEntidadModel) {
            return class MensajeMantenimientoModel extends ModelBase {
                constructor(data = {}) {
                    super(data);

                    this.TipoEntidad = data.TipoEntidad;
                    this.NombreMensaje = data.NombreMensaje;
                    this.FechaCreacionInicio = data.FechaCreacionInicio && moment.utc(data.FechaCreacionInicio).local().toDate();
                    this.FechaCreacionFin = data.FechaCreacionFin && moment.utc(data.FechaCreacionFin).local().toDate();
                    this.EstadoMensaje = data.EstadoMensaje;
                    this.MensajeTemplate = data.MensajeTemplate;
                    this.TipoMensaje = data.TipoMensaje;
                    this.RestringeAcesso = data.RestringeAcesso;
                    this.Roles = (data.Roles || []).map(x => new RoleModel(x));
                    this.Entidades = (data.Entidades || []).map(x => new MensajeEntidadModel(x));
                }

                get EstiloTipoMensaje() {
                    const types = {
                        1: "info",
                        2: "warning",
                        3: "danger"
                    }
                    
                    if(this.RestringeAcesso)
                        return types[3];

                    return types[this.TipoMensaje];
                }

                get TipoMensajeDesc() {
                    const descriciones = {
                        1: "PopUp",
                        2: "Disclaimer",
                        3: "Restringe"
                    }

                    if(this.RestringeAcesso)
                        return descriciones[3];

                    return descriciones[this.TipoMensaje];
                }

                comparar = function (otroMensaje) {
                    return this.TipoEntidad != otroMensaje.TipoEntidad
                            || this.MensajeTemplate != otroMensaje.MensajeTemplate
                            || this.TipoMensaje != otroMensaje.TipoMensaje
                            || this.RestringeAcesso != otroMensaje.RestringeAcesso;
                }

                toJSON() {
                    return {
                        ...this.Base(),
                        TipoEntidad: this.TipoEntidad,
                        NombreMensaje: this.NombreMensaje,
                        FechaCreacionInicio: this.FechaCreacionInicio && moment(this.FechaCreacionInicio).toDate(),
                        FechaCreacionFin: this.FechaCreacionFin && moment(this.FechaCreacionFin).toDate(),
                        EstadoMensaje: this.EstadoMensaje,
                        MensajeTemplate: this.MensajeTemplate,
                        TipoMensaje: this.TipoMensaje,
                        RestringeAcesso: this.RestringeAcesso,
                        Roles: this.Roles || [],
                        Entidades: this.Entidades || []
                    }
                }
            };
    
        }
    ])
})();