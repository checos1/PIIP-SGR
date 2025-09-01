(function () {
    'use strict';
    angular.module('backbone').factory('focalizacionAjustesSgrServicio', focalizacionAjustesSgrServicio);

    focalizacionAjustesSgrServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function focalizacionAjustesSgrServicio($q, $http, $location, constantesBackbone) {
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
            obtenerListaRecursos: obtenerListaRecursos,
        };
        function obtenerPoliticasTransversalesProyecto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesProyectoSgr + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function eliminarPoliticasProyecto(proyectoId, politicaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneEliminarPoliticasProyectoSgr + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }
        function guardarPoliticasTransversalesAjustes(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneAgregarPoliticasTransversalesAjustesSgr + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
        function obtenerListaRecursos(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaTiposRecursos;
            return $http.post(url, peticion);
        }
        function ConsultarPoliticasCategoriasIndicadores(instanciaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarPoliticasCategoriasIndicadoresSgr + "?instanciaId=" + instanciaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function obtenerPoliticasTransversalesCategorias(instanciaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesCategoriasSgr + "?instanciaId=" + instanciaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function eliminarCategoriaPoliticasProyecto(proyectoId, politicaId, categoriaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneEliminarCategoriasPoliticasProyectoSgr + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&categoriaId=" + categoriaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }
        function ModificarPoliticasCategoriasIndicadores(Parametros, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneModificarPoliticasCategoriasIndicadoresSgr + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, Parametros);
        }
        function ObtenerCrucePoliticasAjustes(instanciaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerCrucePoliticasAjustesSgr + "?instanciaId=" + instanciaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function GuardarCrucePoliticasAjustes(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarCrucePoliticasAjustesSgr + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
        function ObtenerPoliticasTransversalesResumen(instanciaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesResumenSgr + "?instanciaId=" + instanciaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }



        function ConsultarPoliticasCategoriasPorPadre(idPadre, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonePoliticasCategoriaPorPadre + "?idPadre=" + idPadre + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }
        function ConsultarCategoriasSubcategorias(idPadre, idEntidad, esCategoria, esGrupoEtnico, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCategoriasSubcategorias + "?idPadre=" + idPadre + "&idEntidad=" + idEntidad + "&esCategoria=" + esCategoria + "&esGrupoEtnico=" + esGrupoEtnico + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }
        function guardarFocalizacionCategorias(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarFocalizacionCategoriasAjustesSgr + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
        function guardarFocalizacionCategoriasPolitica(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneguardarFocalizacionCategoriasPolitica + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
        function ObtenerPoliticasSolicitudConcepto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPoliticasSolicitudConcepto + "?bpin=" + BpinMock + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function FocalizacionSolicitarConceptoDT(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneFocalizacionSolicitarConceptoDT;
            return $http.post(url, parametros);
        }
        function ObtenerUsuariosRValidadorPoliticaTransversal(peticion) {
            return $http.post(apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerUsuariosRValidadorPoliticaTransversal, peticion);
        }
        function ObtenerDireccionesTecnicasPoliticasFocalizacion(usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerDireccionesTecnicasPoliticasFocalizacion + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function ObtenerResumenSolicitudConcepto(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerResumenSolicitudConcepto + "?bpin=" + BpinMock + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function ObtenerPreguntasEnvioPoliticaSubDireccion(parametros) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerPreguntasEnvioPoliticaSubDireccion;
            return $http.post(url, parametros);
        }

    }
})();