(function () {
    'use strict';
    angular.module('backbone').factory('focalizacionAjustesSinTramiteSgpServicio', focalizacionAjustesSinTramiteSgpServicio);

    focalizacionAjustesSinTramiteSgpServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function focalizacionAjustesSinTramiteSgpServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerPoliticasTransversalesProyecto: obtenerPoliticasTransversalesProyecto,
            guardarPoliticasTransversalesAjustes: guardarPoliticasTransversalesAjustes,
            guardarFocalizacionCategorias: guardarFocalizacionCategorias,
            guardarFocalizacionCategoriasPolitica: guardarFocalizacionCategoriasPolitica,
            obtenerPoliticasTransversalesCategorias: obtenerPoliticasTransversalesCategorias,
            eliminarPoliticasProyecto: eliminarPoliticasProyecto,
            eliminarCategoriaPoliticasProyecto: eliminarCategoriaPoliticasProyecto,
            ConsultarPoliticasCategoriasPorPadre: ConsultarPoliticasCategoriasPorPadre,
            ConsultarCategoriasSubcategorias: ConsultarCategoriasSubcategorias,
            ObtenerPoliticasTransversalesResumen: ObtenerPoliticasTransversalesResumen,
            ConsultarPoliticasCategoriasIndicadores: ConsultarPoliticasCategoriasIndicadores,
            ObtenerCrucePoliticasAjustes: ObtenerCrucePoliticasAjustes,
            GuardarCrucePoliticasAjustes: GuardarCrucePoliticasAjustes,
            ModificarPoliticasCategoriasIndicadores: ModificarPoliticasCategoriasIndicadores,
            ObtenerPoliticasSolicitudConcepto: ObtenerPoliticasSolicitudConcepto,
            FocalizacionSolicitarConceptoDT: FocalizacionSolicitarConceptoDT,
            ObtenerUsuariosRValidadorPoliticaTransversal: ObtenerUsuariosRValidadorPoliticaTransversal,
            ObtenerDireccionesTecnicasPoliticasFocalizacion: ObtenerDireccionesTecnicasPoliticasFocalizacion,
            ObtenerResumenSolicitudConcepto: ObtenerResumenSolicitudConcepto,
            ObtenerPreguntasEnvioPoliticaSubDireccion: ObtenerPreguntasEnvioPoliticaSubDireccion,
            GuardarPreguntasEnvioPoliticaSubDireccionAjustes: GuardarPreguntasEnvioPoliticaSubDireccionAjustes,
            GuardarRespuestaEnvioPoliticaSubDireccionAjustes: GuardarRespuestaEnvioPoliticaSubDireccionAjustes,
        };

        function obtenerPoliticasTransversalesProyecto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonePoliticasTransversalesAConsultar + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function guardarPoliticasTransversalesAjustes(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonePoliticasTransversalesAgregar + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function ConsultarPoliticasCategoriasPorPadre(idPadre, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonePoliticasCategoriaPorPadre + "?idPadre=" + idPadre + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

        function ConsultarCategoriasSubcategorias(idPadre, idEntidad,esCategoria, esGrupoEtnico, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCategoriasSubcategorias + "?idPadre=" + idPadre + "&idEntidad=" + idEntidad + "&esCategoria=" + esCategoria + "&esGrupoEtnico=" + esGrupoEtnico + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }

        function guardarFocalizacionCategorias(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneguardarFocalizacionCategoriasAjustes + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function guardarFocalizacionCategoriasPolitica(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneguardarFocalizacionCategoriasPolitica + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function obtenerPoliticasTransversalesCategorias(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonePoliticasTransversalesCategorias + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function eliminarPoliticasProyecto(proyectoId, politicaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarPoliticaProyecto + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }

        function eliminarCategoriaPoliticasProyecto(proyectoId, politicaId, categoriaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneEliminarCategoriaPoliticaProyecto + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&categoriaId=" + categoriaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }

        function ObtenerPoliticasTransversalesResumen(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPoliticasTransversalesResumen + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ConsultarPoliticasCategoriasIndicadores(Bpin, usuarioDNP, idFormulario) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPoliticasCategoriasIndicadores + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ModificarPoliticasCategoriasIndicadores(Parametros, usuarioDNP, idFormulario) {

            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneModificarPoliticasCategoriasIndicadores + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, Parametros);
        }

        function ObtenerCrucePoliticasAjustes(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCrucePoliticasAjustes + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function GuardarCrucePoliticasAjustes(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarCrucePoliticasAjustes + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }

        function ObtenerPoliticasSolicitudConcepto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPoliticasSolicitudConcepto + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function FocalizacionSolicitarConceptoDT(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFocalizacionSolicitarConceptoDT;
            return $http.post(url, parametros);
        }

        function ObtenerUsuariosRValidadorPoliticaTransversal(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosRValidadorPoliticaTransversal, peticion);
        }

        function ObtenerDireccionesTecnicasPoliticasFocalizacion( usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDireccionesTecnicasPoliticasFocalizacion + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ObtenerResumenSolicitudConcepto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerResumenSolicitudConcepto + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ObtenerPreguntasEnvioPoliticaSubDireccion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasEnvioPoliticaSubDireccion;
            return $http.post(url,parametros);
        }

        function GuardarPreguntasEnvioPoliticaSubDireccionAjustes(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarPreguntasEnvioPoliticaSubDireccionAjustes;
            return $http.post(url, parametros);
        }

        function GuardarRespuestaEnvioPoliticaSubDireccionAjustes(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneGuardarRespuestaEnvioPoliticaSubDireccionAjustes;
            return $http.post(url, parametros);
        }
                      
    }
})();