(function () {
    'use strict';

    angular.module('backbone').factory('focalizacionAjustesSGPServicio', focalizacionAjustesSGPServicio);

    focalizacionAjustesSGPServicio.$inject = ['$q', '$http', '$location', 'constantesBackbone'];

    function focalizacionAjustesSGPServicio($q, $http, $location, constantesBackbone) {
        return {
            obtenerPoliticasTransversalesProyectoSGP: obtenerPoliticasTransversalesProyectoSGP,
            guardarPoliticasTransversalesAjustesSGP: guardarPoliticasTransversalesAjustesSGP,
            guardarFocalizacionCategoriasSGP: guardarFocalizacionCategoriasSGP,
            guardarFocalizacionCategoriasPoliticaSGP: guardarFocalizacionCategoriasPoliticaSGP,
            obtenerPoliticasTransversalesCategoriasSGP: obtenerPoliticasTransversalesCategoriasSGP,
            eliminarPoliticasProyectoSGP: eliminarPoliticasProyectoSGP,
            eliminarCategoriaPoliticasProyectoSGP: eliminarCategoriaPoliticasProyectoSGP,
            ConsultarPoliticasCategoriasPorPadre: ConsultarPoliticasCategoriasPorPadre,
            ConsultarCategoriasSubcategoriasSGP: ConsultarCategoriasSubcategoriasSGP,
            ObtenerPoliticasTransversalesResumenSGP: ObtenerPoliticasTransversalesResumenSGP,
            ConsultarPoliticasCategoriasIndicadoresSGP: ConsultarPoliticasCategoriasIndicadoresSGP,
            ObtenerCrucePoliticasAjustesSGP: ObtenerCrucePoliticasAjustesSGP,
            GuardarCrucePoliticasAjustesSGP: GuardarCrucePoliticasAjustesSGP,
            ModificarPoliticasCategoriasIndicadoresSGP: ModificarPoliticasCategoriasIndicadoresSGP,
            ObtenerPoliticasSolicitudConcepto: ObtenerPoliticasSolicitudConcepto,
            FocalizacionSolicitarConceptoDT: FocalizacionSolicitarConceptoDT,
            ObtenerUsuariosRValidadorPoliticaTransversal: ObtenerUsuariosRValidadorPoliticaTransversal,
            ObtenerDireccionesTecnicasPoliticasFocalizacion: ObtenerDireccionesTecnicasPoliticasFocalizacion,
            ObtenerResumenSolicitudConcepto: ObtenerResumenSolicitudConcepto,
            ObtenerPreguntasEnvioPoliticaSubDireccion: ObtenerPreguntasEnvioPoliticaSubDireccion,
            obtenerListaRecursosSGP: obtenerListaRecursosSGP,
        };
        function obtenerPoliticasTransversalesProyectoSGP(Bpin, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesProyectoSGP + "?bpin=" + Bpin + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function eliminarPoliticasProyectoSGP(proyectoId, politicaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneEliminarPoliticasProyectoSGP + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }
        function guardarPoliticasTransversalesAjustesSGP(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneAgregarPoliticasTransversalesAjustesSGP + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
        function obtenerListaRecursosSGP(peticion) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerProyectoListaTiposRecursosSGP;
            return $http.post(url, peticion);
        }
        function ConsultarPoliticasCategoriasIndicadoresSGP(instanciaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneConsultarPoliticasCategoriasIndicadoresSGP + "?instanciaId=" + instanciaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function obtenerPoliticasTransversalesCategoriasSGP(instanciaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesCategoriasSGP + "?instanciaId=" + instanciaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function eliminarCategoriaPoliticasProyectoSGP(proyectoId, politicaId, categoriaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneEliminarCategoriasPoliticasProyectoSGP + "?proyectoId=" + proyectoId + "&politicaId=" + politicaId + "&categoriaId=" + categoriaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url);
        }
        function ModificarPoliticasCategoriasIndicadoresSGP(Parametros, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneModificarPoliticasCategoriasIndicadoresSGP + "?usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.post(url, Parametros);
        }
        function ObtenerCrucePoliticasAjustesSGP(instanciaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerCrucePoliticasAjustesSGP + "?instanciaId=" + instanciaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }
        function GuardarCrucePoliticasAjustesSGP(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarCrucePoliticasAjustesSGP + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
        function ObtenerPoliticasTransversalesResumenSGP(instanciaId, usuarioDNP, idFormulario) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneObtenerPoliticasTransversalesResumenSGP + "?instanciaId=" + instanciaId + "&usuarioDNP=" + usuarioDNP + "&tokenAutorizacion=" + idFormulario;
            return $http.get(url);
        }

        function ConsultarPoliticasCategoriasPorPadre(idPadre, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackbonePoliticasCategoriaPorPadre + "?idPadre=" + idPadre + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }
        function ConsultarCategoriasSubcategoriasSGP(idPadre, idEntidad, esCategoria, esGrupoEtnico, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneObtenerCategoriasSubcategoriasSGP + "?idPadre=" + idPadre + "&idEntidad=" + idEntidad + "&esCategoria=" + esCategoria + "&esGrupoEtnico=" + esGrupoEtnico + "&usuarioDNP=" + usuarioDNP;
            return $http.get(url);
        }
        function guardarFocalizacionCategoriasSGP(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackBoneGuardarFocalizacionCategoriasAjustesSGP + "?usuarioDNP=" + usuarioDNP;
            return $http.post(url, parametros);
        }
        function guardarFocalizacionCategoriasPoliticaSGP(parametros, usuarioDNP) {
            var url = apiBackboneServicioBaseUri + constantesBackbone.apiBackboneguardarFocalizacionCategoriasPoliticaSGP + "?usuarioDNP=" + usuarioDNP;
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