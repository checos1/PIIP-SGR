(function() {
    'use strict'

    angular.module('backbone.model').factory('ModelBase', function() {
        return class ModelBase {
            constructor(data = {}) {
                this.Id = data.Id;
                this.FechaCreacion = data.FechaCreacion && moment.utc(data.FechaCreacion).local().toDate();
                this.CreadoPor = data.CreadoPor;
                this.FechaModificacion = data.FechaModificacion && moment.utc(data.FechaModificacion).local().toDate();
                this.ModificadoPor = data.ModificadoPor;
            }

            Base() {
                return {
                    Id: this.Id,
                    FechaCreacion: this.FechaCreacion && moment(this.FechaCreacion).toDate(),
                    CreadoPor: this.CreadoPor,
                    FechaModificacion: this.FechaModificacion && moment(this.FechaModificacion).toDate(),
                    ModificadoPor: this.ModificadoPor
                }
            }
        };
    })
})();