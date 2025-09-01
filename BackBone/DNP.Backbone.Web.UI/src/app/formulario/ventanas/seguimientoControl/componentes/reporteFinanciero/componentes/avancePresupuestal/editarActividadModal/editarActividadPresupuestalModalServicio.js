(function () {
    'use strict';
    angular.module('backbone').factory('editarActividadPresupuestalModalServicio', editarActividadPresupuestalModalServicio);

    editarActividadPresupuestalModalServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function editarActividadPresupuestalModalServicio($q, $http, $location, constantesBackbone) {
        return {
            editarActividad: editarActividad
        };

        function editarActividad(model) {
            console.log("editarActividad", model)
            const data = {
                ActividadId: model.ActividadId,
                SeguimientoEntregableId: model.SeguimientoEntregableId,
                ActividadProgramacionSeguimientoId: model.ActividadProgramacionSeguimientoId,
                PredecesoraId: model.ActividadPredecesora.ActividadId,
                SeguimientoEntregablePredecesoraId: model.ActividadPredecesora.SeguimientoEntregableId,
                TipoSigla: model.TipoSigla,
                CantidadTotal: model.CantidadTotal,
                CostoTotal: model.CostoTotal,
                DuracionOptimista: model.DuracionOptimista,
                DuracionPesimista: model.DuracionPesimista,
                DuracionProbable: model.DuracionProbable,
                UnidadMedidaId: model.UnidadMedidaId,
                PosPosicion: model.PosPosicion,
                Adelanto: model.Adelanto,
                Bpin: model.Bpin,
                ProyectoId: model.ProyectoId
            };
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.uriEditarProgramarActividad, data);
        }
    }
})();