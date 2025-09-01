(function () {
    'use strict';
    angular.module('backbone').factory('avanceMetaCapServicio', avanceMetaCapServicio);

    avanceMetaCapServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function avanceMetaCapServicio($q, $http, $location, constantesBackbone) {
        return {
            actualizarAvanceMetaProducto: actualizarAvanceMetaProducto,
            consultarAvanceMetaProducto: consultarAvanceMetaProducto
        };

        function actualizarAvanceMetaProducto(Indicador) {

            var listaPeriodosActivos = [];
            Indicador.PeriodosActivos.forEach(Indi => {
                listaPeriodosActivos.push({ Vigencia: Indi.Vigencia, PeriodoProyectoId: Indi.PeriodoProyectoId, PeriodosPeriodicidadId: Indi.PeriodosPeriodicidadId, Observacion: Indi.Observacion, CantidadEjecutada: Indi.CantidadEjecutada });
            });


            const avanceMetaProductoDto = {
                IndicadorId: Indicador.IndicadorId,
                EsPrincipal: Indicador.EsPrincipal,
                ProductoId: Indicador.ProductoId,
                PeriodosActivos: listaPeriodosActivos
            };

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneActualizarAvanceMetaProducto;
            return $http.post(url, avanceMetaProductoDto);

        }

        function consultarAvanceMetaProducto(objetoParametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneConsultarAvanceMetaProducto;
            url += "?instanciaId=" + objetoParametros.instanciaId;
            url += "&proyectoId=" + objetoParametros.proyectoId;
            url += "&codigoBpin=" + objetoParametros.codigoBpin;
            url += "&vigencia=" + objetoParametros.vigencia;
            url += "&periodoPeriodicidad=" + objetoParametros.periodoPeriodicidad;
            return $http.get(url);
        }

    }
})();